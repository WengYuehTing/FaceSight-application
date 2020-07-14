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
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using UnityEngine;

    internal partial class NativeConfigration
    {
        private NativeInterface m_NativeInterface;
        private Dictionary<string, UInt64> m_TrackableImageDatabaseDict;

        private UInt64 m_ConfigHandle = 0;
        private UInt64 m_DatabaseHandle = 0;

        private NRSessionConfig m_LastSessionConfig;
        private NativeTrackableImage m_NativeTrackableImage;
        private bool m_IsUpdateConfigLock = false;

        public NativeConfigration(NativeInterface nativeInterface)
        {
            m_NativeInterface = nativeInterface;
            m_LastSessionConfig = new NRSessionConfig();
            m_NativeTrackableImage = m_NativeInterface.NativeTrackableImage;
            m_TrackableImageDatabaseDict = new Dictionary<string, ulong>();
        }

        public async Task<bool> UpdateConfig(NRSessionConfig config)
        {
            if (m_IsUpdateConfigLock)
            {
                return false;
            }
            m_IsUpdateConfigLock = true;
            if (m_ConfigHandle == 0)
            {
                m_ConfigHandle = this.Create();
            }

            if (m_ConfigHandle == 0 || m_LastSessionConfig.Equals(config))
            {
                NRDebugger.Log("[NativeConfigration] Faild to Update NRSessionConfig!!!");
                m_IsUpdateConfigLock = false;
                return false;
            }

            await UpdatePlaneFindMode(config);
            await UpdateImageTrackingConfig(config);

            m_LastSessionConfig.CopyFrom(config);
            m_IsUpdateConfigLock = false;
            return true;
        }

        private Task UpdatePlaneFindMode(NRSessionConfig config)
        {
            return Task.Run(() =>
            {
                var currentmode = this.GetPlaneFindMode(m_ConfigHandle);
                if (currentmode != config.PlaneFindingMode)
                {
                    SetPlaneFindMode(m_ConfigHandle, config.PlaneFindingMode);
                }
            });
        }

        private Task UpdateImageTrackingConfig(NRSessionConfig config)
        {
            return Task.Run(() =>
            {
                switch (config.ImageTrackingMode)
                {
                    case TrackableImageFindingMode.DISABLE:
                        var result = SetTrackableImageDataBase(m_ConfigHandle, 0);
                        if (result)
                        {
                            m_TrackableImageDatabaseDict.Clear();
                        }
                        NRDebugger.Log("[NativeConfigration] Disable trackable image result : " + result);
                        break;
                    case TrackableImageFindingMode.ENABLE:
                        if (config.TrackingImageDatabase == null)
                        {
                            return;
                        }

                        if (!m_TrackableImageDatabaseDict.TryGetValue(config.TrackingImageDatabase.GUID, out m_DatabaseHandle))
                        {
                            DeployData(config.TrackingImageDatabase);
                            m_DatabaseHandle = m_NativeTrackableImage.CreateDataBase();
                            m_TrackableImageDatabaseDict.Add(config.TrackingImageDatabase.GUID, m_DatabaseHandle);
                        }
                        result = m_NativeTrackableImage.LoadDataBase(m_DatabaseHandle, config.TrackingImageDatabase.TrackingImageDataPath);
                        NRDebugger.LogFormat("[NativeConfigration] LoadDataBase path:{0} result:{1} ", config.TrackingImageDatabase.TrackingImageDataPath, result);
                        result = SetTrackableImageDataBase(m_ConfigHandle, m_DatabaseHandle);
                        NRDebugger.Log("[NativeConfigration] SetTrackableImageDataBase result : " + result);
                        break;
                    default:
                        break;
                }
            });
        }

        private void DeployData(NRTrackingImageDatabase database)
        {
            string deploy_path = database.TrackingImageDataOutPutPath;
            NRDebugger.Log("[TrackingImageDatabase] DeployData to path :" + deploy_path);
            ZipUtility.UnzipFile(database.RawData, deploy_path, NativeConstants.ZipKey);
        }

        private UInt64 Create()
        {
            UInt64 config_handle = 0;
            var result = NativeApi.NRConfigCreate(m_NativeInterface.TrackingHandle, ref config_handle);
            NativeErrorListener.Check(result, this, "Create");
            return config_handle;
        }

        public TrackablePlaneFindingMode GetPlaneFindMode(UInt64 config_handle)
        {
            TrackablePlaneFindingMode mode = TrackablePlaneFindingMode.DISABLE;
            var result = NativeApi.NRConfigGetTrackablePlaneFindingMode(m_NativeInterface.TrackingHandle, config_handle, ref mode);
            NativeErrorListener.Check(result, this, "GetPlaneFindMode");
            return mode;
        }

        public bool SetPlaneFindMode(UInt64 config_handle, TrackablePlaneFindingMode mode)
        {
            int mode_value;
            switch (mode)
            {
                case TrackablePlaneFindingMode.DISABLE:
                case TrackablePlaneFindingMode.HORIZONTAL:
                case TrackablePlaneFindingMode.VERTICLE:
                    mode_value = (int)mode;
                    break;
                case TrackablePlaneFindingMode.BOTH:
                    mode_value = ((int)TrackablePlaneFindingMode.HORIZONTAL) | ((int)TrackablePlaneFindingMode.VERTICLE);
                    break;
                default:
                    mode_value = (int)TrackablePlaneFindingMode.DISABLE;
                    break;
            }
            var result = NativeApi.NRConfigSetTrackablePlaneFindingMode(m_NativeInterface.TrackingHandle, config_handle, mode_value);
            NativeErrorListener.Check(result, this, "SetPlaneFindMode");
            return result == NativeResult.Success;
        }

        public UInt64 GetTrackableImageDataBase(UInt64 config_handle)
        {
            UInt64 database_handle = 0;
            var result = NativeApi.NRConfigGetTrackableImageDatabase(m_NativeInterface.TrackingHandle, config_handle, ref database_handle);
            NativeErrorListener.Check(result, this, "GetTrackableImageDataBase");
            return database_handle;
        }

        public bool SetTrackableImageDataBase(UInt64 config_handle, UInt64 database_handle)
        {
            var result = NativeApi.NRConfigSetTrackableImageDatabase(m_NativeInterface.TrackingHandle, config_handle, database_handle);
            NativeErrorListener.Check(result, this, "SetTrackableImageDataBase");
            return result == NativeResult.Success;
        }

        public bool Destroy(UInt64 config_handle)
        {
            var result = NativeApi.NRConfigDestroy(m_NativeInterface.TrackingHandle, config_handle);
            NativeErrorListener.Check(result, this, "Destroy");
            return result == NativeResult.Success;
        }

        private struct NativeApi
        {
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRConfigCreate(UInt64 session_handle, ref UInt64 out_config_handle);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRConfigDestroy(UInt64 session_handle, UInt64 config_handle);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRConfigGetTrackablePlaneFindingMode(UInt64 session_handle,
                UInt64 config_handle, ref TrackablePlaneFindingMode out_trackable_plane_finding_mode);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRConfigSetTrackablePlaneFindingMode(UInt64 session_handle,
                UInt64 config_handle, int trackable_plane_finding_mode);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRConfigGetTrackableImageDatabase(UInt64 session_handle,
                UInt64 config_handle, ref UInt64 out_trackable_image_database_handle);

            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRConfigSetTrackableImageDatabase(UInt64 session_handle,
                UInt64 config_handle, UInt64 trackable_image_database_handle);
        };
    }
}
