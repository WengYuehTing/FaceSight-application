using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerTracking : MonoBehaviour {

	public MoverioUnityPlugin moverioUnityPlugin;

	void Start () {

		moverioUnityPlugin.SensorData_Controller_Orientation += SensorData_Controller_Orientation;

	}

	void OnDisable(){

		moverioUnityPlugin.SensorData_Controller_Orientation -= SensorData_Controller_Orientation;

	}
	
	// Update is called once per frame
	void Update () {

	}

	private void SensorData_Controller_Orientation(Quaternion quaternion){

		//Rotation Direct Value
		//this.gameObject.transform.localRotation = Quaternion.Slerp(this.gameObject.transform.localRotation,quaternion,Time.deltaTime);

		Vector3 localEulerAngles = new Vector3 (this.gameObject.transform.localEulerAngles.x, this.gameObject.transform.localEulerAngles.y, this.gameObject.transform.localEulerAngles.z);

		this.gameObject.transform.localRotation = quaternion;

		Vector3 vector3 = new Vector3 (localEulerAngles.x, 0, localEulerAngles.z);

		if ((this.gameObject.transform.localEulerAngles.x > 0 && this.gameObject.transform.localEulerAngles.x <= 70) ||
			(this.gameObject.transform.localEulerAngles.x > 290 && this.gameObject.transform.localEulerAngles.x <= 360)) {
			vector3 = new Vector3 (-this.gameObject.transform.localEulerAngles.x,vector3.y,vector3.z);
		}

		if ((this.gameObject.transform.localEulerAngles.z > 0 && this.gameObject.transform.localEulerAngles.z <= 45) ||
			(this.gameObject.transform.localEulerAngles.z > 315 && this.gameObject.transform.localEulerAngles.z <= 360)) {
			vector3 = new Vector3 (vector3.x,vector3.y,-this.gameObject.transform.localEulerAngles.z);
		}

		this.gameObject.transform.localEulerAngles = vector3;

	}

}
