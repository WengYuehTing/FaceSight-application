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
    using System;
    using System.Runtime.InteropServices;

    public partial class NativeGlassesController
    {
        /// <summary>
        /// The callback method type which will be called when put on or take off nreal glasses
        /// </summary>
        /// <param name="glasses_control_handle">glasses_control_handle The handle of GlassesControl.</param>
        /// <param name="wear_or_not">wear_or_not , 1: put on glasses . 0: take off glasses.</param>
        /// <param name="user_data">user_data  The custom user data.</param>
        public delegate void NRGlassesControlWearCallback(UInt64 glasses_control_handle, int wearing_status, UInt64 user_data);
        /// <summary>
        /// The callback method type which will be called when plug off nreal glasses
        /// </summary>
        /// <param name="glasses_control_handle">glasses_control_handle The handle of GlassesControl.</param>
        /// <param name="user_data">user_data The custom user data.</param>
        public delegate void NRGlassesControlPlugOffCallback(UInt64 glasses_control_handle, UInt64 user_data);

        private UInt64 m_GlassesControllerHandle = 0;

        public void Create()
        {
            NativeResult result = NativeApi.NRGlassesControlCreate(ref m_GlassesControllerHandle);
            NativeErrorListener.Check(result, this, "Create");
        }

        public void RegisGlassesWearCallBack(NRGlassesControlWearCallback callback, ulong userdata)
        {
            NativeResult result = NativeApi.NRGlassesControlSetGlassesWearingCallback(m_GlassesControllerHandle, callback, userdata);
            NativeErrorListener.Check(result, this, "RegisGlassesWearCallBack");
        }

        public void RegisGlassesPlugOutCallBack(NRGlassesControlPlugOffCallback callback, ulong userdata)
        {
            NativeResult result = NativeApi.NRGlassesControlSetGlassesDisconnectedCallback(m_GlassesControllerHandle, callback, userdata);
            NativeErrorListener.Check(result, this, "RegisGlassesPlugOutCallBack");
        }

        public void Start()
        {
            NativeResult result = NativeApi.NRGlassesControlStart(m_GlassesControllerHandle);
            NativeErrorListener.Check(result, this, "Start", true);
        }

        public void Pause()
        {
            NativeResult result = NativeApi.NRGlassesControlPause(m_GlassesControllerHandle);
            NativeErrorListener.Check(result, this, "Pause", true);
        }

        public void Resume()
        {
            NativeResult result = NativeApi.NRGlassesControlResume(m_GlassesControllerHandle);
            NativeErrorListener.Check(result, this, "Resume", true);
        }

        public void Stop()
        {
            NativeResult result = NativeApi.NRGlassesControlStop(m_GlassesControllerHandle);
            NativeErrorListener.Check(result, this, "Stop");
        }

        public void Destroy()
        {
            NativeResult result = NativeApi.NRGlassesControlDestroy(m_GlassesControllerHandle);
            NativeErrorListener.Check(result, this, "Destroy");
        }

        private partial struct NativeApi
        {
            /// <summary>
            /// Create the GlassesControl object.
            /// </summary>
            /// <param name="out_glasses_control_handle">The handle of GlassesControl.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlCreate(ref UInt64 out_glasses_control_handle);

            /// <summary>
            /// Start the GlassesControl system.
            /// </summary>
            /// <param name="glasses_control_handle">The handle of GlassesControl.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlStart(UInt64 glasses_control_handle);

            /// <summary>
            /// Pause the GlassesControl system.
            /// </summary>
            /// <param name="glasses_control_handle">The handle of GlassesControl.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlPause(UInt64 glasses_control_handle);


            /// <summary>
            /// Resume the GlassesControl system.
            /// </summary>
            /// <param name="glasses_control_handle">The handle of GlassesControl.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlResume(UInt64 glasses_control_handle);

            /// <summary>
            /// Stop the GlassesControl system.
            /// </summary>
            /// <param name="glasses_control_handle">The handle of GlassesControl.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlStop(UInt64 glasses_control_handle);

            /// <summary>
            /// Release memory used by the GlassesControl.
            /// </summary>
            /// <param name="glasses_control_handle">The handle of GlassesControl.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlDestroy(UInt64 glasses_control_handle);

            /// <summary>
            /// Set the callback method when put on or take off glasses.
            /// </summary>
            /// <param name="glasses_control_handle">The handle of GlassesControl.</param>
            /// <param name="data_callback">The callback method.</param>
            /// <param name="user_data">The data which will be returned when callback is triggered.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlSetGlassesWearingCallback(
                    UInt64 glasses_control_handle, NRGlassesControlWearCallback data_callback, UInt64 user_data);

            /// <summary>
            /// Set the callback method when plug off the glasses.
            /// </summary>
            /// <param name="glasses_control_handle">The handle of GlassesControl.</param>
            /// <param name="data_callback">The callback method.</param>
            /// <param name="user_data">The data which will be returned when callback is triggered.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlSetGlassesDisconnectedCallback(
                    UInt64 glasses_control_handle, NRGlassesControlPlugOffCallback data_callback, UInt64 user_data);
        }
    }
}
