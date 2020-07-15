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
    using NRKernal;
    using System;
    using System.Runtime.InteropServices;

    public class NativeEncoder
    {
        public const String NRNativeEncodeLibrary = "media_enc";
        public UInt64 EncodeHandle;

        public bool Create()
        {
            var result = NativeApi.HWEncoderCreate(ref EncodeHandle);
            return result == 0;
        }

        public bool Start()
        {
            var result = NativeApi.HWEncoderStart(EncodeHandle);
            NativeErrorListener.Check(result, this, "Start");
            return result == 0;
        }

        public void SetConfigration(NativeEncodeConfig config)
        {
            var result = NativeApi.HWEncoderSetConfigration(EncodeHandle, LitJson.JsonMapper.ToJson(config));
            NativeErrorListener.Check(result, this, "SetConfigration");
        }

        public void UpdateSurface(IntPtr texture_id, UInt64 time_stamp)
        {
            var result = NativeApi.HWEncoderUpdateSurface(EncodeHandle, texture_id, time_stamp);
            NativeErrorListener.Check(result, this, "UpdateSurface");
        }

        public bool Stop()
        {
            var result = NativeApi.HWEncoderStop(EncodeHandle);
            NativeErrorListener.Check(result, this, "Stop");
            return result == 0;
        }

        public void Destroy()
        {
            var result = NativeApi.HWEncoderDestroy(EncodeHandle);
            NativeErrorListener.Check(result, this, "Destroy");
        }

        private struct NativeApi
        {
            [DllImport(NRNativeEncodeLibrary)]
            public static extern NativeResult HWEncoderCreate(ref UInt64 out_encoder_handle);

            [DllImport(NRNativeEncodeLibrary)]
            public static extern NativeResult HWEncoderStart(UInt64 encoder_handle);

            [DllImport(NRNativeEncodeLibrary)]
            public static extern NativeResult HWEncoderSetConfigration(UInt64 encoder_handle, string config);

            [DllImport(NRNativeEncodeLibrary)]
            public static extern NativeResult HWEncoderUpdateSurface(UInt64 encoder_handle, IntPtr texture_id, UInt64 time_stamp);

            [DllImport(NRNativeEncodeLibrary)]
            public static extern NativeResult HWEncoderStop(UInt64 encoder_handle);

            [DllImport(NRNativeEncodeLibrary)]
            public static extern NativeResult HWEncoderDestroy(UInt64 encoder_handle);
        }
    }
}