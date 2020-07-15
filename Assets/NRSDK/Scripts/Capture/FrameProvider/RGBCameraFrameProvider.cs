/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

using UnityEngine;

namespace NRKernal.Record
{
    using System;
    using NRKernal;

    public class RGBCameraFrameProvider : AbstractFrameProvider
    {
        private NRRGBCamTexture m_RGBTex;

        public RGBCameraFrameProvider()
        {
            m_RGBTex = new NRRGBCamTexture();
            m_RGBTex.OnUpdate += UpdateFrame;
        }

        private void UpdateFrame(RGBTextureFrame frame)
        {
            OnUpdate?.Invoke(frame);
            m_IsFrameReady = true;
        }

        public override Resolution GetFrameInfo()
        {
            Resolution resolution = new Resolution();
            resolution.width = m_RGBTex.Width;
            resolution.height = m_RGBTex.Height;
            return resolution;
        }

        public override void Play()
        {
            m_RGBTex.Play();
        }

        public override void Stop()
        {
            m_RGBTex.Pause();
        }

        public override void Release()
        {
            m_RGBTex.Stop();
        }
    }
}
