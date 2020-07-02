using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	private ParticleSystem collisionParticleSystem;

	void Start () {

		collisionParticleSystem = this.gameObject.GetComponentInChildren<ParticleSystem> ();

	}

	void Update () {

		if(!collisionParticleSystem.IsAlive()){
			Destroy (this.gameObject);
		}

	}
}
