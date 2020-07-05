using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeadSetTracking : MonoBehaviour {

	private Camera cam;
	private Vector3 center;
	private Image pointer;
	private float raycastDistance;
	[SerializeField] private bool debug = true;

	
	public Window hoveredWindow;
	public Icon hoveredIcon;
	public InteraciveButton targetButton;

	void Start () {
		
		//Input.gyro.enabled = true;
		cam = Camera.main;
		pointer = GameObject.Find("PointerImage").GetComponent<Image>();
		raycastDistance = 4000.0f;
	}

	void Update () {
		
		if(!debug) {
			this.gameObject.transform.rotation = Quaternion.AngleAxis (90.0f, Vector3.right) * Input.gyro.attitude * Quaternion.AngleAxis (180.0f, Vector3.forward);
		}

		center = new Vector2(cam.pixelWidth/2, cam.pixelHeight/2);

		Ray ray = cam.ScreenPointToRay(center);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, raycastDistance))
		{	
			GameObject target = hit.collider.transform.parent.gameObject;
			if(target.GetComponent<Icon>()) {
				hoveredIcon = target.GetComponent<Icon>(); 
				hoveredWindow = hoveredIcon.parent;
				pointer.color = new Color(0f,1f,0f);
				hoveredIcon.OnHovered();
			} else if(target.GetComponent<Window>()) {
				if(hoveredIcon != null) {
					hoveredIcon.OnLeaved();
					hoveredIcon = null;
				}
				hoveredWindow = target.GetComponent<Window>(); 
				pointer.color = new Color(0f,1f,0f);
			} 
		}
		else
		{
			if(hoveredIcon != null) 
				hoveredIcon.OnLeaved();
			hoveredIcon = null;
			hoveredWindow = null;
			pointer.color = new Color(1f, 0f, 0f);
		}


		/*
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        //将点击位置的屏幕坐标赋值给点击事件
        eventDataCurrentPosition.position = center;
		GraphicRaycaster uiRaycaster = GetComponent<GraphicRaycaster>();
		List<RaycastResult> results = new List<RaycastResult>();
        // GraphicRaycaster 发射射线
        uiRaycaster.Raycast(eventDataCurrentPosition, results);
        print(results.Count > 0);
		*/
		
	}


}
