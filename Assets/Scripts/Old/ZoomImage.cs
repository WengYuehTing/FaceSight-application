using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ZoomImage : MonoBehaviour, IScrollHandler {

	private Vector3 initialScale;


    public float zoomSpeed = 5;
    [SerializeField] private float maxZoom = 10.0f;
    [SerializeField] private float minZoom = 10.0f;

    public ScrollRect scrollRect;
	public RawImage rawImage;

	public bool isWizardStartZooming = false;

	private void Awake() {
		initialScale = transform.localScale;
	}


	// Use this for initialization
	void Start () {
		//scrollRect.normalizedPosition = new Vector2(0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q)) {
			if(isWizardStartZooming) {
				isWizardStartZooming = false;
			} else {
				isWizardStartZooming = true;
				zoomSpeed = Mathf.Abs(zoomSpeed);
			}
		}

		if(Input.GetKeyDown(KeyCode.W)) {
			if(isWizardStartZooming) {
				isWizardStartZooming = false;
			} else {
				isWizardStartZooming = true;
				zoomSpeed = -1 * Mathf.Abs(zoomSpeed);
			}
		}

		if(isWizardStartZooming) {
			zoom(zoomSpeed * Time.deltaTime);
		}

	}

	public void zoom(float value) {
		Vector3 values = new Vector3(value, value, value);
		Vector3 desiredScale = rawImage.transform.localScale + values;
		float desiredValue = Mathf.Clamp(desiredScale.x, minZoom, maxZoom);
		desiredScale = new Vector3(desiredValue, desiredValue, 1);
		rawImage.transform.localScale = desiredScale;
	}

	public void OnScroll(PointerEventData eventData) {
	
		Vector3 delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
		Vector3 desiredScale = transform.localScale + delta;
		desiredScale = ClampDesiredScale(desiredScale);
		transform.localScale = desiredScale;
	}

	private Vector3 ClampDesiredScale(Vector3 scale) {
		scale = Vector3.Max(initialScale, scale);
		scale = Vector3.Min(initialScale*maxZoom, scale);
		return scale;
	}
}
