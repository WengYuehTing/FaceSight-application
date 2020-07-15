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
    using AOT;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    /// <summary>
    /// NRNativeRender oprate rendering-related things, provides the feature of optimized rendering and low latency.
    /// </summary>
    public class NRRenderer : MonoBehaviour
    {
        /// @cond EXCLUDE_FROM_DOXYGEN
        public delegate void PoseProvideDelegage(ref Pose pose);
        public PoseProvideDelegage OnUpdatePose = null;

        private delegate void RenderEventDelegate(int eventID);
        private static RenderEventDelegate RenderThreadHandle = new RenderEventDelegate(RunOnRenderThread);
        private static IntPtr RenderThreadHandlePtr = Marshal.GetFunctionPointerForDelegate(RenderThreadHandle);

        private const int SETRENDERTEXTUREEVENT = 0x0001;
        private const int STARTNATIVERENDEREVENT = 0x0002;
        private const int RESUMENATIVERENDEREVENT = 0x0003;
        private const int PAUSENATIVERENDEREVENT = 0x0004;
        private const int STOPNATIVERENDEREVENT = 0x0005;

        public enum Eyes
        {
            Left = 0,
            Right = 1,
            Count = 2
        }

        public Camera leftCamera;
        public Camera rightCamera;
        static NativeRenderring m_NativeRenderring { get; set; }
        /// @endcond

        private const int EyeTextureCount = 3 * (int)Eyes.Count;
        private readonly RenderTexture[] eyeTextures = new RenderTexture[EyeTextureCount];
        private Dictionary<RenderTexture, IntPtr> m_RTDict = new Dictionary<RenderTexture, IntPtr>();

        public enum RendererState
        {
            UnInitialized,
            Initialized,
            Running,
            Paused,
            Destroyed
        }
        private RendererState m_CurrentState = RendererState.UnInitialized;

#if !UNITY_EDITOR
        private int currentEyeTextureIdx = 0;
        private int nextEyeTextureIdx = 0;
#endif

        public static bool isLinearColorSpace
        {
            get
            {
                return QualitySettings.activeColorSpace == ColorSpace.Linear;
            }
        }

        /// <summary>
        /// Initialize the render pipleline.
        /// </summary>
        /// <param name="leftcamera">Left Eye.</param>
        /// <param name="rightcamera">Right Eye.</param>
        /// <param name="poseprovider">provide the pose of camera every frame.</param>
        public void Initialize(Camera leftcamera, Camera rightcamera, PoseProvideDelegage poseprovider)
        {
            if (m_CurrentState != RendererState.UnInitialized)
            {
                return;
            }

            NRSessionManager.SetAppSettings(true);

            leftCamera = leftcamera;
            rightCamera = rightcamera;
            OnUpdatePose = poseprovider;

#if !UNITY_EDITOR
            leftCamera.depthTextureMode = DepthTextureMode.Depth;
            rightCamera.depthTextureMode = DepthTextureMode.Depth;
            leftCamera.rect = new Rect(0, 0, 1, 1);
            rightCamera.rect = new Rect(0, 0, 1, 1);
            leftCamera.enabled = false;
            rightCamera.enabled = false;
            CreateRenderTextures();
            m_CurrentState = RendererState.Initialized;
            Invoke("StartUp", 0.3f);
#endif
        }

        private void StartUp()
        {
            leftCamera.enabled = true;
            rightCamera.enabled = true;
#if !UNITY_EDITOR
            m_NativeRenderring = new NativeRenderring();
            m_NativeRenderring.Create();
#if !UNITY_STANDALONE_WIN
            m_NativeRenderring.InitColorSpace();
#endif

            StartCoroutine(RenderCoroutine());
#endif
            m_CurrentState = RendererState.Running;
            GL.IssuePluginEvent(RenderThreadHandlePtr, STARTNATIVERENDEREVENT);
        }

        /// <summary>
        /// Pause render.
        /// </summary>
        public void Pause()
        {
            if (m_CurrentState != RendererState.Running)
            {
                return;
            }
            m_CurrentState = RendererState.Paused;
            GL.IssuePluginEvent(RenderThreadHandlePtr, PAUSENATIVERENDEREVENT);
        }

        /// <summary>
        /// Resume render.
        /// </summary>
        public void Resume()
        {
            Invoke("DelayResume", 0.3f);
        }

        private void DelayResume()
        {
            if (m_CurrentState != RendererState.Paused)
            {
                return;
            }
            m_CurrentState = RendererState.Running;
            GL.IssuePluginEvent(RenderThreadHandlePtr, RESUMENATIVERENDEREVENT);
        }

#if !UNITY_EDITOR
        void Update()
        {
            if (m_CurrentState == RendererState.Running)
            {
                leftCamera.targetTexture = eyeTextures[currentEyeTextureIdx];
                rightCamera.targetTexture = eyeTextures[currentEyeTextureIdx + 1];
                currentEyeTextureIdx = nextEyeTextureIdx;
                nextEyeTextureIdx = (nextEyeTextureIdx + 2) % EyeTextureCount;
            }
        }
#endif

        private RenderTexture GenRenderTexture(int width, int height)
        {
            RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.Default);
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            if (QualitySettings.antiAliasing > 0)
            {
                renderTexture.antiAliasing = QualitySettings.antiAliasing;
            }
            renderTexture.Create();

            return renderTexture;
        }

        private void CreateRenderTextures()
        {
            var resolution = NRDevice.Instance.NativeHMD.GetEyeResolution(NativeEye.LEFT);
            NRDebugger.Log("[CreateRenderTextures]  resolution :" + resolution.ToString());

            for (int i = 0; i < EyeTextureCount; i++)
            {
                eyeTextures[i] = GenRenderTexture(resolution.width, resolution.height);
                m_RTDict.Add(eyeTextures[i], eyeTextures[i].GetNativeTexturePtr());
            }
        }

        private IEnumerator RenderCoroutine()
        {
            WaitForEndOfFrame delay = new WaitForEndOfFrame();
            yield return delay;

            while (true)
            {
                yield return delay;

                if (m_CurrentState != RendererState.Running)
                {
                    continue;
                }

                NativeMat4f apiPose;
                Pose unityPose = Pose.identity;
                if (OnUpdatePose != null) OnUpdatePose(ref unityPose);
                ConversionUtility.UnityPoseToApiPose(unityPose, out apiPose);
                IntPtr left_target, right_target;
                if (!m_RTDict.TryGetValue(leftCamera.targetTexture, out left_target)) continue;
                if (!m_RTDict.TryGetValue(rightCamera.targetTexture, out right_target)) continue;
                FrameInfo info = new FrameInfo(left_target, right_target, apiPose);
                SetRenderFrameInfo(info);
            }
        }

        [MonoPInvokeCallback(typeof(RenderEventDelegate))]
        private static void RunOnRenderThread(int eventID)
        {
            if (eventID == STARTNATIVERENDEREVENT)
            {
                m_NativeRenderring?.Start();
            }
            else if (eventID == RESUMENATIVERENDEREVENT)
            {
                m_NativeRenderring?.Resume();
            }
            else if (eventID == PAUSENATIVERENDEREVENT)
            {
                m_NativeRenderring?.Pause();
            }
            else if (eventID == STOPNATIVERENDEREVENT)
            {
                m_NativeRenderring?.Destroy();
                m_NativeRenderring = null;
                NRDevice.Instance.Destroy();
            }
            else if (eventID == SETRENDERTEXTUREEVENT)
            {
                FrameInfo framinfo = (FrameInfo)Marshal.PtrToStructure(m_NativeRenderring.FrameInfoPtr, typeof(FrameInfo));
                m_NativeRenderring?.DoRender(framinfo.leftTex, framinfo.rightTex, ref framinfo.pose);
            }
        }

        private static void SetRenderFrameInfo(FrameInfo frame)
        {
            Marshal.StructureToPtr(frame, m_NativeRenderring.FrameInfoPtr, true);
            GL.IssuePluginEvent(RenderThreadHandlePtr, SETRENDERTEXTUREEVENT);
        }

        public void Destroy()
        {
            if (m_CurrentState == RendererState.Destroyed)
            {
                return;
            }
            m_CurrentState = RendererState.Destroyed;
            GL.IssuePluginEvent(RenderThreadHandlePtr, STOPNATIVERENDEREVENT);
        }

        private void OnDestroy()
        {
            this.Destroy();
        }
    }
}
