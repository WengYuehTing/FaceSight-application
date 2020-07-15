using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Attention : MonoBehaviour
{
	[SerializeField] private float slotDistance;
	private Image pointer;
	private GameObject windowSlot; 

	public Window hoveredWindow;
	public Icon hoveredIcon;

	void Start () {
		pointer = GameObject.Find("Pointer").GetComponent<Image>();
		windowSlot = GameObject.Find("WindowSlot");
	}

	void Update () {
		windowSlot.transform.position = transform.forward * slotDistance;

		var center = new Vector2(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2);
		Ray ray = Camera.main.ScreenPointToRay(center);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 4000.0f))
		{	
			GameObject target = hit.collider.transform.parent.gameObject;
			if(target.GetComponent<Icon>()) {
				if(hoveredIcon != null) {
					hoveredIcon.Leave();
					hoveredIcon = null;
				}
				hoveredIcon = target.GetComponent<Icon>(); 
				hoveredWindow = hoveredIcon.parent;
				pointer.color = new Color(0f,1f,0f);
				hoveredIcon.Hover();
			} else if(target.GetComponent<Window>()) {
				if(hoveredIcon != null) {
					hoveredIcon.Leave();
					hoveredIcon = null;
					pointer.color = new Color(1f, 0f, 0f);
				}
				hoveredWindow = target.GetComponent<Window>(); 
			} 
		}
		else
		{
			if(hoveredIcon != null) 
				hoveredIcon.Leave();
			hoveredIcon = null;
			hoveredWindow = null;
			pointer.color = new Color(1f, 0f, 0f);
		}
		
	}

	public Vector3 GetSlotPosition() {
		return windowSlot.transform.position;
	}
}
