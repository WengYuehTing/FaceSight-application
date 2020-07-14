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
    using NRKernal;
    using UnityEngine;
    using System.Collections;

    [RequireComponent(typeof(Camera))]
    public class NRCameraInitializer : MonoBehaviour
    {
        private Camera m_TargetCamera;
        [SerializeField]
        private NativeEye EyeType = NativeEye.RGB;

#if UNITY_EDITOR
        private Matrix4x4 matrix = new Matrix4x4(
                   new Vector4(1.92188f, 0f, 0f, 0f),
                   new Vector4(0f, 3.41598f, 0f, 0f),
                   new Vector4(0.0169f, 0.02256f, -1.00060f, -1f),
                   new Vector4(0, 0f, -0.60018f, 0f)
           );
#endif

        void Start()
        {
            m_TargetCamera = gameObject.GetComponent<Camera>();

#if UNITY_EDITOR
            m_TargetCamera.projectionMatrix = matrix;
#else
            StartCoroutine(Initialize());
#endif
        }

        private IEnumerator Initialize()
        {
            bool result;

            EyeProjectMatrixData matrix_data = NRFrame.GetEyeProjectMatrix(out result, m_TargetCamera.nearClipPlane, m_TargetCamera.farClipPlane);
            while (!result)
            {
                Debug.Log("Waitting to initialize camera param.");
                yield return new WaitForEndOfFrame();
                matrix_data = NRFrame.GetEyeProjectMatrix(out result, m_TargetCamera.nearClipPlane, m_TargetCamera.farClipPlane);
            }

            var eyeposFromHead = NRFrame.EyePosFromHead;
            switch (EyeType)
            {
                case NativeEye.LEFT:
                    m_TargetCamera.projectionMatrix = matrix_data.LEyeMatrix;
                    NRDebugger.Log("[Matrix] RGB Camera Project Matrix :" + m_TargetCamera.projectionMatrix.ToString());
                    transform.localPosition = eyeposFromHead.LEyePose.position;
                    transform.localRotation = eyeposFromHead.LEyePose.rotation;
                    NRDebugger.LogFormat("RGB Camera pos:{0} rotation:{1}", transform.localPosition.ToString(), transform.localRotation.ToString());
                    break;
                case NativeEye.RIGHT:
                    m_TargetCamera.projectionMatrix = matrix_data.REyeMatrix;
                    NRDebugger.Log("[Matrix] RGB Camera Project Matrix :" + m_TargetCamera.projectionMatrix.ToString());
                    transform.localPosition = eyeposFromHead.REyePose.position;
                    transform.localRotation = eyeposFromHead.REyePose.rotation;
                    NRDebugger.LogFormat("RGB Camera pos:{0} rotation:{1}", transform.localPosition.ToString(), transform.localRotation.ToString());
                    break;
                case NativeEye.RGB:
                    m_TargetCamera.projectionMatrix = matrix_data.RGBEyeMatrix;
                    NRDebugger.Log("[Matrix] RGB Camera Project Matrix :" + m_TargetCamera.projectionMatrix.ToString());
                    transform.localPosition = eyeposFromHead.RGBEyePos.position;
                    transform.localRotation = eyeposFromHead.RGBEyePos.rotation;
                    NRDebugger.LogFormat("RGB Camera pos:{0} rotation:{1}", transform.localPosition.ToString(), transform.localRotation.ToString());
                    break;
                default:
                    break;
            }
        }
    }
}
