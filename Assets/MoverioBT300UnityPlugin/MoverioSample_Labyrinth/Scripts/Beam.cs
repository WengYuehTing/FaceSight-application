using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour {

	public GameObject collisionPrefab;

	void Start () {
		
		StartCoroutine (ActiveLimit ());

	}

	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {

		Instantiate (collisionPrefab, this.gameObject.transform.position, Quaternion.identity);
		Destroy (this.gameObject);

	}

	IEnumerator ActiveLimit(){

		yield return new WaitForSeconds (5.0f);

		Destroy (this.gameObject);

	}
}
