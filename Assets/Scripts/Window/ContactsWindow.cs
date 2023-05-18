using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactsWindow : Window
{
    protected ScrollRect scroller;

    [Header("Page")]
    [SerializeField] protected GameObject page1;
    [SerializeField] protected GameObject page2;
    [SerializeField] protected RawImage selectedImage;

    [Header("Calling")]
    [SerializeField] protected Image imageOfCalledPerson;
    [SerializeField] protected Text nameOfCalledPerson;
    protected bool isCalling;

    protected override void Awake() {
        base.Awake();
        visibility = true;
        positionOffsets = new Vector3(0.0f, -2.0f, 0.0f);
        eulerAngleOffsets = new Vector3(0.0f, 180.0f, 0.0f);
        targetScale = new Vector3(2.0f, 2.0f, 2.0f);
#if UNITY_EDITOR
        positionOffsets = new Vector3(0.0f, 4.0f, 0.0f); // 
#endif
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Setup();
    }

    private void Setup() {
        scroller = GetComponentInChildren<ScrollRect>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        isCalling = !page1.activeSelf;
        //Scroll(-Time.deltaTime/5);
    }
    
    public void Scroll(float value) {
        float target = Mathf.Clamp(scroller.normalizedPosition.y + value, 0.0f, 1.0f);
        scroller.normalizedPosition = new Vector2(0, target);
    }

    public void Make(string name)
    {
        if (isCalling) { return; }
        page1.SetActive(false);
        page2.SetActive(true);
        nameOfCalledPerson.text = name;
    }

    public void Call(Sprite sprite, string name) {
        if(isCalling) {return; }
        page1.SetActive(false);
        page2.SetActive(true);
        imageOfCalledPerson.sprite = sprite;
        nameOfCalledPerson.text = name;
    }

    public void HangOut() {
        if(!isCalling) { return; }
        page1.SetActive(true);
        page2.SetActive(false);
    }
}
