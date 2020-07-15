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
    using System.IO;

    /// <summary>
    /// Capture a image from the MR world.
    /// You can capture a RGB only,Virtual only or Blended image through this class.
    /// </summary>
    public class NRCaptureBehaviour : CaptureBehaviourBase
    {
        private ImageEncoder ImageEncoder
        {
            get
            {
                return this.GetContext().GetEncoder() as ImageEncoder;
            }
        }

        public bool Do(int width, int height, PhotoCaptureFileOutputFormat format, string outpath)
        {
            var data = this.ImageEncoder.Encode(width, height, format);
            if (data == null)
            {
                return false;
            }
            File.WriteAllBytes(outpath, data);

            return true;
        }

        public bool Do(int width, int height, PhotoCaptureFileOutputFormat format, ref byte[] data)
        {
            data = this.ImageEncoder.Encode(width, height, format);
            if (data == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Capture a image Asyn.
        /// if system supports AsyncGPUReadback, using AsyncGPUReadback to get the captured image, else getting the image by synchronization way.
        /// </summary>
        private void DoAsyn(CaptureTask task)
        {
            if (SystemInfo.supportsAsyncGPUReadback)
            {
                this.ImageEncoder.Commit(task);
            }
            else
            {
                var data = ImageEncoder.Encode(task.Width, task.Height, task.CaptureFormat);
                if (task.OnReceive != null)
                {
                    task.OnReceive(task, data);
                }
            }
        }

        public void DoAsyn(NRPhotoCapture.OnCapturedToMemoryCallback oncapturedcallback)
        {
            var captureTask = new CaptureTask();
            var cameraParam = this.GetContext().RequestCameraParam();
            captureTask.Width = cameraParam.cameraResolutionWidth;
            captureTask.Height = cameraParam.cameraResolutionHeight;
            captureTask.CaptureFormat = cameraParam.pixelFormat == CapturePixelFormat.PNG ? PhotoCaptureFileOutputFormat.PNG : PhotoCaptureFileOutputFormat.JPG;
            captureTask.OnReceive += (task, data) =>
            {
                if (oncapturedcallback != null)
                {
                    var result = new NRPhotoCapture.PhotoCaptureResult();
                    result.resultType = NRPhotoCapture.CaptureResultType.Success;
                    CapturePixelFormat format = task.CaptureFormat == PhotoCaptureFileOutputFormat.PNG ? CapturePixelFormat.PNG : CapturePixelFormat.JPEG;
                    PhotoCaptureFrame frame = new PhotoCaptureFrame(format, data);
                    oncapturedcallback(result, frame);
                }
            };

            this.DoAsyn(captureTask);
        }

        public void Do(string filename, PhotoCaptureFileOutputFormat fileOutputFormat)
        {
            var cameraParam = this.GetContext().RequestCameraParam();
            this.Do(cameraParam.cameraResolutionWidth,
                    cameraParam.cameraResolutionHeight,
                    fileOutputFormat,
                    filename
            );
        }
    }
}