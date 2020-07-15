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
    using UnityEngine.EventSystems;

    internal class NRButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Sprite ImageNormal;
        [SerializeField]
        private Sprite ImageHover;
        public Action<string, GameObject, RaycastResult> TriggerEvent;
        public const string Enter = "Enter";
        public const string Hover = "Hover";
        public const string Exit = "Exit";

        private Image m_ButtonImage;

        void Start()
        {
            m_ButtonImage = gameObject.GetComponent<Image>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (TriggerEvent != null)
            {
                TriggerEvent(Enter, gameObject, eventData.pointerCurrentRaycast);
            }

            if (ImageHover != null && m_ButtonImage != null)
            {
                m_ButtonImage.sprite = ImageHover;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (TriggerEvent != null)
            {
                TriggerEvent(Exit, gameObject, eventData.pointerCurrentRaycast);
            }

            if (ImageNormal != null && m_ButtonImage != null)
            {
                m_ButtonImage.sprite = ImageNormal;
            }
        }

        // Get onhover by NRMultScrPointerRaycaster
        public void OnHover(RaycastResult racastResult)
        {
            if (TriggerEvent != null && m_ButtonImage != null)
            {
                TriggerEvent(Hover, gameObject, racastResult);
            }
        }
    }
}
