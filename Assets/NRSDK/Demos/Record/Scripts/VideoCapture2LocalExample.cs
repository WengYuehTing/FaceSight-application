using NRKernal.Record;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NRKernal.NRExamples
{
    [HelpURL("https://developer.nreal.ai/develop/unity/video-capture")]
    public class VideoCapture2LocalExample : MonoBehaviour
    {
        public NRPreviewer Previewer;

        // Save the video to Application.persistentDataPath
        public string VideoSavePath
        {
            get
            {
                string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
                string filename = string.Format("TestVideo_{0}.mp4", timeStamp);
                string filepath = Path.Combine(Application.persistentDataPath, filename);
                return filepath;
            }
        }

        NRVideoCapture m_VideoCapture = null;

        void Start()
        {
            CreateVideoCaptureTest();
        }

        void CreateVideoCaptureTest()
        {
            NRVideoCapture.CreateAsync(false, delegate (NRVideoCapture videoCapture)
            {
                if (videoCapture != null)
                {
                    m_VideoCapture = videoCapture;
                }
                else
                {
                    Debug.LogError("Failed to create VideoCapture Instance!");
                }
            });
        }

        public void StartVideoCapture()
        {
            Resolution cameraResolution = NRVideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            Debug.Log(cameraResolution);

            int cameraFramerate = NRVideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();
            Debug.Log(cameraFramerate);

            if (m_VideoCapture != null)
            {
                Debug.Log("Created VideoCapture Instance!");
                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.frameRate = cameraFramerate;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;
                cameraParameters.blendMode = BlendMode.Blend;

                m_VideoCapture.StartVideoModeAsync(cameraParameters, OnStartedVideoCaptureMode);

                //Previewer.SetData(m_VideoCapture.PreviewTexture, true);
            }
        }

        public void StopVideoCapture()
        {
            if (m_VideoCapture == null)
            {
                return;
            }
            Debug.Log("Stop Video Capture!");
            m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
            //Previewer.SetData(m_VideoCapture.PreviewTexture, false);
        }

        void OnStartedVideoCaptureMode(NRVideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Started Video Capture Mode!");
            m_VideoCapture.StartRecordingAsync(VideoSavePath, OnStartedRecordingVideo);
        }

        void OnStartedRecordingVideo(NRVideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Started Recording Video!");
        }

        void OnStoppedRecordingVideo(NRVideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Stopped Recording Video!");
            m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
        }

        void OnStoppedVideoCaptureMode(NRVideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Stopped Video Capture Mode!");
        }
    }
}
