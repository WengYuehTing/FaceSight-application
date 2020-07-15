using NRKernal.Record;
using System.Linq;
using UnityEngine;

namespace NRKernal.NRExamples
{
    [HelpURL("https://developer.nreal.ai/develop/unity/video-capture")]
    public class PhotoCaptureExample : MonoBehaviour
    {
        public NRPreviewer Previewer;
        private NRPhotoCapture m_PhotoCaptureObject;
        private Resolution m_CameraResolution;

        void Start()
        {
            this.Create();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T) || NRInput.GetButtonDown(ControllerButton.TRIGGER))
            {
                TakeAPhoto();
            }

            if (Input.GetKeyDown(KeyCode.Q) || NRInput.GetButtonDown(ControllerButton.HOME))
            {
                Close();
            }

            if (Input.GetKeyDown(KeyCode.O) || NRInput.GetButtonDown(ControllerButton.APP))
            {
                Create();
            }

            if (m_PhotoCaptureObject != null)
            {
                Previewer.SetData(m_PhotoCaptureObject.PreviewTexture, true);
            }
        }

        // Use this for initialization
        void Create()
        {
            if (m_PhotoCaptureObject != null)
            {
                Debug.LogError("The NRPhotoCapture has already been created.");
                return;
            }

            // Create a PhotoCapture object
            NRPhotoCapture.CreateAsync(false, delegate (NRPhotoCapture captureObject)
            {
                m_CameraResolution = NRPhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

                if (captureObject != null)
                {
                    m_PhotoCaptureObject = captureObject;
                }
                else
                {
                    Debug.LogError("Can not get a captureObject.");
                }

                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.cameraResolutionWidth = m_CameraResolution.width;
                cameraParameters.cameraResolutionHeight = m_CameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;
                cameraParameters.blendMode = BlendMode.Blend;

                // Activate the camera
                m_PhotoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (NRPhotoCapture.PhotoCaptureResult result)
                {
                    Debug.Log("Start PhotoMode Async");
                });
            });
        }

        void TakeAPhoto()
        {
            if (m_PhotoCaptureObject == null)
            {
                Debug.Log("The NRPhotoCapture has not been created.");
                return;
            }
            // Take a picture
            m_PhotoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        }

        void OnCapturedPhotoToMemory(NRPhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            var targetTexture = new Texture2D(m_CameraResolution.width, m_CameraResolution.height);
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);

            // Create a gameobject that we can apply our texture to
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
            quadRenderer.material = new Material(Resources.Load<Shader>("Record/Shaders/CaptureScreen"));

            var headTran = NRSessionManager.Instance.NRHMDPoseTracker.centerCamera.transform;
            quad.name = "picture";
            quad.transform.localPosition = headTran.position + headTran.forward * 3f;
            quad.transform.forward = headTran.forward;
            quad.transform.localScale = new Vector3(1.6f, 0.9f, 0);
            quadRenderer.material.SetTexture("_MainTex", targetTexture);
        }

        void Close()
        {
            if (m_PhotoCaptureObject == null)
            {
                Debug.LogError("The NRPhotoCapture has not been created.");
                return;
            }
            // Deactivate our camera
            m_PhotoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }

        void OnStoppedPhotoMode(NRPhotoCapture.PhotoCaptureResult result)
        {
            if (m_PhotoCaptureObject == null)
            {
                Debug.LogError("The NRPhotoCapture has not been created.");
                return;
            }
            // Shutdown our photo capture resource
            m_PhotoCaptureObject.Dispose();
            m_PhotoCaptureObject = null;
        }

        void OnDestroy()
        {
            // Shutdown our photo capture resource
            m_PhotoCaptureObject?.Dispose();
        }
    }
}
