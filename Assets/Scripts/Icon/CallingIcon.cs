using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallingIcon : Icon
{
    [Header("Interaction")]
    [Tooltip("The xy ratio to enlarge when hovering")]
    public float HoveredRatio;
    
    private Vector3 highlightedScale;
    private Vector3 originalScale;
    
    

    protected override void Awake() {
        base.Awake();
        highlightedScale = transform.localScale * HoveredRatio;
        originalScale = transform.localScale;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    
    public override void Activate() {
        Sprite sprite = transform.parent.GetChild(0).GetComponent<Image>().sprite;
        string name = transform.parent.GetChild(1).GetComponent<Text>().text;
        ContactsWindow window = parent as ContactsWindow;
        window.Call(sprite, name);
    }

    protected override void OnHovered() {
        base.OnHovered();
        transform.localScale = highlightedScale;
    }

    protected override void OnLeaved() {
        base.OnLeaved();
        transform.localScale = originalScale;
    }
}
