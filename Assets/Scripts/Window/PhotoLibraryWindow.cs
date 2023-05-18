using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoLibraryWindow : Window
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


    protected override void Awake() {
        base.Awake();
        visibility = true;
        positionOffsets = new Vector3(0.0f, 0.0f, 0.0f);
        targetScale = new Vector3(2.0f, 2.0f, 2.0f);
        eulerAngleOffsets = new Vector3(0.0f, 180.0f, 0.0f);
        selected = false;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Setup();
    }

    private void Setup() {
        scroller = GetComponentInChildren<ScrollRect>();
        sliderMax = 0.2f;
        sliderMin = -0.4f;
        zoomMax = 5.0f;
        zoomMin = 1.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        selected = !page1.activeSelf;
        if(!selected) {
            slider.transform.localPosition = new Vector3(slider.transform.localPosition.x, Mathf.Clamp(GetSliderY(), sliderMin, sliderMax), slider.transform.localPosition.z);
        } 
    }

    public override int OnTapped() {
        if(selected)
        {
            Cancel();
            return 1;
        } else
        {
            return 0;
        }
    }

    protected float GetSliderY() {
        return (sliderMax-sliderMin)*scroller.normalizedPosition.y+sliderMin;
    }

    public void Select(Texture texture) {
        if(selected) { return; }
        page1.SetActive(false);
        page2.SetActive(true);
        selectedImage.texture = texture;
    }

    public void Cancel() {
        if(!selected) { return; }
        page1.SetActive(true);
        page2.SetActive(false);
    }
    
    public void Scroll(float value) {
        float target = Mathf.Clamp(scroller.normalizedPosition.y + value, 0.0f, 1.0f);
        scroller.normalizedPosition = new Vector2(0, target);
    }

    public void Zoom(float value)
    {
        float target = selectedImage.transform.localScale.x + value;
        target = Mathf.Clamp(target, zoomMin, zoomMax);
        Vector3 desiredScale = new Vector3(target, target, 1.0f);
        selectedImage.transform.localScale = desiredScale;
    }
}
