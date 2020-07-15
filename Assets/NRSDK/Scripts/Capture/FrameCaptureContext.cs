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
    using UnityEngine;

    public class FrameCaptureContext
    {
        private FrameBlender m_Blender;
        private IEncoder m_Encoder;
        private CameraParameters m_CameraParameters;
        private AbstractFrameProvider m_FrameProvider;
        private CaptureBehaviourBase m_CaptureBehaviour;
        private bool m_IsInit = false;

        public Texture PreviewTexture
        {
            get
            {
                return m_Blender?.BlendTexture;
            }
        }

        public CaptureBehaviourBase GetBehaviour()
        {
            return m_CaptureBehaviour;
        }

        public AbstractFrameProvider GetFrameProvider()
        {
            return m_FrameProvider;
        }

        public FrameBlender GetBlender()
        {
            return m_Blender;
        }

        public CameraParameters RequestCameraParam()
        {
            return m_CameraParameters;
        }

        public IEncoder GetEncoder()
        {
            return m_Encoder;
        }

        public FrameCaptureContext(AbstractFrameProvider provider)
        {
            this.m_FrameProvider = provider;
        }

        public void StartCaptureMode(CameraParameters param)
        {
            if (m_IsInit)
            {
                return;
            }
            this.m_CaptureBehaviour = this.GetCaptureBehaviourByMode(param.camMode);
            this.m_CameraParameters = param;
            this.m_Encoder = GetEncoderByMode(param.camMode);
            this.m_Encoder.Config(param);
            this.m_Blender = new FrameBlender();
            this.m_Blender.Init(m_CaptureBehaviour.CaptureCamera, m_Encoder, param);
            this.m_CaptureBehaviour.Init(this, m_Blender);
            this.m_FrameProvider.OnUpdate += this.m_CaptureBehaviour.OnFrame;
            this.m_IsInit = true;
        }

        private CaptureBehaviourBase GetCaptureBehaviourByMode(CamMode mode)
        {
            if (mode == CamMode.PhotoMode)
            {
                NRCaptureBehaviour capture = GameObject.FindObjectOfType<NRCaptureBehaviour>();
                var headParent = NRSessionManager.Instance.NRSessionBehaviour.transform.parent;
                if (capture == null)
                {
                    capture = GameObject.Instantiate(Resources.Load<NRCaptureBehaviour>("Record/Prefabs/NRCaptureBehaviour"), headParent);
                }
                GameObject.DontDestroyOnLoad(capture.gameObject);
                return capture;
            }
            else if (mode == CamMode.VideoMode)
            {
                NRRecordBehaviour capture = GameObject.FindObjectOfType<NRRecordBehaviour>();
                var headParent = NRSessionManager.Instance.NRSessionBehaviour.transform.parent;
                if (capture == null)
                {
                    capture = GameObject.Instantiate(Resources.Load<NRRecordBehaviour>("Record/Prefabs/NRRecorderBehaviour"), headParent);
                }
                GameObject.DontDestroyOnLoad(capture.gameObject);
                return capture;
            }
            else
            {
                throw new Exception("CamMode need to be set correctly for capture behaviour!");
            }
        }

        private IEncoder GetEncoderByMode(CamMode mode)
        {
            if (mode == CamMode.PhotoMode)
            {
                return new ImageEncoder();
            }
            else if (mode == CamMode.VideoMode)
            {
                return new VideoEncoder();
            }
            else
            {
                throw new Exception("CamMode need to be set correctly for encoder!");
            }
        }

        public void StopCaptureMode()
        {
            this.Release();
        }

        public void StartCapture()
        {
            if (!m_IsInit)
            {
                return;
            }
            m_Encoder?.Start();
            m_FrameProvider?.Play();
        }

        public void StopCapture()
        {
            if (!m_IsInit)
            {
                return;
            }
            m_FrameProvider?.Stop();
            m_Encoder?.Stop();
        }

        public void Release()
        {
            if (!m_IsInit)
            {
                return;
            }
            m_FrameProvider.OnUpdate -= this.m_CaptureBehaviour.OnFrame;
            m_FrameProvider?.Release();
            m_Blender?.Dispose();
            m_Encoder?.Release();
            GameObject.Destroy(m_CaptureBehaviour);
            m_IsInit = false;
        }
    }
}
