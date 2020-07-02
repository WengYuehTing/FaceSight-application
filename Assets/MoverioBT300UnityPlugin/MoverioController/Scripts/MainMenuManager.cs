using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GetSetSampleButton(){

		SceneManager.LoadScene ("GetSetSample");

	}

	public void DeviceControlSampleButton(){

		SceneManager.LoadScene ("DeviceControlSample");

	}
}
