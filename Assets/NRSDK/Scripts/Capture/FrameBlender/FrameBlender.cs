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
    using UnityEngine;

    public class FrameBlender
    {
        protected Camera m_TargetCamera;
        protected IEncoder m_Encoder;
        private Material m_BlendMaterial;
        protected BlendMode m_BlendMode;
        protected Texture2D m_RGBOrigin;
        protected RenderTexture m_RGBSource;
        protected Texture2D m_TempCombineTex;
        private int m_FrameCount;

        public BlendMode BlendMode
        {
            get
            {
                return m_BlendMode;
            }
        }

        private RenderTexture m_BlendTexture;
        public RenderTexture BlendTexture
        {
            get
            {
                return m_BlendTexture;
            }
            protected set
            {
                m_BlendTexture = value;
            }
        }

        public RenderTexture RGBTexture
        {
            get
            {
                return m_RGBSource;
            }
        }

        public RenderTexture VirtualTexture
        {
            get
            {
                return m_TargetCamera.targetTexture;
            }
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public int FrameCount
        {
            get
            {
                return m_FrameCount;
            }
            private set
            {
                m_FrameCount = value;
            }
        }

        public virtual void Init(Camera camera, IEncoder encoder, CameraParameters param)
        {
            Width = param.cameraResolutionWidth;
            Height = param.cameraResolutionHeight;
            m_BlendMode = param.blendMode;
            m_TargetCamera = camera;
            m_Encoder = encoder;

            switch (m_BlendMode)
            {
                case BlendMode.RGBOnly:
                    BlendTexture = new RenderTexture(Width, Height, 24, RenderTextureFormat.ARGB32);
                    break;
                case BlendMode.VirtualOnly:
                    BlendTexture = new RenderTexture(Width, Height, 24, RenderTextureFormat.ARGB32);
                    break;
                case BlendMode.Blend:
                    Shader blendshader;
                    blendshader = Resources.Load<Shader>("Record/Shaders/AlphaBlend");
                    m_BlendMaterial = new Material(blendshader);
                    BlendTexture = new RenderTexture(Width, Height, 24, RenderTextureFormat.ARGB32);
                    break;
                case BlendMode.WidescreenBlend:
                    BlendTexture = new RenderTexture(2 * Width, Height, 24, RenderTextureFormat.ARGB32);
                    m_RGBSource = new RenderTexture(Width, Height, 24, RenderTextureFormat.ARGB32);
                    m_TempCombineTex = new Texture2D(2 * Width, Height, TextureFormat.ARGB32, false);
                    break;
                default:
                    break;
            }
            m_TargetCamera.enabled = false;
            m_TargetCamera.targetTexture = new RenderTexture(Width, Height, 24, RenderTextureFormat.ARGB32);
        }

        public void OnFrame(RGBTextureFrame frame)
        {
            Texture2D frametex = frame.texture as Texture2D;
            m_RGBOrigin = frametex;

            m_TargetCamera.Render();

            switch (m_BlendMode)
            {
                case BlendMode.RGBOnly:
                    Graphics.Blit(frame.texture, BlendTexture);
                    break;
                case BlendMode.VirtualOnly:
                    Graphics.Blit(m_TargetCamera.targetTexture, BlendTexture);
                    break;
                case BlendMode.Blend:
                    m_BlendMaterial.SetTexture("_MainTex", m_TargetCamera.targetTexture);
                    m_BlendMaterial.SetTexture("_BcakGroundTex", frame.texture);
                    Graphics.Blit(m_TargetCamera.targetTexture, BlendTexture, m_BlendMaterial);
                    break;
                case BlendMode.WidescreenBlend:
                    CombineTexture(frametex, m_TargetCamera.targetTexture, m_TempCombineTex, BlendTexture);
                    break;
                default:
                    break;
            }

            // Commit frame                
            m_Encoder.Commit(BlendTexture, frame.timeStamp);
            FrameCount++;
        }

        private void CombineTexture(Texture2D bgsource, RenderTexture foresource, Texture2D tempdest, RenderTexture dest)
        {
            Graphics.Blit(bgsource, m_RGBSource);

            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = m_RGBSource;
            tempdest.ReadPixels(new Rect(0, 0, m_RGBSource.width, m_RGBSource.height), 0, 0);

            RenderTexture.active = foresource;
            tempdest.ReadPixels(new Rect(0, 0, foresource.width, foresource.height), foresource.width, 0);
            tempdest.Apply();
            RenderTexture.active = prev;

            Graphics.Blit(tempdest, dest);
        }

        public virtual void Dispose()
        {
            RenderTexture.active = null;
            BlendTexture?.Release();
            m_RGBSource?.Release();

            GameObject.Destroy(m_TempCombineTex);
            BlendTexture = null;
            m_RGBSource = null;
            m_TempCombineTex = null;
        }
    }
}
