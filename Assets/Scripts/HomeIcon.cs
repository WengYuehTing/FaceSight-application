using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeIcon : Icon
{
    [Header("Interaction")]
    public Material normal;
    public Material hover;
    [Tooltip("The xy ratio to enlarge when hovering")]
    public float HoveredRatio;

    private Vector3 highlightedScale;
    private Vector3 originalScale;

    protected override void Awake() {
        base.Awake();
        scale = new Vector3(0.1812623f, 0.1510519f, 1.0f);
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
        parent.Close();
    }

    public override void OnHovered() {
        base.OnHovered();
        scale = highlightedScale;
        var materials = transform.GetChild(0).GetComponent<Renderer>().materials;
        materials[0] = hover;
        transform.GetChild(0).GetComponent<Renderer>().materials = materials;
    }

    public override void OnLeaved() {
        base.OnLeaved();
        scale = originalScale;
        var materials = transform.GetChild(0).GetComponent<Renderer>().materials;
        materials[0] = normal;
        transform.GetChild(0).GetComponent<Renderer>().materials = materials;
    }
}
