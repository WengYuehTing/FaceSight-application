/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/                  
* 
*****************************************************************************/

namespace NRKernal
{
    /// <summary>
    /// RGB Camera's image format.
    /// </summary>
    internal enum CameraImageFormat
    {
        /// <summary>
        /// YUV Not Supported now.
        /// </summary>
        YUV_420_888 = 1,

        /// <summary>
        /// RGB three channel format.
        /// </summary>
        RGB_888 = 2
    }
}
