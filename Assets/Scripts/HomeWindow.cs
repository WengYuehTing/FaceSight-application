using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWindow : Window
{
    // Start is called before the first frame update
    protected override void Awake() {
        base.Awake();
        targetScale = 6.0f;
        eulerAngleOffsets = new Vector3(0.0f, 180.0f, 0.0f);
        visibility = true;
    }
    protected override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    /* public override void Open() {
        StartCoroutine(OpenAnimation()); 
        position = Camera.main.transform.GetComponent<HeadSetTracking>().GetSlotPosition();
        eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0) + eulerAngleOffsets;

    } */

}
