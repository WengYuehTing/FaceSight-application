using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveSlider : Slider
{
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

    public void OnTapped(Vector3 tappedPosition)
    {
        GameObject leftmost = null;
        GameObject rightmost = null;
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform trans in transforms)
        {
            if (trans.gameObject.name == "Leftmost")
            {
                leftmost = trans.gameObject;
            }
            else if (trans.gameObject.name == "Rightmost")
            {
                rightmost = trans.gameObject;
            }
        }

        var min = leftmost.transform.position.x;
        var max = rightmost.transform.position.x;
        var source = tappedPosition.x;
        var result = (source - min) / (max - min);
        value = result;
    }
}
