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

    public class CaptureBehaviourBase : MonoBehaviour
    {
        [SerializeField]
        private Transform RGBCameraRig;
        public Camera CaptureCamera;
        private FrameCaptureContext m_FrameCaptureContext;
        private FrameBlender m_Blender;

        public FrameCaptureContext GetContext()
        {
            return m_FrameCaptureContext;
        }

        public virtual void Init(FrameCaptureContext context, FrameBlender blendCamera)
        {
            this.m_FrameCaptureContext = context;
            this.m_Blender = blendCamera;
        }

        private ulong m_PredictTime;
        public void UpdatePredictTime(float value)
        {
            m_PredictTime = (ulong)(value * 1000000);
        }

        public virtual void OnFrame(RGBTextureFrame frame)
        {
            // update camera pose
            UpdateHeadPoseByTimestamp(frame.timeStamp);

            // commit a frame
            m_Blender.OnFrame(frame);
        }

        private void UpdateHeadPoseByTimestamp(UInt64 timestamp)
        {
            Pose head_pose = Pose.identity;
            var result = NRFrame.GetHeadPoseByTime(ref head_pose, timestamp, m_PredictTime);
            if (result)
            {
                RGBCameraRig.transform.localPosition = head_pose.position;
                RGBCameraRig.transform.localRotation = head_pose.rotation;
            }
        }
    }
}
