/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Create a rgb camera texture.
    /// </summary>
    public class NRRGBCamTexture
    {
        public Action<RGBTextureFrame> OnUpdate;

        public int Height
        {
            get
            {
                return NRRgbCamera.Resolution.height;
            }
        }

        public int Width
        {
            get
            {
                return NRRgbCamera.Resolution.width;
            }
        }

        private bool m_IsPlaying = false;
        public bool IsPlaying
        {
            get
            {
                return m_IsPlaying;
            }
        }

        public bool DidUpdateThisFrame
        {
            get
            {
                return NRRgbCamera.HasFrame();
            }
        }

        public int FrameCount = 0;

        private Texture2D m_texture;

        public RGBTextureFrame CurrentFrame;

        private bool m_IsInitilized = false;
        private void Initilize()
        {
            if (m_IsInitilized)
            {
                return;
            }
            if (m_texture == null)
            {
                m_texture = CreateTex();
            }
            NRRgbCamera.Regist(this);
            m_IsInitilized = true;
        }

        private Texture2D CreateTex()
        {
            return new Texture2D(NRRgbCamera.Resolution.width, NRRgbCamera.Resolution.height, TextureFormat.RGB24, false);
        }

        public void Play()
        {
            if (m_IsPlaying)
            {
                return;
            }
            this.Initilize();
            NRKernalUpdater.Instance.OnUpdate += UpdateTexture;
            NRRgbCamera.Play();
            m_IsPlaying = true;
        }

        public void Pause()
        {
            if (!m_IsPlaying)
            {
                return;
            }
            NRKernalUpdater.Instance.OnUpdate -= UpdateTexture;
            m_IsPlaying = false;
        }

        public Texture2D GetTexture()
        {
            if (m_texture == null)
            {
                m_texture = CreateTex();
            }
            return m_texture;
        }

        private void UpdateTexture()
        {
            if (!NRRgbCamera.HasFrame())
            {
                return;
            }
            RGBRawDataFrame rgbRawDataFrame = NRRgbCamera.GetRGBFrame();

            m_texture.LoadRawTextureData(rgbRawDataFrame.data);
            m_texture.Apply();

            CurrentFrame.timeStamp = rgbRawDataFrame.timeStamp;
            CurrentFrame.texture = m_texture;
            FrameCount++;

            OnUpdate?.Invoke(CurrentFrame);
        }

        public void Stop()
        {
            if (!m_IsInitilized)
            {
                return;
            }
            NRRgbCamera.UnRegist(this);
            this.Pause();
            NRRgbCamera.Stop();
            GameObject.Destroy(m_texture);
            m_texture = null;
            m_IsInitilized = false;
        }
    }
}
