using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Signboard : MonoBehaviour {

	public MeshRenderer individualMeshRenderer;

	void Start () {
		
	}

	void Update () {
		
	}

	public void Setting(Material material){

		individualMeshRenderer.material = material;

	}

}
