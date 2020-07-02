using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSetWalkFilter : MonoBehaviour {

	public Transform head;
	public Rigidbody playerRigidbody;

	public float deltaDerivative = 0.05f;

	public float upLimit = -0.9f;
	public float downLimit = -1.1f;

	public float walkMinTime = 0.8f;

	public float accelerationThreshold = 20;

	public float speed=0.0025f;

	private float upFixedTime;
	private float downFixedTime;
	private float elapsedFixedTime;
	private float calculationAcceleration;
	private float lastAcceleration_Y;

	void Start () {

		upFixedTime = 1000;
		downFixedTime = 500;
	
	}

	void FixedUpdate (){

		float acceleration_Y = Input.acceleration.y;

		elapsedFixedTime += Time.fixedDeltaTime;

		if(elapsedFixedTime > deltaDerivative) {
			
			calculationAcceleration = Mathf.Abs((acceleration_Y-lastAcceleration_Y)/elapsedFixedTime);
			lastAcceleration_Y=acceleration_Y;

			elapsedFixedTime=0;

		}

		if(acceleration_Y>upLimit){	
			
			upFixedTime = Time.fixedTime;

		}

		else if(Time.fixedTime-upFixedTime>walkMinTime){
			
			upFixedTime = 1000;

		}

		if(acceleration_Y<downLimit){
			
			downFixedTime = Time.fixedTime;

		}
		else if(Time.fixedTime-upFixedTime>walkMinTime){
			
			upFixedTime = 500;

		}

		if (Mathf.Abs (upFixedTime - downFixedTime) < walkMinTime && calculationAcceleration < accelerationThreshold) {

			var index = 1 / Mathf.Abs (upFixedTime - downFixedTime);

			if (index > 20) {

				index = 20;

			} 

			playerRigidbody.MovePosition(transform.position + head.transform.forward * index * speed + transform.up*0.01f);

		}

	}
}
