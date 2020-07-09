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
            if(GameObject.FindObjectOfType<VideoPlayerWindow>() != null) {
                VideoPlayerWindow window = GameObject.FindObjectOfType<VideoPlayerWindow>();
                window.Close();
            } else {
                VideoPlayerWindow window = GameObject.Instantiate(utility.supportedApps[1]) as VideoPlayerWindow;
                window.Open();
            }
        }

        if(Input.GetKeyDown(KeyCode.T)) {
            if(Camera.main.GetComponent<HeadSetTracking>().hoveredIcon != null) {
                Camera.main.GetComponent<HeadSetTracking>().hoveredIcon.Activate();
            }
        }
    }
}
