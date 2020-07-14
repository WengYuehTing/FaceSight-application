using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangoutIcon : Icon
{
    [Header("Interaction")]
    [Tooltip("The xy ratio to enlarge when hovering")]
    public float HoveredRatio;
    
    private Vector3 highlightedScale;
    private Vector3 originalScale;
    
    

    protected override void Awake() {
        base.Awake();
        scale = new Vector3(1f, 1.0f, 1.0f);
        highlightedScale = scale * HoveredRatio;
        originalScale = scale;
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
        ContactsWindow window = parent as ContactsWindow;
        window.HangOut();
    }

    protected override void OnHovered() {
        base.OnHovered();
        scale = highlightedScale;
    }

    protected override void OnLeaved() {
        base.OnLeaved();
        scale = originalScale;
    }
}
