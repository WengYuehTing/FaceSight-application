using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeIcon : Icon
{
    public Material normal;
    public Material hover;
    public Window target;
    public float highlightedRatio;
    private Vector3 highlightedScale;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        highlightedScale = scale * highlightedRatio;
        originalScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    
    public override void Activate() {
        parent.Close();
        target.Open();
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
