/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal.Record
{
    public enum CodecType
    {
        Local = 0,
        Rtmp = 1,
        Rtp = 2,
    }

    public enum BlendMode
    {
        Blend,
        RGBOnly,
        VirtualOnly,
        WidescreenBlend
    }

    public delegate void CaptureTaskCallback(CaptureTask task, byte[] data);

    public struct CaptureTask
    {
        public int Width;
        public int Height;
        public PhotoCaptureFileOutputFormat CaptureFormat;
        public CaptureTaskCallback OnReceive;

        public CaptureTask(int w, int h, PhotoCaptureFileOutputFormat format, CaptureTaskCallback callback)
        {
            this.Width = w;
            this.Height = h;
            this.CaptureFormat = format;
            this.OnReceive = callback;
        }
    }

    public struct NativeEncodeConfig
    {
        public int width { get; private set; }
        public int height { get; private set; }
        public int bitRate { get; private set; }
        public int fps { get; private set; }
        public int codecType { get; private set; }    // 0 local; 1 rtmp ; 2 rtp
        public string outPutPath { get; private set; }
        public int useStepTime { get; private set; }
        public bool useAlpha { get; private set; }
        public bool useLinnerTexture { get; private set; }

        public NativeEncodeConfig(int w, int h, int bitrate, int f, CodecType codectype, string path, bool usealpha = false)
        {
            this.width = w;
            this.height = h;
            this.bitRate = bitrate;
            this.fps = 20;
            this.codecType = (int)codectype;
            this.outPutPath = path;
            this.useStepTime = 0;
            this.useAlpha = usealpha;
            this.useLinnerTexture = NRRenderer.isLinearColorSpace;
        }

        public NativeEncodeConfig(CameraParameters cameraparam, string path = "")
        {
            this.width = cameraparam.blendMode == BlendMode.WidescreenBlend ? 2 * cameraparam.cameraResolutionWidth : cameraparam.cameraResolutionWidth;
            this.height = cameraparam.cameraResolutionHeight;
            this.bitRate = 10240000;
            this.fps = cameraparam.frameRate;
            this.codecType = GetCodecTypeByPath(path);
            this.outPutPath = path;
            this.useStepTime = 0;
            this.useAlpha = cameraparam.hologramOpacity < float.Epsilon;
            this.useLinnerTexture = NRRenderer.isLinearColorSpace;
        }

        public void SetOutPutPath(string path)
        {
            this.codecType = GetCodecTypeByPath(path);
            this.outPutPath = path;
        }

        private static int GetCodecTypeByPath(string path)
        {
            if (path.StartsWith("rtmp://"))
            {
                return 1;
            }
            else if (path.StartsWith("rtp://"))
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public NativeEncodeConfig(NativeEncodeConfig config)
            : this(config.width, config.height, config.bitRate, config.fps, (CodecType)config.codecType, config.outPutPath, config.useAlpha)
        {
        }

        public override string ToString()
        {
            return LitJson.JsonMapper.ToJson(this);
        }
    }
}
