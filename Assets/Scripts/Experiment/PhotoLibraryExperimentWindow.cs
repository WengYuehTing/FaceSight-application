using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoLibraryExperimentWindow : Window
{
    protected ScrollRect scroller;

    [Header("Page")]
    [SerializeField] protected GameObject page1;
    [SerializeField] protected GameObject page2;
    [SerializeField] protected RawImage selectedImage;
    protected bool selected;

    [Header("Slider")]
    [SerializeField] protected GameObject slider;

    protected float sliderMax;
    protected float sliderMin;
    protected float zoomMax;
    protected float zoomMin;
    public float scrollValue = 1.0f;
    public float zoomValue = 1.2f;

    public bool zoomEnabled = false;

    public GameObject zoomingCube;
    public bool isCurrentZoomUp = true;

    protected override void Awake()
    {
        base.Awake();
        visibility = true;
        targetScale = new Vector3(1.2f, 1.2f, 1.0f);
        eulerAngleOffsets = new Vector3(0.0f, 180.0f, 0.0f);
        positionOffsets = new Vector3(0.0f, -2.0f, 0.0f);
#if UNITY_EDITOR
        positionOffsets = new Vector3(0.0f, 4.0f, 0.0f); // 
#endif
        selected = false;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Setup();
        zoomingCube = GameObject.Find("ZoomingCube");
    }

    private void Setup()
    {
        scroller = GetComponentInChildren<ScrollRect>();
        sliderMax = 0.2f;
        sliderMin = -0.4f;
        zoomMax = 1.8f;
        zoomMin = 1.2f;
        zoomValue = 1.2f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        selected = !page1.activeSelf;
        if (!selected)
        {
            slider.transform.localPosition = new Vector3(slider.transform.localPosition.x, Mathf.Clamp(GetSliderY(), sliderMin, sliderMax), slider.transform.localPosition.z);
        }
    }

    public override int OnTapped()
    {
        if (selected)
        {
            Cancel();
            return 1;
        }
        else
        {
            return 0;
        }
    }

    protected float GetSliderY()
    {
        return (sliderMax - sliderMin) * scroller.normalizedPosition.y + sliderMin;
    }

    public void Select(Texture texture)
    {
        if (selected) { return; }
        page1.SetActive(false);
        page2.SetActive(true);
        selectedImage.texture = texture;
    }

    public void Cancel()
    {
        if (!selected) { return; }
        page1.SetActive(true);
        page2.SetActive(false);
    }

    public void Scroll(float value)
    {
        if (zoomEnabled)
        {
            return;
        }

        if (!scroller)
        {
            scroller = GetComponentInChildren<ScrollRect>();
        }


        scrollValue = Mathf.Clamp(scrollValue + value, 0.0f, 1.0f);
        // float target = scrollValue + Mathf.Clamp(value, 0.0f, 1.0f);
        if (scrollValue >= 0.92f)
        {
            scroller.normalizedPosition = new Vector2(0, 1.0f);
        }
        else if (scrollValue >= 0.75f)
        {
            scroller.normalizedPosition = new Vector2(0, 0.83f);
        }
        else if (scrollValue >= 0.58f)
        {
            scroller.normalizedPosition = new Vector2(0, 0.66f);
        }
        else if (scrollValue >= 0.41f)
        {
            scroller.normalizedPosition = new Vector2(0, 0.49f);
        }
        else if (scrollValue >= 0.24f)
        {
            scroller.normalizedPosition = new Vector2(0, 0.32f);
        }
        else if (scrollValue >= 0.07f)
        {
            scroller.normalizedPosition = new Vector2(0, 0.15f);
        }
        else
        {
            scroller.normalizedPosition = new Vector2(0, 0.0f);
        }
        // scroller.normalizedPosition = new Vector2(0, target);
    }

    public float GetScrolledValue()
    {
        return Mathf.Clamp(scroller.normalizedPosition.y, 0.0f, 1.0f);
    }

    public float GetZoomValue()
    {
        return Mathf.Clamp(zoomValue, zoomMin, zoomMax);
    }

    public void Zoom(float value)
    {
        zoomValue = Mathf.Clamp(zoomValue + value, zoomMin, zoomMax);
        this.gameObject.transform.localScale = new Vector3(zoomValue, zoomValue, 1.0f);
        /*
        float target = selectedImage.transform.localScale.x + value;
        target = Mathf.Clamp(target, zoomMin, zoomMax);
        Vector3 desiredScale = new Vector3(target, target, 1.0f);
        selectedImage.transform.localScale = desiredScale;
        */
    }

    public void prepareZoom(float value)
    {
        zoomValue = Mathf.Clamp(zoomValue + value, zoomMin, zoomMax);
        this.gameObject.transform.localScale = new Vector3(zoomValue, zoomValue, 1.0f);
    }

    public void StartZoom()
    {
        zoomEnabled = true;
    }

    public void StopZoom()
    {
        zoomEnabled = false;
    }

    public void ShowCube(bool isZoomUp)
    {
        zoomingCube.transform.position = transform.position;
        zoomingCube.transform.eulerAngles = transform.eulerAngles;

        if (isZoomUp)
        {
            zoomingCube.transform.localScale = new Vector3(10.68f, 6.78f, 0.013f);
            zoomingCube.transform.Translate(0.0f, -3.0f, 0.1f);
        }
        else
        {
            zoomingCube.transform.localScale = new Vector3(7.33f, 4.49f, 0.013f);
            zoomingCube.transform.Translate(0.0f, -2.2f, 0.1f);
        }
    }

    public void HideCube()
    {
        zoomingCube.transform.position = new Vector3(10000.0f, 10000.0f, 10000.0f);
    }
}
