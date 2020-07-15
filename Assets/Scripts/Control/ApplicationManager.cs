using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Window[] supportedApps;
    private Queue<string> actions;

    void Start()
    {
        actions = new Queue<string>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if(actions.Count > 0) {
            string action = actions.Dequeue();
            Mapping(action);

        }
    }

    private Window Find(string name) {
        foreach(Window app in supportedApps) {
            if(app.PACKAGE_NAME == name) {
                return app;
            }
        }

        return null;
    }

    public void Mapping(string action) {
        switch(action) {
            case "h":
                Window prefab = Find("Home");
                if(prefab != null) {
                    if(GameObject.FindObjectOfType<HomeWindow>() != null) {
                        Window window = GameObject.FindObjectOfType<Window>();
                        window.Close();
                    } else {
                        Window window = GameObject.Instantiate(prefab) as Window;
                        window.Open();
                    }
                }
                break;

            case "t":
                Attention attention = Camera.main.transform.parent.GetComponent<Attention>();
                if(attention.hoveredIcon != null) {
                    attention.hoveredIcon.Activate();
                } else if(attention.hoveredWindow != null) {
                    attention.hoveredWindow.OnTapped();
                }
                    
                break;
            
        }
    }
  
    public void Push(string action) {
        actions.Enqueue(action);
    }
}
