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
    using System;
    using AOT;
    using System.Runtime.InteropServices;

    public class VideoEncoder : IEncoder
    {
#if !UNITY_EDITOR
        public static NativeEncoder NativeEncoder { get; set; }
        private delegate void RenderEventDelegate(int eventID);
        private static RenderEventDelegate RenderThreadHandle = new RenderEventDelegate(RunOnRenderThread);
        private static IntPtr RenderThreadHandlePtr = Marshal.GetFunctionPointerForDelegate(RenderThreadHandle);
#endif
        private bool m_IsStarted = false;

        public NativeEncodeConfig EncodeConfig;

        private const int STARTENCODEEVENT = 0x0001;

        private IntPtr m_TexPtr = IntPtr.Zero;

        public VideoEncoder()
        {
#if !UNITY_EDITOR
            NativeEncoder = new NativeEncoder();
            NativeEncoder.Create();
#endif
        }

#if !UNITY_EDITOR
        [MonoPInvokeCallback(typeof(RenderEventDelegate))]
        private static void RunOnRenderThread(int eventID)
        {
            if (eventID == STARTENCODEEVENT)
            {
                NativeEncoder.Start();
            }
        }
#endif

        public void Config(CameraParameters param)
        {
            Config(new NativeEncodeConfig(param));
        }

        public void Config(NativeEncodeConfig config)
        {
            EncodeConfig = config;
            NRDebugger.Log("Encode record Config：" + config.ToString());
        }

        public void Start()
        {
            if (m_IsStarted)
            {
                return;
            }
#if !UNITY_EDITOR
            NativeEncoder.SetConfigration(EncodeConfig);
            GL.IssuePluginEvent(RenderThreadHandlePtr, STARTENCODEEVENT);
#endif
            NRDebugger.Log("Encode record Start");
            m_IsStarted = true;
        }

        public void Commit(RenderTexture rt, UInt64 timestamp)
        {
            if (!m_IsStarted)
            {
                return;
            }
            if (m_TexPtr == IntPtr.Zero)
            {
                m_TexPtr = rt.GetNativeTexturePtr();
            }
#if !UNITY_EDITOR
            NativeEncoder.UpdateSurface(m_TexPtr, timestamp);
#endif
        }

        public void Stop()
        {
            if (!m_IsStarted)
            {
                return;
            }
#if !UNITY_EDITOR
            NativeEncoder.Stop();
#endif
            NRDebugger.Log("Encode record Stop");
            m_IsStarted = false;
        }

        public void Release()
        {
            NRDebugger.Log("Encode record Release...");
#if !UNITY_EDITOR
            NativeEncoder.Destroy();
#endif
        }
    }
}
