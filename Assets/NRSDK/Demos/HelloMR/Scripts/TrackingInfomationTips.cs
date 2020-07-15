using System.Collections.Generic;
using UnityEngine;

namespace NRKernal.NRExamples
{
    [HelpURL("https://developer.nreal.ai/develop/discover/introduction-nrsdk")]
    public class TrackingInfomationTips : SingletonBehaviour<TrackingInfomationTips>
    {
        private Dictionary<TipType, GameObject> m_TipsDict = new Dictionary<TipType, GameObject>();
        public enum TipType
        {
            UnInitialized,
            LostTracking,
            None
        }
        [Tooltip("Camera would only show the layer of TipsLayer when lost tracking.\n" +
            "Select the layer which you want to show when lost tracking.")]
        [SerializeField]
        private LayerMask m_TipsLayer;
        private GameObject m_CurrentTip;
        private Camera centerCamera;

        private int originLayerLCam;
        private int originLayerRCam;

        void Start()
        {
            originLayerLCam = NRSessionManager.Instance.NRHMDPoseTracker.leftCamera.cullingMask;
            originLayerRCam = NRSessionManager.Instance.NRHMDPoseTracker.rightCamera.cullingMask;
            centerCamera = NRSessionManager.Instance.NRHMDPoseTracker.centerCamera;
            ShowTips(TipType.UnInitialized);
        }

        private void OnEnable()
        {
            NRHMDPoseTracker.OnHMDLostTracking += OnHMDLostTracking;
            NRHMDPoseTracker.OnHMDPoseReady += OnHMDPoseReady;
        }

        private void OnDisable()
        {
            NRHMDPoseTracker.OnHMDLostTracking -= OnHMDLostTracking;
            NRHMDPoseTracker.OnHMDPoseReady -= OnHMDPoseReady;
        }

        private void OnHMDPoseReady()
        {
            Debug.Log("[NRHMDPoseTracker] OnHMDPoseReady");
            ShowTips(TipType.None);
        }

        private void OnHMDLostTracking()
        {
            Debug.Log("[NRHMDPoseTracker] OnHMDLostTracking");
            ShowTips(TipType.LostTracking);
        }

        public void ShowTips(TipType type)
        {
            switch (type)
            {
                case TipType.UnInitialized:
                case TipType.LostTracking:
                    GameObject go;
                    m_TipsDict.TryGetValue(type, out go);
                    int layer = LayerMaskToLayer(m_TipsLayer);
                    if (go == null)
                    {
                        go = Instantiate(Resources.Load(type.ToString() + "Tip"), centerCamera.transform) as GameObject;
                        m_TipsDict.Add(type, go);
                        go.layer = layer;
                        foreach (Transform child in go.transform)
                        {
                            child.gameObject.layer = layer;
                        }
                    }
                    if (go != m_CurrentTip)
                    {
                        m_CurrentTip?.SetActive(false);
                        go.SetActive(true);
                        m_CurrentTip = go;
                    }
                    NRSessionManager.Instance.NRHMDPoseTracker.leftCamera.cullingMask = 1 << layer;
                    NRSessionManager.Instance.NRHMDPoseTracker.rightCamera.cullingMask = 1 << layer;
                    break;
                case TipType.None:
                    if (m_CurrentTip != null)
                    {
                        m_CurrentTip.SetActive(false);
                    }
                    m_CurrentTip = null;
                    NRSessionManager.Instance.NRHMDPoseTracker.leftCamera.cullingMask = originLayerLCam;
                    NRSessionManager.Instance.NRHMDPoseTracker.rightCamera.cullingMask = originLayerRCam;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Layer mask to layer.
        /// </summary>
        /// <param name="layerMask"></param>
        /// <returns>The last layer of the layer mask.</returns>
        public static int LayerMaskToLayer(LayerMask layerMask)
        {
            int layerNumber = 0;
            int layer = layerMask.value;
            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            return layerNumber - 1;
        }

        new void OnDestroy()
        {
            if (isDirty) return;
            if (m_TipsDict != null)
            {
                foreach (var item in m_TipsDict)
                {
                    if (item.Value != null)
                    {
                        GameObject.Destroy(item.Value);
                    }
                }
            }
        }
    }
}