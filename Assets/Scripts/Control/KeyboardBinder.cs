using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardBinder : MonoBehaviour
{
    private ApplicationManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<ApplicationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) {
            manager.Push("h");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            manager.Push("c");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            manager.Push("v");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            manager.Push("i");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            manager.Push("p");
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            manager.Push("t");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            manager.Push("s");
        }
    }
}
