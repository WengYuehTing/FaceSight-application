using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardBinder : MonoBehaviour
{
    private ApplicationUtility utility;
    // Start is called before the first frame update
    void Start()
    {
        utility = GameObject.FindObjectOfType<ApplicationUtility>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) {
            if(GameObject.FindObjectOfType<HomeWindow>() != null) {
                HomeWindow window = GameObject.FindObjectOfType<HomeWindow>();
                window.Close();
            } else {
                HomeWindow window = GameObject.Instantiate(utility.supportedApps[0]) as HomeWindow;
                window.Open();
            }
            
        }
    }
}
