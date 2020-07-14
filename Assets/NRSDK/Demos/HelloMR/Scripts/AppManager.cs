using UnityEngine;

namespace NRKernal.NRExamples
{
    [DisallowMultipleComponent]
    [HelpURL("https://developer.nreal.ai/develop/discover/introduction-nrsdk")]
    public class AppManager : MonoBehaviour
    {
        private float m_LastClickTime = 0;
        private int m_CumulativeClickNum = 0;
        private bool m_IsProfilerOpened = false;
        private const int triggerClickCount = 3;

        private void OnEnable()
        {
            NRInput.AddClickListener(ControllerHandEnum.Right, ControllerButton.HOME, OnHomeButtonClick);
            NRInput.AddClickListener(ControllerHandEnum.Left, ControllerButton.HOME, OnHomeButtonClick);
            NRInput.AddClickListener(ControllerHandEnum.Right, ControllerButton.APP, OnAppButtonClick);
            NRInput.AddClickListener(ControllerHandEnum.Left, ControllerButton.APP, OnAppButtonClick);
        }

        private void OnDisable()
        {
            NRInput.RemoveClickListener(ControllerHandEnum.Right, ControllerButton.HOME, OnHomeButtonClick);
            NRInput.RemoveClickListener(ControllerHandEnum.Left, ControllerButton.HOME, OnHomeButtonClick);
            NRInput.RemoveClickListener(ControllerHandEnum.Right, ControllerButton.APP, OnAppButtonClick);
            NRInput.RemoveClickListener(ControllerHandEnum.Left, ControllerButton.APP, OnAppButtonClick);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
                QuitApplication();
#endif
        }

        private void OnHomeButtonClick()
        {
            NRHomeMenu.Toggle();
        }

        private void OnAppButtonClick()
        {
            NRHomeMenu.Hide();
            CollectClickEvent();
        }

        private void CollectClickEvent()
        {
            if (Time.unscaledTime - m_LastClickTime < 0.2f)
            {
                m_CumulativeClickNum++;
                if (m_CumulativeClickNum == (triggerClickCount - 1))
                {
                    // Show the VisualProfiler
                    NRVisualProfiler.Instance.Switch(!m_IsProfilerOpened);
                    m_IsProfilerOpened = !m_IsProfilerOpened;
                    m_CumulativeClickNum = 0;
                }
            }
            else
            {
                m_CumulativeClickNum = 0;
            }
            m_LastClickTime = Time.unscaledTime;
        }

        public static void QuitApplication()
        {
            NRDevice.QuitApp();
        }
    }
}
