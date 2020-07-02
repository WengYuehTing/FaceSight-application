using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour {

	private UnityEvent enter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Setting(UnityAction action){

		if(enter == null){
			enter = new UnityEvent();
		}

		enter.AddListener (action);

	}

	void OnCollisionEnter(Collision collision) {

		if(collision.collider.name.Contains("WalkPlayer")){

			enter.Invoke ();

		}
	}

}
