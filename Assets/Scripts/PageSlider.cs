using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PageSlider : MonoBehaviour {

	// Use this for initialization
	public ScrollRect scrollRect;
    public GameObject highlighted;
    public Image highlightedImage;
	public RawImage content;
	public RawImage loadingAnimation;
	public float slideSpeed = 0.5f;
	public float randomOffset = 0.2f;

	private bool isLoadingPage = false;
	private float loadingTime = 0.0f;

	private int pageIndex = 0;
	public float loadingDuration = 1.0f;

	public Texture[] textures;

	public bool isWizardStartSliding = false;
    public bool isWizardStartZooming = false;
    public float zoomSpeed = 5;
    [SerializeField] private float maxZoom = 10.0f;
    [SerializeField] private float minZoom = 10.0f;


    private bool shouldChangingPage = false;


    void Start () {
		scrollRect.horizontal = false;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isLoadingPage) {
			loadingTime += Time.deltaTime;
			
			if(loadingTime >= loadingDuration) {
				onFinishLoadPage();
			}
		}

		BindKeyBoardInput();

		if(isWizardStartSliding) {
			float val = Random.Range(slideSpeed - randomOffset, slideSpeed + randomOffset);
			AdjustSlider(val * Time.deltaTime);
		}

        if (isWizardStartZooming)
        {
            zoom(zoomSpeed * Time.deltaTime);
        }
    }

	private void onFinishLoadPage() {
		if(shouldChangingPage) {
			if(pageIndex == 0) {
				pageIndex = 1;
				content.transform.localScale = new Vector3(content.transform.localScale.x, content.transform.localScale.y * 0.4f, content.transform.localScale.z);
			} else {
				pageIndex = 0;
				content.transform.localScale = new Vector3(content.transform.localScale.x, content.transform.localScale.y * 2.5f, content.transform.localScale.z);
			}
		}
		

		content.texture = textures[pageIndex];
		isLoadingPage = false;
		loadingTime = 0.0f;
		loadingAnimation.enabled = false;
		shouldChangingPage = false;
		scrollRect.normalizedPosition = new Vector2(0, 1);
	}

	public void LoadPage(bool change) {
		content.texture = null;;
		isLoadingPage = true;
		loadingAnimation.enabled = true;
		shouldChangingPage = change;
	}

	//0是底
	//1是頂
	public void AdjustSlider(float value) {
		float valueToSet = Mathf.Clamp(scrollRect.normalizedPosition.y + value, 0, 1);
		scrollRect.normalizedPosition = new Vector2(0, valueToSet);
	}

	public void Nav() {
		if(isLoadingPage) { return; }
		LoadPage(true);
	}

	public void PreviousPage() {
		if(isLoadingPage) { return; }
		LoadPage(true);
	}

	public void NextPage() {
		if(isLoadingPage) { return; }
		LoadPage(true);
	}

	public void Refresh() {
		if(isLoadingPage) { return; }
		LoadPage(false);
	}

    public void zoom(float value)
    {
        Vector3 values = new Vector3(value, value, value);
        Vector3 desiredScale = highlightedImage.transform.localScale + values;
        float desiredValue = Mathf.Clamp(desiredScale.x, minZoom, maxZoom);
        desiredScale = new Vector3(desiredValue, desiredValue, 1);
        highlightedImage.transform.localScale = desiredScale;
    }

    private void BindKeyBoardInput() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			if(isWizardStartSliding) {
				isWizardStartSliding = false;
			} else {
				isWizardStartSliding = true;
				slideSpeed = Mathf.Abs(slideSpeed);
			}
		
		}

		if(Input.GetKeyDown(KeyCode.A)) {
			if(isWizardStartSliding) {
				isWizardStartSliding = false;
			} else {
				isWizardStartSliding = true;
				slideSpeed = -1 * Mathf.Abs(slideSpeed);
			}
		}

		if(Input.GetKeyDown(KeyCode.W)) {
            highlighted.SetActive(!highlighted.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isWizardStartZooming)
            {
                isWizardStartZooming = false;
            }
            else
            {
                isWizardStartZooming = true;
                zoomSpeed = Mathf.Abs(zoomSpeed);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (isWizardStartZooming)
            {
                isWizardStartZooming = false;
            }
            else
            {
                isWizardStartZooming = true;
                zoomSpeed = -1 * Mathf.Abs(zoomSpeed);
            }
        }
    }
}
