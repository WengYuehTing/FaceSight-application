using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	public GameObject bullet;
	public float speed = 1000;

	void Start () {

	}

	void Update () {
		/*if(Input.GetKeyDown (KeyCode.Z)){
			Fire ();
		}*/
	}

	public void Fire(){

		GameObject bullets = GameObject.Instantiate(bullet)as GameObject;

		Vector3 force;
		force = this.gameObject.transform.forward * speed;
		bullets.GetComponent<Rigidbody>().AddForce (force);
		bullets.transform.position = this.gameObject.transform.position;

	}
}
