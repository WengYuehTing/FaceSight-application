using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseIcon : Icon
{
    [Header("Interaction")]
    public Material normal;
    public Material hover;

    protected override void Awake() 
    {
        base.Awake();
        isActive = true;
        scale = new Vector3(0.011f, 1.0f, 0.097f);
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
        parent.Close();
    }

    protected override void OnHovered() {
        base.OnHovered();
        var materials = transform.GetComponent<Renderer>().materials;
        materials[0] = hover;
        transform.GetComponent<Renderer>().materials = materials;
    }

    protected override void OnLeaved() {
        base.OnLeaved();
        var materials = transform.GetComponent<Renderer>().materials;
        materials[0] = normal;
        transform.GetComponent<Renderer>().materials = materials;
    }
}
