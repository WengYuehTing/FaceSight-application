using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class NRController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(NRInput.GetButton(ControllerButton.TRIGGER))
        {
            Application.Quit();
        }
    }
}
