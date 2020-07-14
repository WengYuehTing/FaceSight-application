/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal.Record
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class ImageEncoder : IEncoder
    {
        RenderTexture m_CurrentFrame;
        Queue<AsyncGPUReadbackRequest> m_Requests;
        Queue<CaptureTask> m_Tasks;
        CameraParameters m_CameraParameters;
        // A temp texture for capture.
        Texture2D m_EncodeTempTex = null;

        #region Interface
        public void Commit(RenderTexture rt, ulong timestamp)
        {
            m_CurrentFrame = rt;
        }

        public void Config(CameraParameters param)
        {
            this.m_CameraParameters = param;
            m_Requests = new Queue<AsyncGPUReadbackRequest>();
            m_Tasks = new Queue<CaptureTask>();
        }

        public void Start()
        {
            NRKernalUpdater.Instance.OnUpdate -= Update;
            NRKernalUpdater.Instance.OnUpdate += Update;
        }

        public void Stop()
        {
            NRKernalUpdater.Instance.OnUpdate -= Update;
        }

        public void Release()
        {
            if (m_EncodeTempTex != null)
            {
                GameObject.Destroy(m_EncodeTempTex);
                m_EncodeTempTex = null;
            }
        }
        #endregion

        public void Commit(CaptureTask task)
        {
            if (m_CurrentFrame != null)
            {
                m_Requests.Enqueue(AsyncGPUReadback.Request(m_CurrentFrame));
                m_Tasks.Enqueue(task);
            }
            else
            {
                Debug.LogWarning("[ImageEncoder] Lost frame data.");
            }
        }

        private void Update()
        {
            while (m_Requests.Count > 0)
            {
                var req = m_Requests.Peek();
                var task = m_Tasks.Peek();

                if (req.hasError)
                {
                    Debug.Log("GPU readback error detected");
                    m_Requests.Dequeue();

                    CommitResult(null, task);
                    m_Tasks.Dequeue();
                }
                else if (req.done)
                {
                    var buffer = req.GetData<Color32>();
                    if (m_EncodeTempTex != null &&
                        m_EncodeTempTex.width != m_CameraParameters.cameraResolutionWidth &&
                        m_EncodeTempTex.height != m_CameraParameters.cameraResolutionHeight)
                    {
                        GameObject.Destroy(m_EncodeTempTex);
                        m_EncodeTempTex = null;
                    }
                    if (m_EncodeTempTex == null)
                    {
                        m_EncodeTempTex = new Texture2D(
                            m_CameraParameters.cameraResolutionWidth,
                            m_CameraParameters.cameraResolutionHeight,
                            TextureFormat.RGB24,
                            false
                        );
                    }
                    m_EncodeTempTex.SetPixels32(buffer.ToArray());
                    m_EncodeTempTex.Apply();

                    if (task.OnReceive != null)
                    {
                        if (m_EncodeTempTex.width != task.Width || m_EncodeTempTex.height != task.Height)
                        {
                            Texture2D scaledtexture;
                            NRDebugger.LogFormat("[BlendCamera] need to scale the texture which origin width:{0} and out put width:{1}",
                                m_EncodeTempTex.width, task.Width);
                            scaledtexture = ImageEncoder.ScaleTexture(m_EncodeTempTex, task.Width, task.Height);
                            CommitResult(scaledtexture, task);
                            //Destroy the scale temp texture.
                            GameObject.Destroy(scaledtexture);
                        }
                        else
                        {
                            CommitResult(m_EncodeTempTex, task);
                        }
                    }
                    m_Requests.Dequeue();
                    m_Tasks.Dequeue();
                }
                else
                {
                    break;
                }
            }
        }

        private void CommitResult(Texture2D texture, CaptureTask task)
        {
            if (task.OnReceive == null)
            {
                return;
            }

            if (texture == null)
            {
                task.OnReceive(task, null);
                return;
            }

            byte[] result = null;
            switch (task.CaptureFormat)
            {
                case PhotoCaptureFileOutputFormat.JPG:
                    result = texture.EncodeToJPG();
                    break;
                case PhotoCaptureFileOutputFormat.PNG:
                    result = texture.EncodeToPNG();
                    break;
                default:
                    break;
            }
            task.OnReceive(task, result);
        }

        public byte[] Encode(int width, int height, PhotoCaptureFileOutputFormat format)
        {
            if (m_CurrentFrame == null)
            {
                Debug.LogWarning("Current frame is empty!");
                return null;
            }
            byte[] data = null;
            RenderTexture pre = RenderTexture.active;
            RenderTexture targetRT = m_CurrentFrame;

            RenderTexture.active = targetRT;
            Texture2D texture2D = new Texture2D(targetRT.width, targetRT.height, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(new Rect(0, 0, targetRT.width, targetRT.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = pre;

            Texture2D outPutTex = texture2D;
            Texture2D scaleTexture = null;

            // Scale the texture while the output width or height not equal to the targetRT.
            if (width != targetRT.width || height != targetRT.height)
            {
                scaleTexture = ImageEncoder.ScaleTexture(texture2D, width, height);
                outPutTex = scaleTexture;
            }

            switch (format)
            {
                case PhotoCaptureFileOutputFormat.JPG:
                    data = outPutTex.EncodeToJPG();
                    break;
                case PhotoCaptureFileOutputFormat.PNG:
                    data = outPutTex.EncodeToPNG();
                    break;
                default:
                    break;
            }

            // Clear the temp texture.
            GameObject.Destroy(texture2D);
            if (scaleTexture != null)
            {
                GameObject.Destroy(scaleTexture);
            }

            return data;
        }

        public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Debug.Log("[ImageEncoder] ScaleTexture..");
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);

            for (int i = 0; i < result.height; ++i)
            {
                for (int j = 0; j < result.width; ++j)
                {
                    Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                    result.SetPixel(j, i, newColor);
                }
            }

            result.Apply();
            return result;
        }
    }
}
