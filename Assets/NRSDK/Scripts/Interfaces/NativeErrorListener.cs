/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

using UnityEngine;

namespace NRKernal
{
    public class NativeErrorListener
    {
        internal static void Check(NativeResult result, object module, string funcName = "", bool needthrowerror = false)
        {
            if (result == NativeResult.Success)
            {
                return;
            }

            string module_tag = string.Format("[{0}] {1}: ", module.GetType().Name, funcName);
            if (needthrowerror)
            {
                try
                {
                    switch (result)
                    {
                        case NativeResult.Failure:
                            throw new NRKernalError(module_tag + "Failed!");
                        case NativeResult.InvalidArgument:
                            throw new NRInvalidArgumentError(module_tag + "InvalidArgument error!");
                        case NativeResult.NotEnoughMemory:
                            throw new NRNotEnoughMemoryError(module_tag + "NotEnoughMemory error!");
                        case NativeResult.UnSupported:
                            throw new NRUnSupportedError(module_tag + "UnSupported error!");
                        case NativeResult.GlassesDisconnect:
                            throw new NRGlassesConnectError(module_tag + "Glasses connect error!");
                        case NativeResult.SdkVersionMismatch:
                            throw new NRSdkVersionMismatchError(module_tag + "SDK version mismatch error!");
                        case NativeResult.SdcardPermissionDeny:
                            throw new NRSdcardPermissionDenyError(module_tag + "Sdcard permission deny error!");
                        default:
                            break;
                    }
                }
                catch (System.Exception e)
                {
                    NRSessionManager.Instance.OprateInitException(e);
                    throw;
                }
            }
            else
            {
                Debug.LogError(module_tag + result.ToString());
            }
        }
    }
}
