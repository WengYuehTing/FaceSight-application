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

	[Header("Interaction")]
	public Window hoveredWindow;
	public Icon hoveredIcon;
	public InteractiveSlider hoveredSlider;
	public Vector3 hitPosition;

	void Start () {
		pointer = GameObject.Find("Pointer").GetComponent<Image>();
		windowSlot = GameObject.Find("WindowSlot");
		hitPosition = Vector3.zero;
	}

	void Update () {
		windowSlot.transform.position = transform.forward * slotDistance;

		var center = new Vector2(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2);
		Ray ray = Camera.main.ScreenPointToRay(center);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 4000.0f))
		{
			hitPosition = hit.point;
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

			if(hit.collider.gameObject.GetComponent<InteractiveSlider>())
            {
				hoveredSlider = hit.collider.gameObject.GetComponent<InteractiveSlider>();
				pointer.color = new Color(0f, 1f, 0f);
			} else 
            {
				hoveredSlider = null;
				pointer.color = new Color(1f, 0f, 0f);
			}
		}
		else
		{
			if(hoveredIcon != null) 
				hoveredIcon.Leave();
			hoveredIcon = null;
			hoveredWindow = null;
			hoveredSlider = null;
			pointer.color = new Color(1f, 0f, 0f);
		}
		
	}

	public Vector3 GetSlotPosition() {
		return windowSlot.transform.position;
	}
}
