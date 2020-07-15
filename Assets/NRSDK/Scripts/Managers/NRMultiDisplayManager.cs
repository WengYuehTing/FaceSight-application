using UnityEngine;

namespace NRKernal
{
    [HelpURL("https://developer.nreal.ai/develop/unity/customize-phone-controller")]
    public class NRMultiDisplayManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_DefaultVirtualDisplayer;
        private NRVirtualDisplayer m_VirtualDisplayer;

        private void Start()
        {
#if !UNITY_STANDALONE_WIN
            Init();
#endif
        }

        private void Init()
        {
            m_VirtualDisplayer = FindObjectOfType<NRVirtualDisplayer>();
            if (m_VirtualDisplayer == null)
            {
                Instantiate(m_DefaultVirtualDisplayer);
            }
        }
    }
}
