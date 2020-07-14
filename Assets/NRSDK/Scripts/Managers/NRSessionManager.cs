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
    using System.IO;
    using UnityEngine;
    using System.Collections;
    using NRKernal.Record;

    /// <summary>
    ///  Manages AR system state and handles the session lifecycle.
    ///  this class, application can create a session, configure it, start/pause or stop it.
    /// </summary>
    public class NRSessionManager
    {
        private static NRSessionManager m_Instance;

        public static NRSessionManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new NRSessionManager();
                }
                return m_Instance;
            }
        }

        private LostTrackingReason m_LostTrackingReason = LostTrackingReason.INITIALIZING;
        /// <summary>
        /// Current lost tracking reason.
        /// </summary>
        public LostTrackingReason LostTrackingReason
        {
            get
            {
                return m_LostTrackingReason;
            }
        }

        private SessionState m_SessionState = SessionState.UnInitialized;

        public SessionState SessionState
        {
            get
            {
                return m_SessionState;
            }
            private set
            {
                m_SessionState = value;
            }
        }

        public NRSessionBehaviour NRSessionBehaviour { get; private set; }

        public NRHMDPoseTracker NRHMDPoseTracker { get; private set; }

        internal NativeInterface NativeAPI { get; private set; }

        private NRRenderer NRRenderer { get; set; }

        public NRVirtualDisplayer VirtualDisplayer { get; set; }

        public bool IsInitialized
        {
            get
            {
                return (SessionState != SessionState.UnInitialized
                    && SessionState != SessionState.Destroyed);
            }
        }

        public void CreateSession(NRSessionBehaviour session)
        {
            if (SessionState != SessionState.UnInitialized && SessionState != SessionState.Destroyed)
            {
                return;
            }
            if (NRSessionBehaviour != null)
            {
                NRDebugger.LogError("Multiple SessionBehaviour components cannot exist in the scene. " +
                  "Destroying the newest.");
                GameObject.DestroyImmediate(session.gameObject);
                return;
            }
            NRSessionBehaviour = session;
            NRHMDPoseTracker = session.GetComponent<NRHMDPoseTracker>();

            try
            {
                NRDevice.Instance.Init();
                NRDevice.Instance.CreateGlassesController();
                NRDevice.Instance.CreateHMD();
            }
            catch (Exception)
            {
                throw;
            }

            NativeAPI = new NativeInterface();
            NativeAPI.NativeTracking.Create();
            bool is3dof = (NRHMDPoseTracker.TrackingMode == NRHMDPoseTracker.TrackingType.Tracking3Dof);
            TrackingMode mode = is3dof ? TrackingMode.MODE_3DOF : TrackingMode.MODE_6DOF;
            if (mode == TrackingMode.MODE_3DOF)
            {
                NRSessionBehaviour.SessionConfig.PlaneFindingMode = TrackablePlaneFindingMode.DISABLE;
                NRSessionBehaviour.SessionConfig.ImageTrackingMode = TrackableImageFindingMode.DISABLE;
            }
            NativeAPI.NativeTracking.SetTrackingMode(mode);

            NRKernalUpdater.Instance.OnUpdate -= Update;
            NRKernalUpdater.Instance.OnUpdate += Update;

            SessionState = SessionState.Initialized;
        }

        private bool m_IsSessionError = false;
        public void OprateInitException(Exception e)
        {
            if (m_IsSessionError)
            {
                return;
            }
            m_IsSessionError = true;
            if (e is NRGlassesConnectError)
            {
                ShowErrorTips(NativeConstants.GlassesDisconnectErrorTip);
            }
            else if (e is NRSdkVersionMismatchError)
            {
                ShowErrorTips(NativeConstants.SdkVersionMismatchErrorTip);
            }
            else if (e is NRSdcardPermissionDenyError)
            {
                ShowErrorTips(NativeConstants.SdcardPermissionDenyErrorTip);
            }
            else
            {
                ShowErrorTips(NativeConstants.UnknowErrorTip);
            }
        }

        private void ShowErrorTips(string msg)
        {
            var sessionbehaviour = GameObject.FindObjectOfType<NRSessionBehaviour>();
            if (sessionbehaviour != null)
            {
                GameObject.Destroy(sessionbehaviour.gameObject);
            }
            var input = GameObject.FindObjectOfType<NRInput>();
            if (input != null)
            {
                GameObject.Destroy(input.gameObject);
            }
            var virtualdisplay = GameObject.FindObjectOfType<NRVirtualDisplayer>();
            if (virtualdisplay != null)
            {
                GameObject.Destroy(virtualdisplay.gameObject);
            }
            GameObject.Instantiate<NRGlassesInitErrorTip>(Resources.Load<NRGlassesInitErrorTip>("NRErrorTips")).Init(msg, () =>
            {
                NRDevice.QuitApp();
            });
        }

        private void Update()
        {
            if (SessionState == SessionState.Running)
            {
                m_LostTrackingReason = NativeAPI.NativeHeadTracking.GetTrackingLostReason();
            }

            NRFrame.OnUpdate();
        }

        public void SetConfiguration(NRSessionConfig config)
        {
            if (config == null)
            {
                return;
            }
            NativeAPI.Configration.UpdateConfig(config);
        }

        public void Recenter()
        {
            if (SessionState != SessionState.Running)
            {
                return;
            }
            NativeAPI.NativeTracking.Recenter();
        }

        public static void SetAppSettings(bool useOptimizedRendering)
        {
            Application.targetFrameRate = 240;
            QualitySettings.maxQueuedFrames = -1;
            QualitySettings.vSyncCount = useOptimizedRendering ? 0 : 1;
            Screen.fullScreen = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public void StartSession()
        {
            if (SessionState == SessionState.Running
                || SessionState == SessionState.UnInitialized
                || SessionState == SessionState.Destroyed)
            {
                return;
            }
            var config = NRSessionBehaviour.SessionConfig;
            if (config != null)
            {
                SetAppSettings(config.OptimizedRendering);
#if !UNITY_EDITOR
                if (config.OptimizedRendering)
                {
                    if (NRSessionBehaviour.gameObject.GetComponent<NRRenderer>() == null)
                    {
                        NRRenderer = NRSessionBehaviour.gameObject.AddComponent<NRRenderer>();
                        NRRenderer.Initialize(NRHMDPoseTracker.leftCamera, NRHMDPoseTracker.rightCamera, NRHMDPoseTracker.GetHeadPose);
                    }
                }
#endif
            }
            else
            {
                SetAppSettings(false);
            }
            NativeAPI.NativeTracking.Start();
            NativeAPI.NativeHeadTracking.Create();
#if UNITY_EDITOR
            InitEmulator();
#endif
            SessionState = SessionState.Running;
        }

        public void DisableSession()
        {
            if (SessionState != SessionState.Running)
            {
                return;
            }
            SessionState = SessionState.Paused;

            if (NRVirtualDisplayer.RunInBackground)
            {
                NRRenderer?.Pause();
                NativeAPI.NativeTracking?.Pause();
                VirtualDisplayer?.Pause();
                NRDevice.Instance.PauseGlassesController();
                NRDevice.Instance.PauseHMD();
            }
            else
            {
                NRDevice.ForceKill();
            }
        }

        public void ResumeSession()
        {
            if (SessionState != SessionState.Paused)
            {
                return;
            }

            SessionState = SessionState.Running;
            NRDevice.Instance.ResumeHMD();
            NRDevice.Instance.ResumeGlassesController();
            VirtualDisplayer?.Resume();
            NativeAPI.NativeTracking.Resume();
            NRRenderer?.Resume();
        }

        public void DestroySession()
        {
            if (SessionState == SessionState.Destroyed || SessionState == SessionState.UnInitialized)
            {
                return;
            }
            SessionState = SessionState.Destroyed;
            NRRenderer?.Destroy();
            NativeAPI.NativeHeadTracking.Destroy();
            NativeAPI.NativeTracking.Destroy();
            VirtualDisplayer?.Destory();
            NRDevice.Instance.Destroy();
            FrameCaptureContextFactory.DisposeAllContext();
        }

        private void InitEmulator()
        {
            if (!NREmulatorManager.Inited && !GameObject.Find("NREmulatorManager"))
            {
                NREmulatorManager.Inited = true;
                GameObject.Instantiate(Resources.Load("Prefabs/NREmulatorManager"));
            }
            if (!GameObject.Find("NREmulatorHeadPos"))
            {
                GameObject.Instantiate(Resources.Load("Prefabs/NREmulatorHeadPose"));
            }
        }
    }
}
