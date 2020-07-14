using UnityEngine;
using UnityEngine.UI;

namespace NRKernal.NRExamples
{
    [HelpURL("https://developer.nreal.ai/develop/unity/rgb-camera")]
    public class CameraCaptureController : MonoBehaviour
    {
        public RawImage CaptureImage;
        public Text FrameCount;
        private NRRGBCamTexture RGBCamTexture { get; set; }

        private void Start()
        {
            RGBCamTexture = new NRRGBCamTexture();
            CaptureImage.texture = RGBCamTexture.GetTexture();
            RGBCamTexture.Play();
        }

        void Update()
        {
            FrameCount.text = RGBCamTexture.FrameCount.ToString();
        }

        public void Play()
        {
            RGBCamTexture.Play();

            // The origin texture will be destroyed after call "Stop",
            // Rebind the texture.
            CaptureImage.texture = RGBCamTexture.GetTexture();
        }

        public void Pause()
        {
            RGBCamTexture.Pause();
        }

        public void Stop()
        {
            RGBCamTexture.Stop();
        }

        void OnDestroy()
        {
            RGBCamTexture.Stop();
        }
    }
}
