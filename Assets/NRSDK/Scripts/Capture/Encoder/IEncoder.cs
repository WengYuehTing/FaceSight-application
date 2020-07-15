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

    public interface IEncoder
    {
        void Config(CameraParameters param);

        void Commit(RenderTexture rt, UInt64 timestamp);

        void Start();

        void Stop();

        void Release();
    }
}
