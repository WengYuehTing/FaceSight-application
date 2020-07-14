using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeadSetTracking : MonoBehaviour {

	[SerializeField] private bool debug = false;
	[SerializeField] private Vector2 rotAngle;
	[SerializeField] private float slotDistance;

	private Vector3 angle;
    private Vector3 tempAngle;
	private Image pointer;
	private GameObject windowSlot; 

	public Window hoveredWindow;
	public Icon hoveredIcon;

	void Start () {
		pointer = GameObject.Find("PointerImage").GetComponent<Image>();
		windowSlot = GameObject.Find("WindowSlot");
		angle = Vector3.zero;
		tempAngle = Vector3.zero;
		StartCoroutine(InitializeGyro());
	}

	void Update () {
		
		if(Input.isGyroAvailable) {
			this.gameObject.transform.rotation = Quaternion.AngleAxis (90.0f, Vector3.right) * Input.gyro.attitude * Quaternion.AngleAxis (180.0f, Vector3.forward);
		} else {
			angle.x = (Input.acceleration.y * rotAngle.y);
        	angle.y = (Input.acceleration.x * rotAngle.x);
        	tempAngle = Vector3.Slerp(tempAngle, angle, Time.deltaTime * 5);
        	transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(tempAngle), Time.deltaTime * 10);
		}
		

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

	void OnGUI()
    {
        //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        GUI.Label(new Rect(500, 300, 200, 40), "x:" + angle.x);
        GUI.Label(new Rect(500, 350, 200, 40), "y:" + angle.y);
        GUI.Label(new Rect(500, 400, 200, 40), "z:" + angle.z);
		GUI.Label(new Rect(500, 450, 200, 40), "gyro1:" + Input.isGyroAvailable);
		GUI.Label(new Rect(500, 500, 200, 40), "gyro2:" + Input.gyro.attitude.ToString());
    }

	IEnumerator InitializeGyro()
 	{
    	Input.gyro.enabled = true;
     	yield return null;
     	Debug.Log(Input.gyro.attitude); // attitude has data now
 	}
}
