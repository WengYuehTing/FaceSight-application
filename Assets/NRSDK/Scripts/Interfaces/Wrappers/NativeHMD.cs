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
    using System.Runtime.InteropServices;

    /// <summary>
    /// HMD Eye offset Native API .
    /// </summary>
    public partial class NativeHMD
    {
        private UInt64 m_HmdHandle;

        public bool Create()
        {
            NativeResult result = NativeApi.NRHMDCreate(ref m_HmdHandle);
            NativeErrorListener.Check(result, this, "Create");
            return result == NativeResult.Success;
        }

        public bool Pause()
        {
            NativeResult result = NativeApi.NRHMDPause(m_HmdHandle);
            NativeErrorListener.Check(result, this, "Pause");
            return result == NativeResult.Success;
        }

        public bool Resume()
        {
            NativeResult result = NativeApi.NRHMDResume(m_HmdHandle);
            NativeErrorListener.Check(result, this, "Resume");
            return result == NativeResult.Success;
        }

        public Pose GetEyePoseFromHead(NativeEye eye)
        {
            Pose outEyePoseFromHead = Pose.identity;
            NativeMat4f mat4f = new NativeMat4f(Matrix4x4.identity);
            NativeResult result = NativeApi.NRHMDGetEyePoseFromHead(m_HmdHandle, (int)eye, ref mat4f);
            if (result == NativeResult.Success)
            {
                ConversionUtility.ApiPoseToUnityPose(mat4f, out outEyePoseFromHead);
            }
            return outEyePoseFromHead;
        }

        public bool GetProjectionMatrix(ref EyeProjectMatrixData outEyesProjectionMatrix, float znear, float zfar)
        {
            NativeFov4f fov = new NativeFov4f();
            NativeResult result_left = NativeApi.NRHMDGetEyeFov(m_HmdHandle, (int)NativeEye.LEFT, ref fov);
            outEyesProjectionMatrix.LEyeMatrix = ConversionUtility.GetProjectionMatrixFromFov(fov, znear, zfar).ToUnityMat4f();
            NativeResult result_right = NativeApi.NRHMDGetEyeFov(m_HmdHandle, (int)NativeEye.RIGHT, ref fov);
            outEyesProjectionMatrix.REyeMatrix = ConversionUtility.GetProjectionMatrixFromFov(fov, znear, zfar).ToUnityMat4f();
            NativeResult result_RGB = NativeApi.NRHMDGetEyeFov(m_HmdHandle, (int)NativeEye.RGB, ref fov);
            outEyesProjectionMatrix.RGBEyeMatrix = ConversionUtility.GetProjectionMatrixFromFov(fov, znear, zfar).ToUnityMat4f();
            return (result_left == NativeResult.Success && result_right == NativeResult.Success && result_RGB == NativeResult.Success);
        }

        public NativeResolution GetEyeResolution(NativeEye eye)
        {
            NativeResolution resolution = new NativeResolution(3840, 1080);
            var result = NativeApi.NRHMDGetEyeResolution(m_HmdHandle, (int)eye, ref resolution);
            NativeErrorListener.Check(result, this, "GetEyeResolution");
            return resolution;
        }

        public bool Destroy()
        {
            NativeResult result = NativeApi.NRHMDDestroy(m_HmdHandle);
            NativeErrorListener.Check(result, this, "Destroy");
            return result == NativeResult.Success;
        }

        private struct NativeApi
        {
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRHMDCreate(ref UInt64 out_hmd_handle);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRHMDPause(UInt64 hmd_handle);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRHMDResume(UInt64 hmd_handle);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRHMDGetEyePoseFromHead(UInt64 hmd_handle, int eye, ref NativeMat4f outEyePoseFromHead);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRHMDGetEyeFov(UInt64 hmd_handle, int eye, ref NativeFov4f out_eye_fov);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRHMDGetEyeResolution(UInt64 hmd_handle, int eye, ref NativeResolution out_eye_resolution);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRHMDDestroy(UInt64 hmd_handle);
        };
    }
}
