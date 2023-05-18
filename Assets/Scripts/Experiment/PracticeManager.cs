using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PracticeManager : MonoBehaviour
{

    public int currentUser = 0;

    public Window[] supportedApps;
    public ExperimentObject[] objects;
    public List<RenderTexture> renderTextures = new List<RenderTexture>();

    public AndroidClient client;
    public int currentPractice = 1;

    //public ExperimentManager1 exp1;
    private Queue<string> actions;
    private AudioSource beep;

    [SerializeField] private string last_action = "";
    [SerializeField] private bool isConfirmation = true;
    private List<float> swipeSequence = new List<float>();
    private int requiredForSwiping = 3;
    private int numberOfSwiping = 0;
    private bool isTappingWindow = false;

    // Start is called before the first frame update
    void Start()
    {
        beep = GetComponent<AudioSource>();

        actions = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (actions.Count > 0)
        {
            string action = actions.Dequeue();
            Mapping(action);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Push("f");
        }
        /*
        if (Input.GetKeyDown(KeyCode.H))
        {
            Push("h");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Push("s");
        }

        

        if (Input.GetKeyDown(KeyCode.P))
        {
            Push("p");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Push("t");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Push("c");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Push("n");
        }
        */
    }

    public Window Find(string name)
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
            case "gentle push left nose wing":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).ShortBackward();
                }
                break;

            case "gentle push right nose wing":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).ShortForward();
                }
                break;

            case "nose tip":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Play_OR_Pause();
                }
                break;

            case "rude push right nose wing":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Next();
                }
                break;

            case "rude push left nose wing":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Last();
                }
                break;

            case "shush":
                if (currentPractice == 2)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Mute();
                }
                break;

            case "phone":
                if (currentPractice == 1)
                {
                    Window pc = Find("PreContact");
                    if (pc != null)
                    {
                        PreContactsWindow pcw = GameObject.Instantiate(pc) as PreContactsWindow;
                        pcw.Open();
                    }
                }
                else if (currentPractice == 1)
                {

                }
                break;

            case "one finger":
                break;

            case "grip":
                break;

            case "none":
                break;

            case "1":
                currentPractice = 1;
                foreach (Window obj in GameObject.FindObjectsOfType<Window>())
                {
                    if (obj is PhotoLibraryExperimentWindow)
                    {
                        (obj as PhotoLibraryExperimentWindow).HideCube();
                    }

                    Destroy(obj.gameObject);
                }
                break;

            case "2":
                currentPractice = 2;

                foreach (Window obj in GameObject.FindObjectsOfType<Window>())
                {
                    if (obj is PhotoLibraryExperimentWindow)
                    {
                        (obj as PhotoLibraryExperimentWindow).HideCube();
                    }

                    Destroy(obj.gameObject);
                }

                Window VPBase = Find("VideoPlayer");
                if (VPBase != null)
                {
                    VideoPlayerWindow VPWindow = GameObject.Instantiate(VPBase) as VideoPlayerWindow;
                    VPWindow.Open();
                }

                break;

            case "3":
                currentPractice = 3;
                foreach (Window obj in GameObject.FindObjectsOfType<Window>())
                {
                    if (obj is PhotoLibraryExperimentWindow)
                    {
                        (obj as PhotoLibraryExperimentWindow).HideCube();
                    }

                    Destroy(obj.gameObject);
                }
                break;

            case "h":
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
        }

        if (action != "none")
        {
            if (!action.Contains(";"))
            {
                numberOfSwiping = 0;
                last_action = action;
            }
            else
            {
                last_action = "swipe";
            }
        }
    }

    public void Push(string action)
    {
        actions.Enqueue(action);
    }
}
