namespace NRKernal.Record
{
    public struct CameraParameters
    {
        public CameraParameters(CamMode webCamMode, BlendMode mode)
        {
            this.camMode = webCamMode;
            this.hologramOpacity = 1f;
            this.frameRate = 20;

            this.cameraResolutionWidth = NRRgbCamera.Resolution.width;
            this.cameraResolutionHeight = NRRgbCamera.Resolution.height;

            this.pixelFormat = CapturePixelFormat.BGRA32;
            this.blendMode = mode;
        }

        /// <summary>
        /// The opacity of captured holograms.
        /// </summary>
        public float hologramOpacity { get; set; }

        /// <summary>
        /// The framerate at which to capture video. This is only for use with VideoCapture.
        /// </summary>
        public int frameRate { get; set; }

        /// <summary>
        /// A valid width resolution for use with the web camera.
        /// </summary>
        public int cameraResolutionWidth { get; set; }

        /// <summary>
        /// A valid height resolution for use with the web camera.
        /// </summary>
        public int cameraResolutionHeight { get; set; }

        /// <summary>
        /// The pixel format used to capture and record your image data.
        /// </summary>
        public CapturePixelFormat pixelFormat { get; set; }

        /// <summary>
        /// The camera mode of capture.
        /// </summary>
        public CamMode camMode { get; set; }

        /// <summary>
        /// The blend mode of camera output.
        /// </summary>
        public BlendMode blendMode { get; set; }
    }
}
