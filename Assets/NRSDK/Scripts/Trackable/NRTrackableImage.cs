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
    /// A trackable image in the real world detected by NRInternel.
    /// </summary>
    public class NRTrackableImage : NRTrackable
    {
        internal NRTrackableImage(UInt64 nativeHandle, NativeInterface nativeInterface)
          : base(nativeHandle, nativeInterface)
        {
        }

        /// <summary>
        /// Gets the position and orientation of the marker's center in Unity world space.
        /// </summary>
        /// <returns></returns>
        public override Pose GetCenterPose()
        {
            if (NRFrame.SessionStatus != SessionState.Running)
            {
                return Pose.identity;
            }
            return NativeInterface.NativeTrackableImage.GetCenterPose(TrackableNativeHandle);
        }

        /// <summary>
        /// Gets the width of marker.
        /// </summary>
        public float ExtentX
        {
            get
            {
                return Size.x;
            }
        }

        /// <summary>
        /// Gets the height of marker.
        /// </summary>
        public float ExtentZ
        {
            get
            {
                return Size.y;
            }
        }

        /// <summary>
        /// Get the size of trackable image.
        /// size of trackable imag(width、height).
        /// </summary>
        public Vector2 Size
        {
            get
            {
                if (NRFrame.SessionStatus != SessionState.Running)
                {
                    return Vector2.zero;
                }
                return NativeInterface.NativeTrackableImage.GetSize(TrackableNativeHandle);
            }
        }
    }
}
