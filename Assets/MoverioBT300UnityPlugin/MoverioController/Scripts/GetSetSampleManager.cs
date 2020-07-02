using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetSetSampleManager : MonoBehaviour {

	public MoverioUnityPlugin moverioUnityPlugin;

	public Text displayModeGetValue;

	public Text displayBrightnessGetValue;
	public InputField displayBrightnessSetValue;

	public Text muteDisplayGetValue;

	public Text Controller_Accelerometer_value;
	public Text Controller_MagneticField_value;
	public Text Controller_Gyroscope_value;
	public Text Controller_RotationVector_value;
	public Text HeadSet_Tap_Action;

	private int tapCount;

	// Use this for initialization
	void Start () {

		moverioUnityPlugin.SensorData_Controller_Accelerometer += SensorData_Controller_Accelerometer;
		moverioUnityPlugin.SensorData_Controller_MagneticField += SensorData_Controller_MagneticField;
		moverioUnityPlugin.SensorData_Controller_Gyroscope += SensorData_Controller_Gyroscope;
		moverioUnityPlugin.SensorData_Controller_RotationVector += SensorData_Controller_RotationVector;
		moverioUnityPlugin.SensorData_HeadSet_Tap += SensorData_HeadSet_Tap;

	}

	void OnDisable(){

		moverioUnityPlugin.SensorData_Controller_Accelerometer -= SensorData_Controller_Accelerometer;
		moverioUnityPlugin.SensorData_Controller_MagneticField -= SensorData_Controller_MagneticField;
		moverioUnityPlugin.SensorData_Controller_Gyroscope -= SensorData_Controller_Gyroscope;
		moverioUnityPlugin.SensorData_Controller_RotationVector -= SensorData_Controller_RotationVector;
		moverioUnityPlugin.SensorData_HeadSet_Tap -= SensorData_HeadSet_Tap;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BackSceneButton(){

		SceneManager.LoadScene ("MainMenu");

	}

	public void GetDisplayMode(){

		displayModeGetValue.text = moverioUnityPlugin.GetDisplayMode ().ToString();

	}

	public void SetDisplayMode2D(){

		moverioUnityPlugin.SetDisplayMode (MoverioUnityPlugin.DisplayMode.DisplayMode2D);
	}

	public void SetDisplayMode3D(){

		moverioUnityPlugin.SetDisplayMode (MoverioUnityPlugin.DisplayMode.DisplayMode3D);
	}

	public void GetDisplayBrightness(){

		displayBrightnessGetValue.text = moverioUnityPlugin.GetDisplayBrightness ().ToString();

	}

	public void SetDisplayBrightness(){

		int brightnessIndex = 0;

		if (int.TryParse (displayBrightnessSetValue.text,out brightnessIndex)) {
			moverioUnityPlugin.SetDisplayBrightness (brightnessIndex);
		} 

	}

	public void GetMuteDisplay(){

		muteDisplayGetValue.text = moverioUnityPlugin.GetMuteDisplay ().ToString();

	}

	public void SetMuteDisplay(){

		StartCoroutine (ChangeMuteDisplay());

	}

	IEnumerator ChangeMuteDisplay(){

		moverioUnityPlugin.SetMuteDisplay (true);

		yield return new WaitForSeconds (3.0f);

		moverioUnityPlugin.SetMuteDisplay (false);

	}
		
	private void SensorData_Controller_Accelerometer(float[] values){

		Controller_Accelerometer_value.text ="X:"+ values [0] + " Y:" + values [1] + " Z:" + values [2];

	}

	private void SensorData_Controller_MagneticField(float[] values){

		Controller_MagneticField_value.text ="X:"+ values [0] + " Y:" + values [1] + " Z:" + values [2];

	}

	private void SensorData_Controller_Gyroscope(float[] values){

		Controller_Gyroscope_value.text ="X:"+ values [0] + " Y:" + values [1] + " Z:" + values [2];

	}

	private void SensorData_Controller_RotationVector(float[] values){

		Controller_RotationVector_value.text ="X:"+ values [0] + " Y:" + values [1] + " Z:" + values [2];

	}

	private void SensorData_HeadSet_Tap(){

		tapCount++;

		HeadSet_Tap_Action.text = "TapCount:" + tapCount.ToString();

	}


}
