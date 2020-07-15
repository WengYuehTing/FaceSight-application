namespace NRKernal
{
    using UnityEngine;

    public class NRPhoneScreen
    {
        private static float m_ScreenWidth = 0;
        private static float m_ScreenHeight = 0;

        public const float DefaultWidth = 1080;
        public const float DefaultHeight = 2340;

        public static Vector2 Resolution
        {
            get
            {
                if (m_ScreenWidth < float.Epsilon || m_ScreenHeight < float.Epsilon)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                    AndroidJavaClass j = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = j.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject displayManager = currentActivity.Call<AndroidJavaObject>("getSystemService", new AndroidJavaObject("java.lang.String", "display"));
                    AndroidJavaObject display = displayManager.Call<AndroidJavaObject>("getDisplay", 0);
                    AndroidJavaObject outSize = new AndroidJavaObject("android.graphics.Point");
                    display.Call("getRealSize", outSize);
                    m_ScreenWidth = outSize.Get<int>("x");
                    m_ScreenHeight = outSize.Get<int>("y");
#else
                    m_ScreenWidth = DefaultWidth;
                    m_ScreenHeight = DefaultHeight;
#endif
                    NRDebugger.Log(string.Format("[NRPhoneScreen] width:{0} height:{1}", m_ScreenWidth, m_ScreenHeight));
                }

                return new Vector2(m_ScreenWidth, m_ScreenHeight);
            }
        }
    }
}
