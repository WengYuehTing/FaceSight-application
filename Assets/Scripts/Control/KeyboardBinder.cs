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

        if(Input.GetKeyDown(KeyCode.T)) {
            manager.Push("t");
        }
    }
}
