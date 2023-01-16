using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Attention : MonoBehaviour
{
	[SerializeField] private float slotDistance;
	private GameObject windowSlot;

	[Header("Interaction")]
	public Window hoveredWindow;
	public Icon hoveredIcon;
	public InteractiveSlider hoveredSlider;
	public Vector3 hitPosition;
	private Window lastHoveredWindow;

	[Header("Pointer")]
	public Pointer pointer;

	private string mode_to_send = "idle";
	public AndroidClient client;


	void Start () {

		
		windowSlot = GameObject.Find("WindowSlot");
		hitPosition = Vector3.zero;
	}

	void FindMode()
    {
		return;
		print(mode_to_send);
		if(hoveredWindow == null)
        {
			client.Write("0");
        } else
        {
			if (hoveredWindow is VideoPlayerWindow)
			{
				client.Write("5");
			}
			else if (hoveredWindow is PhotoLibraryWindow)
			{
				client.Write("2");
			}
			else if (hoveredWindow is ContactsWindow)
			{
				client.Write("3");
			}
			else if (hoveredWindow is VoiceWindow)
			{
				client.Write("4");
			}
			else
			{
				client.Write("1");
			}
        }

		lastHoveredWindow = hoveredWindow;
    } 

	void Update () {
		FindMode();
		pointer = GameObject.FindObjectOfType<Pointer>();
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
				if (pointer != null)
				{
					pointer.Hit();
				}
				hoveredIcon.Hover();
			} else if(target.GetComponent<Window>()) {
				if(hoveredIcon != null) {
					hoveredIcon.Leave();
					hoveredIcon = null;
					if (pointer != null)
					{
						pointer.Normal();
					}
				}
				hoveredWindow = target.GetComponent<Window>(); 
			} 

			if(hit.collider.gameObject.GetComponent<InteractiveSlider>())
            {
				hoveredSlider = hit.collider.gameObject.GetComponent<InteractiveSlider>();
			} else 
            {
				hoveredSlider = null;
			}

			if(hoveredSlider != null || hoveredIcon != null)
            {
				if (pointer != null)
				{
					pointer.Hit();
				}
			} else
            {
				if (pointer != null)
				{
					pointer.Normal();
				}
			}
		}
		else
		{
			if(hoveredIcon != null) 
				hoveredIcon.Leave();
			hoveredIcon = null;
			hoveredWindow = null;
			hoveredSlider = null;
			if (pointer != null)
            {
				pointer.Normal();
			}
		}
		
	}

	public Vector3 GetSlotPosition() {
		return windowSlot.transform.position;
	}
}
