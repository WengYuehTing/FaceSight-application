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
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Holds information about NR Device's pose in the world coordinate, trackables, etc..
    /// Through this class, application can get the infomation of current frame.
    /// It contains session status, lost tracking reason, device pose, trackables, etc..
    /// </summary>
    public partial class NRFrame
    {
        /// <summary>
        /// Get the tracking state of HMD.
        /// </summary>
        public static SessionState SessionStatus
        {
            get
            {
                return NRSessionManager.Instance.SessionState;
            }
        }

        /// <summary>
        /// Get the lost tracking reason of HMD.
        /// </summary>
        public static LostTrackingReason LostTrackingReason
        {
            get
            {
                return NRSessionManager.Instance.LostTrackingReason;
            }
        }

        private static Pose m_HeadPose;

        /// <summary>
        /// Get the pose of device in unity world coordinate.
        /// </summary>
        /// <returns>Pose of device.</returns>
        public static Pose HeadPose
        {
            get
            {
                return m_HeadPose;
            }
        }

        public static bool GetHeadPoseByTime(ref Pose pose, UInt64 timestamp = 0, UInt64 predict = 0)
        {
            if (SessionStatus == SessionState.Running)
            {
                return NRSessionManager.Instance.NativeAPI.NativeHeadTracking.GetHeadPose(ref pose, timestamp, predict);
            }
            return false;
        }

        /// <summary>
        /// Get the pose of center camera between left eye and right eye.
        /// </summary>
        public static Pose CenterEyePose
        {
            get
            {
                Transform leftCamera = NRSessionManager.Instance.NRHMDPoseTracker.leftCamera.transform;
                Transform rightCamera = NRSessionManager.Instance.NRHMDPoseTracker.rightCamera.transform;

                Vector3 centerEye_pos = (leftCamera.position + rightCamera.position) * 0.5f;
                Quaternion centerEye_rot = Quaternion.Lerp(leftCamera.rotation, rightCamera.rotation, 0.5f);

                return new Pose(centerEye_pos, centerEye_rot);
            }
        }

        private static EyePoseData m_EyePosFromHead;

        /// <summary>
        /// Get the offset position between eye and head.
        /// </summary>
        public static EyePoseData EyePosFromHead
        {
            get
            {
                if (SessionStatus == SessionState.Running)
                {
                    m_EyePosFromHead.LEyePose = NRDevice.Instance.NativeHMD.GetEyePoseFromHead(NativeEye.LEFT);
                    m_EyePosFromHead.REyePose = NRDevice.Instance.NativeHMD.GetEyePoseFromHead(NativeEye.RIGHT);
                    m_EyePosFromHead.RGBEyePos = NRDevice.Instance.NativeHMD.GetEyePoseFromHead(NativeEye.RGB);
                }
                return m_EyePosFromHead;
            }
        }

        /// <summary>
        /// Get the project matrix of camera in unity.
        /// </summary>
        /// <returns>project matrix of camera.</returns>
        public static EyeProjectMatrixData GetEyeProjectMatrix(out bool result, float znear, float zfar)
        {
            result = false;
            EyeProjectMatrixData m_EyeProjectMatrix = new EyeProjectMatrixData();
            result = NRDevice.Instance.NativeHMD.GetProjectionMatrix(ref m_EyeProjectMatrix, znear, zfar);
            return m_EyeProjectMatrix;
        }

        public static void OnUpdate()
        {
            // Update head pos
            Pose pose = Pose.identity;
            if (GetHeadPoseByTime(ref pose))
            {
                m_HeadPose = pose;
            }
        }

        /// <summary>
        /// Get the list of trackables with specified filter.
        /// </summary>
        /// <param name="trackables">A list where the returned trackable stored.The previous values will be cleared.</param>
        /// <param name="filter">Query filter.</param>
        public static void GetTrackables<T>(List<T> trackables, NRTrackableQueryFilter filter) where T : NRTrackable
        {
            if (SessionStatus != SessionState.Running)
            {
                return;
            }
            trackables.Clear();
            NRSessionManager.Instance.NativeAPI.TrackableFactory.GetTrackables<T>(trackables, filter);
        }
    }
}
