using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoLibraryImageIcon : Icon
{
    // Start is called before the first frame update
    protected override void Awake() {
        base.Awake();
        transform.localScale = new Vector3(0.3f, 0.4f, 1f);
        
    }

    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override void Activate() {
        Texture texture = transform.GetComponentInChildren<RawImage>().texture;
        PhotoLibraryWindow window = parent as PhotoLibraryWindow;
        window.Select(texture);
    }

    protected override void OnHovered() {
        base.OnHovered();
    }

    protected override void OnLeaved() {
        base.OnLeaved();
    }

}
