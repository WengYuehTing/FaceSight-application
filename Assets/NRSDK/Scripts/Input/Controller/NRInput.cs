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
    using System.Runtime.InteropServices;

    /// <summary>
    /// Enumeration of handedness
    /// </summary>
    public enum ControllerHandEnum
    {
        Right = 0,
        Left = 1
    }

    /// <summary>
    /// Enumeration of raycast mode. Normally, suggest using "Laser" mode.
    /// </summary>
    public enum RaycastModeEnum
    {
        Gaze,
        Laser
    }

    /// <summary>
    /// Enumeration of controller visual types.
    /// </summary>
    public enum ControllerVisualType
    {
        None = 0,
        NrealLight = 1,
        Phone = 2,
        FinchShift
    }

    /// <summary>
    /// The main class to handle controller related things, such as to get controller states, update controller states
    /// Through this class, application would create a controllerProvider which could be custom, then the controllerProvider
    /// iteself would define how to update the controller states, so that every frame NRInput could get the right states.There
    /// are max two states for one controllerProvider.
    /// </summary>
    [HelpURL("https://developer.nreal.ai/develop/unity/controller")]
    public partial class NRInput : SingletonBehaviour<NRInput>
    {
        [Tooltip("If enable this, phone virtual controller would be shown in Unity Editor")]
        [SerializeField]
        private bool m_EmulateVirtualDisplayInEditor;
        [SerializeField]
        private Transform m_OverrideCameraCenter;
        [SerializeField]
        private ControllerAnchorsHelper m_AnchorHelper;
        [SerializeField]
        private RaycastModeEnum m_RaycastMode = RaycastModeEnum.Laser;
        [SerializeField]
        private float m_ClickInterval = 0.3f;
        [SerializeField]
        private float m_DragThreshold = 0.02f;
        private ControllerVisualManager m_VisualManager;
        private int m_LastControllerCount;
        private bool m_ReticleVisualActive = true;
        private bool m_LaserVisualActive = true;
        private bool m_ControllerVisualActive = true;
        private bool m_HapticVibrationEnabled = true;

        private static NRInput m_Instance = null;
        private static ControllerHandEnum m_DomainHand = ControllerHandEnum.Right;
        private static ControllerProviderBase m_ControllerProvider;
        private static ControllerState[] m_States = new ControllerState[MAX_CONTROLLER_STATE_COUNT]
        {
            new ControllerState(),
            new ControllerState()
        };

        /// <summary>
        /// Max count of controllerstates supported per frame
        /// </summary>
        public const int MAX_CONTROLLER_STATE_COUNT = 2;

        /// <summary>
        /// Event invoked whenever the domain hand has changed.
        /// </summary>
        public static Action<ControllerHandEnum> OnDomainHandChanged;

        /// <summary>
        /// Event invoked whenever a controller device is connected.
        /// </summary>
        public static Action OnControllerConnected;

        /// <summary>
        /// Event invoked whenever a controller device is disconnected.
        /// </summary>
        public static Action OnControllerDisconnected;

        /// <summary>
        /// Event invoked before controller devices are going to recenter.
        /// </summary>
        public static Action OnBeforeControllerRecenter;

        /// <summary>
        /// Event invoked whenever controller devices are recentering.
        /// </summary>
        public static Action OnControllerRecentering;

        /// <summary>
        /// Event invoked whenever controller devices are recentered.
        /// </summary>
        public static Action OnControllerRecentered;

        /// <summary>
        /// Event invoked whenever controller devices states are updated
        /// </summary>
        public static Action OnControllerStatesUpdated;

        /// <summary>
        /// Determine whether to show reticle visuals, could be get and set at runtime.
        /// </summary>
        public static bool ReticleVisualActive { get { return Instance.m_ReticleVisualActive; } set { Instance.m_ReticleVisualActive = value; } }

        /// <summary>
        /// Determine whether to show laser visuals, could be get and set at runtime.
        /// </summary>
        public static bool LaserVisualActive { get { return Instance.m_LaserVisualActive; } set { Instance.m_LaserVisualActive = value; } }

        /// <summary>
        /// Determine whether to show controller visuals, could be get and set at runtime.
        /// </summary>
        public static bool ControllerVisualActive { get { return Instance.m_ControllerVisualActive; } set { Instance.m_ControllerVisualActive = value; } }

        /// <summary>
        /// Determine whether enable haptic vibration.
        /// </summary>
        public static bool HapticVibrationEnabled { get { return Instance.m_HapticVibrationEnabled; } set { Instance.m_HapticVibrationEnabled = value; } }

        /// <summary>
        /// Determine whether emulate phone virtual display in Unity Editor
        /// </summary>
        public static bool EmulateVirtualDisplayInEditor { get { return Instance ? Instance.m_EmulateVirtualDisplayInEditor : false; } }

        /// <summary>
        /// It's a helper to get controller anchors which are frequently used.
        /// </summary>
        public static ControllerAnchorsHelper AnchorsHelper { get { return Instance.m_AnchorHelper; } }

        /// <summary>
        /// Get the current enumeration of handedness.
        /// </summary>
        public static ControllerHandEnum DomainHand { get { return m_DomainHand; } }

        /// <summary>
        /// Determine which raycast mode to use.
        /// </summary>
        public static RaycastModeEnum RaycastMode { get { return Instance.m_RaycastMode; } set { Instance.m_RaycastMode = value; } }

        /// <summary>
        /// Get and set button click interval
        /// </summary>
        public static float ClickInterval { get { return Instance.m_ClickInterval; } set { Instance.m_ClickInterval = value; } }

        /// <summary>
        /// Get and set pointer drag threshold
        /// </summary>
        public static float DragThreshold { get { return Instance.m_DragThreshold; } set { Instance.m_DragThreshold = value; } }

        /// <summary>
        /// Get the transform of the camera which controllers are following.
        /// </summary>
        public static Transform CameraCenter { get { return Instance.GetCameraCenter(); } }

        private void Start()
        {
            if (isDirty)
            {
                return;
            }
            //Should not be inited at Awake, because that default controller provider may be changed at Awake
            Init();
#if UNITY_EDITOR
            // For Emulator Init
            InitEmulator();
#endif
        }

        private void Update()
        {
            if (m_ControllerProvider == null)
                return;
            UpdateControllerProvider();
        }

        private void UpdateControllerProvider()
        {
            if (m_ControllerProvider.Inited)
            {
                m_ControllerProvider.Update();
                if (OnControllerStatesUpdated != null)
                    OnControllerStatesUpdated();
                CheckControllerConnection();
                CheckControllerRecentered();
                CheckControllerButtonEvents();
            }
            else
            {
                m_ControllerProvider.Update();
            }
        }

        private void OnEnable()
        {
            if (isDirty)
            {
                return;
            }
            if (m_ControllerProvider != null)
                m_ControllerProvider.OnResume();
        }

        private void OnDisable()
        {
            if (isDirty)
            {
                return;
            }
            if (m_ControllerProvider != null)
                m_ControllerProvider.OnPause();
        }

        public string GetVersion(int index)
        {
            if (m_ControllerProvider is NRControllerProvider)
            {
                return ((NRControllerProvider)m_ControllerProvider).GetVersion(index);
            }
            else
            {
                return "0.0.0";
            }
        }

        public void Destroy()
        {
            if (m_ControllerProvider != null)
            {
                m_ControllerProvider.OnDestroy();
                m_ControllerProvider = null;
            }
        }

        new void OnDestroy()
        {
            if (isDirty)
            {
                return;
            }
            base.OnDestroy();
            Destroy();
        }

        private void CheckControllerConnection()
        {
            int currentControllerCount = GetAvailableControllersCount();
            if (m_LastControllerCount < currentControllerCount)
            {
                if (OnControllerConnected != null)
                    OnControllerConnected();
            }
            else if (m_LastControllerCount > currentControllerCount)
            {
                if (OnControllerDisconnected != null)
                    OnControllerDisconnected();
            }
            m_LastControllerCount = currentControllerCount;
        }

        private void CheckControllerRecentered()
        {
            if (GetControllerState(DomainHand).recentered)
            {
                if (OnBeforeControllerRecenter != null)
                    OnBeforeControllerRecenter();
                if (OnControllerRecentering != null)
                    OnControllerRecentering();
                if (OnControllerRecentered != null)
                    OnControllerRecentered();
            }
        }

        private void CheckControllerButtonEvents()
        {
            int currentControllerCount = GetAvailableControllersCount();
            for (int i = 0; i < currentControllerCount; i++)
            {
                m_States[i].CheckButtonEvents();
            }
        }

        private void OnApplicationPause(bool paused)
        {
            if (m_ControllerProvider == null || !m_ControllerProvider.Inited)
                return;
            if (paused)
                m_ControllerProvider.OnPause();
            else
                m_ControllerProvider.OnResume();
        }

        private void Init()
        {
            NRDevice.Instance.Init();
            m_VisualManager = gameObject.AddComponent<ControllerVisualManager>();
            m_VisualManager.Init(m_States);
            m_ControllerProvider = ControllerProviderFactory.CreateControllerProvider(m_States);
        }

        private void InitEmulator()
        {
            if (!NREmulatorManager.Inited && !GameObject.Find("NREmulatorManager"))
            {
                NREmulatorManager.Inited = true;
                GameObject.Instantiate(Resources.Load("Prefabs/NREmulatorManager"));
            }
            if (!GameObject.Find("NREmulatorController"))
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/NREmulatorController"));
            }
        }

        private Transform GetCameraCenter()
        {
            if (m_OverrideCameraCenter)
                return m_OverrideCameraCenter;
            if (Camera.main)
                return Camera.main.transform;
            return null;
        }

        private static int ConvertHandToIndex(ControllerHandEnum handEnum)
        {
            if (GetAvailableControllersCount() < 2)
                return DomainHand == handEnum ? 0 : 1;
            return (int)handEnum;
        }

        private static ControllerState GetControllerState(ControllerHandEnum hand)
        {
            return m_States[ConvertHandToIndex(hand)];
        }

        /// <summary>
        /// Set the current enumeration of handedness.
        /// </summary>
        /// <param name="handEnum"></param>
        public static void SetDomainHandMode(ControllerHandEnum handEnum)
        {
            if (m_DomainHand == handEnum)
                return;
            m_DomainHand = handEnum;
            if (OnDomainHandChanged != null)
                OnDomainHandChanged(m_DomainHand);
        }

        /// <summary>
        /// Get the current count of controllers which are connected and available
        /// </summary>
        /// <returns></returns>
        public static int GetAvailableControllersCount()
        {
            if (m_ControllerProvider == null)
                return 0;
            return m_ControllerProvider.ControllerCount;
        }

        /// <summary>
        /// Get the ControllerType of current controller
        /// </summary>
        /// <returns></returns>
        public static ControllerType GetControllerType()
        {
            return GetControllerState(DomainHand).controllerType;
        }

        /// <summary>
        /// Get the ConnectionState of current controller
        /// </summary>
        /// <returns></returns>
        public static ControllerConnectionState GetControllerConnectionState()
        {
            return GetControllerState(DomainHand).connectionState;
        }

        /// <summary>
        /// Returns true if the controller 
        /// </summary>
        public static bool CheckControllerAvailable(ControllerHandEnum handEnum)
        {
            int availableCount = GetAvailableControllersCount();
            if (availableCount == 2)
                return true;
            if (availableCount == 1)
                return handEnum == DomainHand;
            return false;
        }

        /// <summary>
        /// Returns true if the current controller supports the certain feature.
        /// </summary>
        public static bool GetControllerAvailableFeature(ControllerAvailableFeature feature)
        {
            if (GetAvailableControllersCount() == 0)
                return false;
            return GetControllerState(m_DomainHand).IsFeatureAvailable(feature);
        }

        /// <summary>
        /// Returns true if the button is currently pressed this frame.
        /// </summary>
        public static bool GetButton(ControllerButton button)
        {
            return GetButton(m_DomainHand, button);
        }

        /// <summary>
        /// Returns true if the button was pressed down this frame.
        /// </summary>
        public static bool GetButtonDown(ControllerButton button)
        {
            return GetButtonDown(m_DomainHand, button);
        }

        /// <summary>
        /// Returns true if the button was released this frame.
        /// </summary>
        public static bool GetButtonUp(ControllerButton button)
        {
            return GetButtonUp(m_DomainHand, button);
        }

        /// <summary>
        /// Returns a Vector2 touch position on touchpad of the domain controller, range: x(-1f ~ 1f), y(-1f ~ 1f);
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetTouch()
        {
            return GetTouch(m_DomainHand);
        }

        /// <summary>
        /// Returns a Vector2 delta touch value on touchpad of the domain controller
        /// </summary>
        public static Vector2 GetDeltaTouch()
        {
            return GetDeltaTouch(m_DomainHand);
        }

        /// <summary>
        /// Returns the current position of the domain controller if 6dof, otherwise returns Vector3.zero
        /// </summary>
        public static Vector3 GetPosition()
        {
            return GetPosition(m_DomainHand);
        }

        /// <summary>
        /// Returns the current rotation of the domain controller
        /// </summary>
        public static Quaternion GetRotation()
        {
            return GetRotation(m_DomainHand);
        }

        /// <summary>
        /// Returns the gyro sensor value of the domain controller
        /// </summary>
        public static Vector3 GetGyro()
        {
            return GetGyro(m_DomainHand);
        }

        /// <summary>
        /// Returns the accel sensor value of the domain controller
        /// </summary>
        public static Vector3 GetAccel()
        {
            return GetAccel(m_DomainHand);
        }

        /// <summary>
        /// Returns the magnetic sensor value of the domain controller
        /// </summary>
        public static Vector3 GetMag()
        {
            return GetMag(m_DomainHand);
        }

        /// <summary>
        /// Returns the battery level of the domain controller
        /// </summary>
        public static int GetControllerBattery()
        {
            return GetControllerBattery(DomainHand);
        }

        /// <summary>
        /// Trigger vibration of the domain controller
        /// </summary>
        public static void TriggerHapticVibration(float durationSeconds = 0.1f, float frequency = 200f, float amplitude = 0.8f)
        {
            TriggerHapticVibration(m_DomainHand, durationSeconds, frequency, amplitude);
        }

        /// <summary>
        /// Returns true if the button is currently pressed this frame on a certain handedness controller.
        /// </summary>
        public static bool GetButton(ControllerHandEnum hand, ControllerButton button)
        {
            return GetControllerState(hand).GetButton(button);
        }

        /// <summary>
        /// Returns true if the button was pressed down this frame on a certain handedness controller.
        /// </summary>
        public static bool GetButtonDown(ControllerHandEnum hand, ControllerButton button)
        {
            return GetControllerState(hand).GetButtonDown(button);
        }

        /// <summary>
        /// Returns true if the button was released this frame on a certain handedness controller.
        /// </summary>
        public static bool GetButtonUp(ControllerHandEnum hand, ControllerButton button)
        {
            return GetControllerState(hand).GetButtonUp(button);
        }

        /// <summary>
        /// Returns a Vector2 touch position on touchpad of a certain handedness controller, range: x(-1f ~ 1f), y(-1f ~ 1f).
        /// </summary>
        public static Vector2 GetTouch(ControllerHandEnum hand)
        {
            return GetControllerState(hand).touchPos;
        }

        /// <summary>
        /// Returns a Vector2 delta touch value on touchpad of a certain handedness controller
        /// </summary>
        public static Vector2 GetDeltaTouch(ControllerHandEnum hand)
        {
            return GetControllerState(hand).deltaTouch;
        }

        /// <summary>
        /// Returns the current position of a certain handedness controller if 6dof, otherwise returns Vector3.zero
        /// </summary>
        public static Vector3 GetPosition(ControllerHandEnum hand)
        {
            return GetControllerState(hand).position;
        }

        /// <summary>
        /// Returns the current rotation of a certain handedness controller
        /// </summary>
        public static Quaternion GetRotation(ControllerHandEnum hand)
        {
            return GetControllerState(hand).rotation;
        }

        /// <summary>
        /// Returns the gyro sensor value of a certain handedness controller
        /// </summary>
        public static Vector3 GetGyro(ControllerHandEnum hand)
        {
            return GetControllerState(hand).gyro;
        }

        /// <summary>
        /// Returns the accel sensor value of a certain handedness controller
        /// </summary>
        public static Vector3 GetAccel(ControllerHandEnum hand)
        {
            return GetControllerState(hand).accel;
        }

        /// <summary>
        /// Returns the magnetic sensor value of a certain handedness controller
        /// </summary>
        public static Vector3 GetMag(ControllerHandEnum hand)
        {
            return GetControllerState(hand).mag;
        }

        /// <summary>
        /// Returns the battery level of a certain handedness controller, range from 0 to 100
        /// </summary>
        public static int GetControllerBattery(ControllerHandEnum hand)
        {
            return GetControllerState(hand).batteryLevel;
        }

        /// <summary>
        /// Trigger vibration of a certain handedness controller
        /// </summary>
        public static void TriggerHapticVibration(ControllerHandEnum hand, float durationSeconds = 0.1f, float frequency = 200f, float amplitude = 0.8f)
        {
            if (!HapticVibrationEnabled)
                return;
            if (GetAvailableControllersCount() == 0)
                return;
            m_ControllerProvider.TriggerHapticVibration(ConvertHandToIndex(hand), durationSeconds, frequency, amplitude);
        }

        /// <summary>
        /// Add button down event listerner
        /// </summary>
        public static void AddDownListener(ControllerHandEnum hand, ControllerButton button, Action action)
        {
            GetControllerState(hand).AddButtonListener(ButtonEventType.Down, button, action);
        }

        /// <summary>
        /// Remove button down event listerner
        /// </summary>
        public static void RemoveDownListener(ControllerHandEnum hand, ControllerButton button, Action action)
        {
            GetControllerState(hand).RemoveButtonListener(ButtonEventType.Down, button, action);
        }

        /// <summary>
        /// Add button pressing event listerner
        /// </summary>
        public static void AddPressingListener(ControllerHandEnum hand, ControllerButton button, Action action)
        {
            GetControllerState(hand).AddButtonListener(ButtonEventType.Pressing, button, action);
        }

        /// <summary>
        /// Remove button pressing event listerner
        /// </summary>
        public static void RemovePressingListener(ControllerHandEnum hand, ControllerButton button, Action action)
        {
            GetControllerState(hand).RemoveButtonListener(ButtonEventType.Pressing, button, action);
        }

        /// <summary>
        /// Add button up event listerner
        /// </summary>
        public static void AddUpListener(ControllerHandEnum hand, ControllerButton button, Action action)
        {
            GetControllerState(hand).AddButtonListener(ButtonEventType.Up, button, action);
        }

        /// <summary>
        /// Remove button up event listerner
        /// </summary>
        public static void RemoveUpListener(ControllerHandEnum hand, ControllerButton button, Action action)
        {
            GetControllerState(hand).RemoveButtonListener(ButtonEventType.Up, button, action);
        }

        /// <summary>
        /// Add button click event listerner
        /// </summary>
        public static void AddClickListener(ControllerHandEnum hand, ControllerButton button, Action action)
        {
            GetControllerState(hand).AddButtonListener(ButtonEventType.Click, button, action);
        }

        /// <summary>
        /// Remove button click event listerner
        /// </summary>
        public static void RemoveClickListener(ControllerHandEnum hand, ControllerButton button, Action action)
        {
            GetControllerState(hand).RemoveButtonListener(ButtonEventType.Click, button, action);
        }
    }
}
