using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour {

	private int flag;
	private IntEvent sendFlag;

	public MeshRenderer individualMeshRenderer;

	public class IntEvent : UnityEvent<int>
	{
	}
		
	void Start () {
		
	}

	void Update () {
		
	}

	public void Setting(int index,UnityAction<int> action,Material material){

		flag = index;

		if(sendFlag == null){
			sendFlag = new IntEvent();
		}

		sendFlag.AddListener (action);

		individualMeshRenderer.material = material;

	}

	public void AnimationEnd(){

		sendFlag.Invoke (flag);

	}

}
