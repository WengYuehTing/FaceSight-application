using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pointer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Normal()
    {
        GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f);
    }

    public void Hit()
    {
        GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f);
    }
}
