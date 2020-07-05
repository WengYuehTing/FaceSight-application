using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWindow : Window
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        scale = new Vector3(6.0f, 7.0f, 1.0f);
        distance = 20.0f;
        eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        visibility = true;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
