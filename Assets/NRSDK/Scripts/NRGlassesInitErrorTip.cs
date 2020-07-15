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
    using UnityEngine;
    using UnityEngine.UI;

    public class NRGlassesInitErrorTip : MonoBehaviour
    {
        public event Action OnConfirm;
        public Button m_ConfirmBtn;
        public Text m_Tips;

        public void Init(string msg, Action confirm)
        {
            m_Tips.text = msg;
            OnConfirm += confirm;
            m_ConfirmBtn.onClick.AddListener(() =>
            {
                OnConfirm?.Invoke();
            });
        }

        void Start()
        {
            var inputmodule = GameObject.FindObjectOfType<NRInputModule>();
            if (inputmodule != null)
            {
                GameObject.Destroy(inputmodule.gameObject);
            }
        }
    }
}
