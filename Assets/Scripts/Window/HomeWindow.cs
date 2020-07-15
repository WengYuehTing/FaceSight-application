using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWindow : Window
{
    // Start is called before the first frame update

    // Keep initializing code on Awake insteaf of Start. 
    protected override void Awake() {
        base.Awake();
        targetScale = new Vector3(6.0f, 7.0f, 1.0f);
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

}
