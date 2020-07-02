using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {

		if(collision.collider.name.Contains("Beam") || collision.collider.name.Contains("Hammer")){

			Animator animator = this.gameObject.GetComponentInParent<Animator> ();
			animator.SetTrigger ("Hit");

		}
	}

}
