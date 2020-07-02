using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MoverioUnityPlugin : MonoBehaviour {

	private const int VECTOR_SIZE = 3;
	private const int QUATERNION_SIZE = 4;

	private const int SHAKE_COUNT = 3;
	private const int SHAKE_DURATION = 20;
	private const int SHAKE_INTERVAL = 750;

	private AndroidJavaClass moverioUnityPlugin;
	private AndroidJavaObject androidJavaObject;

	private bool setSensorListenerState;

	public event Action<float[]> SensorData_Controller_Accelerometer = delegate {};
	public event Action<float[]> SensorData_Controller_MagneticField = delegate {};
	public event Action<float[]> SensorData_Controller_Gyroscope = delegate {};
	public event Action<float[]> SensorData_Controller_RotationVector = delegate {};
	public event Action<Quaternion> SensorData_Controller_Orientation = delegate {};

	public event Action<float> SensorData_Controller_Shake = delegate {};

	public event Action SensorData_HeadSet_Tap = delegate {};

	private float[] shakeStorage = new float[SHAKE_COUNT];
	private bool shakeState;
	private int shakeCount;
	private System.DateTime shakeLastTime;

	public enum DisplayMode
	{
		DisplayMode2D = 0,
		DisplayMode3D
	}
		
	void Awake(){

		#if UNITY_ANDROID && !UNITY_EDITOR

		androidJavaObject = new AndroidJavaClass("co.jp.systemfriend.CMoverioUnityPlugin");
		androidJavaObject.CallStatic ("Init",gameObject.name);
		setSensorListenerState = true;

		#endif

	}
		
	void Start () {

	}

	void Update () {

	}

	void OnApplicationPause (bool pauseStatus){

		#if UNITY_ANDROID && !UNITY_EDITOR

		if(pauseStatus && setSensorListenerState){
			androidJavaObject.CallStatic ("UnSetSensorListener");
			setSensorListenerState = false;
		}

		if(!pauseStatus && !setSensorListenerState){
			androidJavaObject.CallStatic ("SetSensorListener");
			setSensorListenerState = true;
		}

		#endif

	}

	void OnDisable(){

		#if UNITY_ANDROID && !UNITY_EDITOR

		if(setSensorListenerState){
			androidJavaObject.CallStatic ("UnSetSensorListener");
		}

		#endif

	}

	public DisplayMode GetDisplayMode(){

		return (DisplayMode)androidJavaObject.CallStatic<int> ("GetDisplayMode");
	}

	public void SetDisplayMode(DisplayMode displayMode){

		androidJavaObject.CallStatic("SetDisplayMode",(int)displayMode);
	}

	public int GetDisplayBrightness(){

		return androidJavaObject.CallStatic<int> ("GetDisplayBrightness");
	}

	public void SetDisplayBrightness(int brightness){

		androidJavaObject.CallStatic("SetDisplayBrightness",brightness);
	}

	public bool GetMuteDisplay(){

		return androidJavaObject.CallStatic<bool> ("GetMuteDisplay");
	}

	public void SetMuteDisplay(bool state){

		androidJavaObject.CallStatic("SetMuteDisplay",state);
	}

	public void OnSensorData_Controller_Accelerometer(string message){

		SensorData_Controller_Accelerometer (ConversionStringToFloats(message));

		OnSensorData_Controller_Shake (ConversionStringToFloats(message));

	}

	public void OnSensorData_Controller_MagneticField(string message){

		SensorData_Controller_MagneticField (ConversionStringToFloats(message));

	}

	public void OnSensorData_Controller_Gyroscope(string message){

		SensorData_Controller_Gyroscope (ConversionStringToFloats(message));

	}

	public void OnSensorData_Controller_RotationVector(string message){

		SensorData_Controller_RotationVector (ConversionStringToFloats(message));

	}

	public void OnSensorData_Controller_Orientation(string message){

		OnSensorData_Controller_Orientation (ConversionStringToFloats (message,QUATERNION_SIZE));

	}

	public void OnSensorData_HeadSet_Tap(string message){

		SensorData_HeadSet_Tap ();
	}
		
	///------------------------------------------------------------------------------------------------

	private void OnSensorData_Controller_Shake(float[] accelerometer){

		System.DateTime nowTime = System.DateTime.Now;

		if(shakeLastTime == System.DateTime.MinValue){
			shakeLastTime = nowTime;
		}

		if (!shakeState) {

			double vectorLength = Math.Sqrt (Math.Pow (accelerometer [0], 2) + Math.Pow (accelerometer [1], 2) + Math.Pow (accelerometer [2], 2));

			if (vectorLength > SHAKE_DURATION) {

				shakeStorage [shakeCount] = (float)vectorLength;
				shakeCount++;

				if (shakeCount >= SHAKE_COUNT) {
					SensorData_Controller_Shake (Mathf.Max (shakeStorage));
					shakeLastTime = nowTime;
					shakeState = true;
					shakeCount = 0;

				}
				
			} else {
				shakeCount = 0;
			}

		} else {

			if((nowTime - shakeLastTime).Milliseconds > SHAKE_INTERVAL){
				shakeState = false;
			}
		}

	}

	private void OnSensorData_Controller_Orientation(float[] index){

		float X = index [0];
		float Y = index [1];
		float Z = index [2];
		float W = index [3];

		Quaternion quaternion = new Quaternion (X,-Z,Y,W);
		SensorData_Controller_Orientation(quaternion);

	}

	///------------------------------------------------------------------------------------------------

	private float[] ConversionStringToFloats(string message){

		float [] values = new float[VECTOR_SIZE];

		string[] punctuationMessage = message.Split (',');

		for (int i=0; i < values.Length; i++) {
			values [i] = float.Parse (punctuationMessage [i]);
		}

		return values;

	}
		
	private float[] ConversionStringToFloats(string message,int size){

		float [] values = new float[size];

		string[] punctuationMessage = message.Split (',');

		for (int i=0; i < values.Length; i++) {
			values [i] = float.Parse (punctuationMessage [i]);
		}

		return values;

	}

}
