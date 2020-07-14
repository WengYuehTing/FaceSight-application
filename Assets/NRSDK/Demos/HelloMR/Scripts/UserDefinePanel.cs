using System.Collections;
using UnityEngine;

namespace NRKernal.NRExamples
{
    [HelpURL("https://developer.nreal.ai/develop/unity/customize-phone-controller")]
    public class UserDefinePanel : MonoBehaviour
    {
        public GameObject m_UserDefinePanel;

        void Start()
        {
            StartCoroutine(RigistUserDefinePanel());
        }

        private IEnumerator RigistUserDefinePanel()
        {
            while (GameObject.FindObjectOfType<NRVirtualDisplayer>() == null)
            {
                yield return new WaitForEndOfFrame();
            }
            var virtualdisplayer = GameObject.FindObjectOfType<NRVirtualDisplayer>();
            var root = virtualdisplayer.transform.GetComponentInChildren<Canvas>().transform;
            Instantiate(m_UserDefinePanel, root);
        }
    }
}