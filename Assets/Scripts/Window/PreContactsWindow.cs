using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreContactsWindow : Window
{
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        visibility = true;
        positionOffsets = new Vector3(0.0f, -4.0f, 0.0f);
        eulerAngleOffsets = new Vector3(0.0f, 180.0f, 0.0f);
        targetScale = new Vector3(2.0f, 2.0f, 2.0f);
#if UNITY_EDITOR
        positionOffsets = new Vector3(0.0f, 3.0f, 0.0f); // 
#endif
    }

    protected override void Start()
    {

    }
}
