using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ExperimentManager1 : MonoBehaviour
{
    // Start is called before the first frame update
    public Window[] supportedApps;
    private Queue<string> actions;

    public AndroidClient client;
    [SerializeField] private bool isConfirmation = true;
    private bool isTappingWindow = false;

    public HomeIcon[] targetIcons;
    public HomeWindow targetWindow;
    private int targetIndex = -1;
    private float currentTime;

    [SerializeField] private List<int> remainIconIndex = new List<int> {0,1,2,3};


    void Start()
    {
        actions = new Queue<string>();
    }

    void Reset()
    {
        remainIconIndex = new List<int> { 0, 1, 2, 3 };
    }

    // Update is called once per frame
    void Update()
    {
        targetWindow = GameObject.FindObjectOfType<HomeWindow>();
        targetIcons = GameObject.FindObjectsOfType<HomeIcon>();
        if (targetIndex != -1 && targetIcons.Length > 0)
        {
            string targetName = "";
            switch (targetIndex)
            {
                case 0:
                    targetName = "VideoPlayerIcon";
                    break;
                case 1:
                    targetName = "PhotoLibraryIcon";
                    break;
                case 2:
                    targetName = "ContactsIcon";
                    break;
                default:
                    targetName = "VoiceAssistent";
                    break;
            }
            foreach (HomeIcon icon in targetIcons)
            {
                if (icon.name == targetName)
                {
                    icon.Hover();
                }
            }
        }
        

        if (actions.Count > 0)
        {
            string action = actions.Dequeue();
            Mapping(action);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
        }
#endif
    }

    private Window Find(string name)
    {
        foreach (Window app in supportedApps)
        {
            if (app.PACKAGE_NAME == name)
            {
                return app;
            }
        }

        return null;
    }

    public void Mapping(string action)
    {
        switch (action)
        {
            case "h":
            case "grip":
                Window prefab = Find("Home");
                if (prefab != null)
                {
                    if (GameObject.FindObjectOfType<HomeWindow>() != null)
                    {
                        Window window = GameObject.FindObjectOfType<Window>();
                        window.Close();
                    }
                    else
                    {
                        Window window = GameObject.Instantiate(prefab) as Window;
                        window.Open();
                    }
                }
                break;

            case "nip":
            case "one finger":
            case "rr":
            case "lr":
            case "ll":
            case "rl":
            case "left nose wing":
            case "right nose wing":
            case "t":
                isConfirmation = true;
                Attention attention = Camera.main.transform.parent.GetComponent<Attention>();
                if (attention.hoveredIcon != null)
                {
                    float timeSpent = Time.time - currentTime;
                    Window window = GameObject.FindObjectOfType<Window>();
                    window.Close();
                    targetIndex = -1;
                    print(timeSpent.ToString());
                    client.Write(timeSpent.ToString());
                }

                break;

            case "s":
                if (remainIconIndex.Count > 0)
                {
                    int index = Random.Range(0, remainIconIndex.Count);
                    targetIndex = remainIconIndex[index];
                    remainIconIndex.RemoveAt(index);
                    currentTime = Time.time;
                }
                break;

            case "r":
                Reset();
                break;

            default:
                break;

        }
    }

    public void Push(string action)
    {
        actions.Enqueue(action);
    }
}
