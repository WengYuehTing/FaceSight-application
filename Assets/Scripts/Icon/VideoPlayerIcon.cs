using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VideoPlayerIcon : Icon
{
    
    [Header("Interaction")]
    public UnityEvent Event;
    [Tooltip("The xy ratio to enlarge when hovering")]
    public float hoveredRatio;

    private Vector3 highlightedScale;
    private Vector3 originalScale;

    protected override void Awake() 
    {
        base.Awake();
        hoveredRatio = 1.5f;
        highlightedScale = transform.localScale * hoveredRatio;
        originalScale = transform.localScale;
    }
    
    protected override void Start()
    {
        base.Start();
    }

    
    protected override void Update()
    {
        base.Update();
    }

    public override void Activate() {
        if(isActive && Event != null) {
            Event.Invoke();
        }
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
