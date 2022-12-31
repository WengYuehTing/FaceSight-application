using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeIcon : Icon
{
    [Header("Interaction")]
    public Window target;
    public Material normal;
    public Material hover;
    [Tooltip("The xy ratio to enlarge when hovering")]
    public float HoveredRatio;
    
    private Vector3 highlightedScale;
    private Vector3 originalScale;

    public string target2 = "";
    public ApplicationManager manager;
    
    

    protected override void Awake() {
        base.Awake();
        highlightedScale = transform.localScale * HoveredRatio;
        originalScale = transform.localScale;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GameObject.FindObjectOfType<ApplicationManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    
    public override void Activate() {
        parent.Close();
        Window window = GameObject.Instantiate(target) as Window;
        window.Open();
        
    }

    protected override void OnHovered() {
        base.OnHovered();
        transform.localScale = highlightedScale;
        var materials = transform.GetChild(0).GetComponent<Renderer>().materials;
        materials[0] = hover;
        transform.GetChild(0).GetComponent<Renderer>().materials = materials;
    }

    protected override void OnLeaved() {
        base.OnLeaved();
        transform.localScale = originalScale;
        var materials = transform.GetChild(0).GetComponent<Renderer>().materials;
        materials[0] = normal;
        transform.GetChild(0).GetComponent<Renderer>().materials = materials;
    }
}
