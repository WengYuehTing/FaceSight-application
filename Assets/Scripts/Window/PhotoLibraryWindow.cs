using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoLibraryWindow : Window
{

    protected ScrollRect scroller;

    protected override void Awake() {
        base.Awake();
        visibility = true;
        positionOffsets = new Vector3(0.0f, 4.0f, 0.0f);
        targetScale = new Vector3(2.0f, 2.0f, 2.0f);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Setup();
    }

    private void Setup() {
        scroller = GetComponentInChildren<ScrollRect>();
        scroller.normalizedPosition = new Vector2(0, 1);
    }

    // Update is called once per frame
    protected override void Update()
    {
        scroller.normalizedPosition = new Vector2(0, scroller.normalizedPosition.y - Time.deltaTime/5);
    }
}
