using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeviceControlSampleManager : MonoBehaviour {

	public MoverioUnityPlugin moverioUnityPlugin;

	public Text shakeSpeedText;

	public GameObject controller;

	void Start () {

		Input.gyro.enabled = true;
		
		moverioUnityPlugin.SensorData_Controller_Shake += SensorData_Controller_Shake;

	}

	void OnDisable(){
		
		moverioUnityPlugin.SensorData_Controller_Shake -= SensorData_Controller_Shake;

	}
		
	void Update () {

	}

	public void BackSceneButton(){

		SceneManager.LoadScene ("MainMenu");

	}

	private void SensorData_Controller_Shake(float speed){

		shakeSpeedText.text = speed.ToString();

	}


}
