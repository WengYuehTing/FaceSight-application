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
    using UnityEngine;

    /// <summary>
    /// Oprate AR system state and handles the session lifecycle for application layer.
    /// </summary>
    [HelpURL("https://developer.nreal.ai/develop/discover/introduction-nrsdk")]
    public class NRSessionBehaviour : SingletonBehaviour<NRSessionBehaviour>
    {
        /// <summary>
        /// The SessionConfig of nrsession.
        /// </summary>
        [Tooltip("A scriptable object specifying the NRSDK session configuration.")]
        public NRSessionConfig SessionConfig;

        new void Awake()
        {
            base.Awake();

            if (isDirty) return;
#if !UNITY_EDITOR
            NRDebugger.EnableLog = Debug.isDebugBuild;
#endif
            NRDebugger.Log("[SessionBehaviour Awake: CreateSession]");
            NRSessionManager.Instance.CreateSession(this);
        }

        void Start()
        {
            if (isDirty) return;
            NRDebugger.Log("[SessionBehaviour DelayStart: StartSession]");
            NRSessionManager.Instance.StartSession();
            NRSessionManager.Instance.SetConfiguration(SessionConfig);
        }

        private void OnApplicationPause(bool pause)
        {
            NRDebugger.LogFormat("[SessionBehaviour OnApplicationPause: {0}]", pause);
            if (isDirty) return;
            if (pause)
            {
                NRSessionManager.Instance.DisableSession();
            }
            else
            {
                NRSessionManager.Instance.ResumeSession();
            }
        }

        void OnDisable()
        {
            if (isDirty) return;
            NRDebugger.Log("[SessionBehaviour OnDisable: DisableSession]");
            NRSessionManager.Instance.DisableSession();
        }

        new void OnDestroy()
        {
            if (isDirty) return;
            base.OnDestroy();
            NRDebugger.Log("[SessionBehaviour OnDestroy DestroySession]");
            NRSessionManager.Instance.DestroySession();
        }
    }
}
