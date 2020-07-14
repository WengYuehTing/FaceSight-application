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
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Contains information captured from the web camera.
    /// </summary>
    public sealed class PhotoCaptureFrame : IDisposable
    {
        private byte[] data;

        public PhotoCaptureFrame(CapturePixelFormat format, byte[] data)
        {
            this.data = data;
            this.pixelFormat = format;
        }

        ~PhotoCaptureFrame()
        {

        }

        /// <summary>
        /// The length of the raw IMFMediaBuffer which contains the image captured.
        /// </summary>
        public int dataLength { get; }

        /// <summary>
        /// Specifies whether or not spatial data was captured.
        /// </summary>
        public bool hasLocationData { get; }

        /// <summary>
        /// The raw image data pixel format.
        /// </summary>
        public CapturePixelFormat pixelFormat { get; }

        public void CopyRawImageDataIntoBuffer(List<byte> byteBuffer)
        {

        }

        /// <summary>
        /// Disposes the PhotoCaptureFrame and any resources it uses.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Provides a COM pointer to the native IMFMediaBuffer that contains the image data.
        /// </summary>
        /// <returns>A native COM pointer to the IMFMediaBuffer which contains the image data.</returns>
        public IntPtr GetUnsafePointerToBuffer()
        {
            return IntPtr.Zero;
        }

        public bool TryGetCameraToWorldMatrix(out Matrix4x4 cameraToWorldMatrix)
        {
            cameraToWorldMatrix = Matrix4x4.identity;
            return true;
        }

        public bool TryGetProjectionMatrix(out Matrix4x4 projectionMatrix)
        {
            projectionMatrix = Matrix4x4.identity;
            return true;
        }

        public bool TryGetProjectionMatrix(float nearClipPlane, float farClipPlane, out Matrix4x4 projectionMatrix)
        {
            projectionMatrix = Matrix4x4.identity;
            return true;
        }

        /// <summary>
        /// This method will copy the captured image data into a user supplied texture for use in Unity.
        /// </summary>
        /// <param name="targetTexture">The target texture that the captured image data will be copied to.</param>
        public void UploadImageDataToTexture(Texture2D targetTexture)
        {
            if (data == null)
            {
                return;
            }
            ImageConversion.LoadImage(targetTexture, data);
        }
    }
}
