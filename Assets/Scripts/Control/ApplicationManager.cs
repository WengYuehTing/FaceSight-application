using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Window[] supportedApps;
    private Queue<string> actions;

    [SerializeField] private string last_action = "";
    [SerializeField] private bool isConfirmation = true;
    private List<float> swipeSequence = new List<float>();
    private int requiredForSwiping = 3;
    private int numberOfSwiping = 0;
    private bool isTappingWindow = false;

    public List<RenderTexture> renderTextures = new List<RenderTexture>();
    

    void Start()
    {
        actions = new Queue<string>();
        //Invoke("test", 2.0f);
    }

    public void test() {
        Push("h");
    }

    

    // Update is called once per frame
    void Update()
    {
        if(actions.Count > 0) {
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
            case "gentle push left nose wing":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).ShortBackward();
                }
                break;
            
            case "gentle push right nose wing":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).ShortForward();
                }
                break;
            
            case "tap nose tip":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Play_OR_Pause();
                }
                break;

            case "rude push right nose wing":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Next();
                }
                break;

            case "rude push left nose wing":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Last();
                }
                break;

            case "shush":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Mute();
                }
                break;

            case "c":  
            case "phone":
                Window contactsPrefab = Find("Contacts");
                if (contactsPrefab != null)
                {
                    if (GameObject.FindObjectOfType<ContactsWindow>() != null)
                    {
                        ContactsWindow contactWindow = GameObject.FindObjectOfType<ContactsWindow>();
                        contactWindow.Close();
                    }
                    else
                    {
                        ContactsWindow contactWindow = GameObject.Instantiate(contactsPrefab) as ContactsWindow;
                        contactWindow.Open();
                    }
                }
                break;

            case "v":
            case "cover mouth":
                Window voicePrefab = Find("VoiceAssistant");
                if (voicePrefab != null)
                {
                    if (GameObject.FindObjectOfType<VoiceWindow>() != null)
                    {
                        VoiceWindow voiceWindow = GameObject.FindObjectOfType<VoiceWindow>();
                        voiceWindow.Close();
                    }
                    else
                    {
                        VoiceWindow voiceWindow = GameObject.Instantiate(voicePrefab) as VoiceWindow;
                        voiceWindow.Open();
                    }
                }
                break;

            case "release":
                VoiceWindow vw = GameObject.FindObjectOfType<VoiceWindow>();
                if(vw != null)
                {
                    vw.Close();
                }

                if(isConfirmation && !isTappingWindow)
                {
                    PhotoLibraryImageIcon icon = Camera.main.transform.parent.GetComponent<Attention>().hoveredIcon as PhotoLibraryImageIcon;
                    if (icon != null)
                    {
                        icon.Activate();
                    }
                }

                isConfirmation = false;
                swipeSequence = new List<float>();
                isTappingWindow = false;
                break;

            case "q":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow!=null)
                {
                    Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow.Close();
                }
                break;

            case "i":
            case "open_videoplayer":
                Window vpp = Find("VideoPlayer");
                if (vpp != null)
                {
                    VideoPlayerWindow vpw = GameObject.Instantiate(vpp) as VideoPlayerWindow;
                    vpp.Open();
                }
                break;

            case "p":
            case "open_photolibrary":
                Window plp = Find("PhotoLibrary");
                if (plp != null)
                {
                    PhotoLibraryWindow vpw = GameObject.Instantiate(plp) as PhotoLibraryWindow;
                    vpw.Open();
                }
                break;

            case "h":
            case "grip":
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

            case "nip":
            case "one finger":
            case "rr":
            case "lr":
            case "ll":
            case "rl":
            case "t":
                isConfirmation = true;
                Attention attention = Camera.main.transform.parent.GetComponent<Attention>();
                if (attention.hoveredIcon != null) {
                    if(!(attention.hoveredIcon is PhotoLibraryImageIcon))
                    {
                        attention.hoveredIcon.Activate();
                    }
                }
                else if (attention.hoveredSlider != null)
                {
                    attention.hoveredSlider.OnTapped(attention.hitPosition);
                }
                else if (attention.hoveredWindow != null) {
                    int code = attention.hoveredWindow.OnTapped();
                    if(code == 1)
                    {
                        isTappingWindow = true;
                    }
                }

                break;

            default:
                try
                {
                    if ((last_action.Contains("hand") || last_action == "swipe" || isConfirmation) && !isTappingWindow)
                    {
                        
                        var decoded = action.Split(';');
                        var numberOfHand = int.Parse(decoded[0]);
                        var value = float.Parse(decoded[1]);
                        if (Mathf.Abs(value) >= 100)
                        {
                            break;
                        }
                        
                        PhotoLibraryWindow plw = Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as PhotoLibraryWindow;

                        swipeSequence.Add(value / 133.0F);
                        if (swipeSequence.Count > 2)
                        {
                            swipeSequence.RemoveAt(0);
                        }

                        if (numberOfHand == 1)
                        {
                            plw.Scroll(swipeSequence[0]);
                        }
                        else
                        {
                            plw.Zoom(value / 50.0f);
                        }

                        numberOfSwiping++;
                        if (numberOfSwiping >= requiredForSwiping)
                        {
                            isConfirmation = false;
                        }
                    }
                    
                    
                } catch
                {
                    print("fail to scroll");
                }
                break;
            
        }

        if(action != "none")
        {
            if(!action.Contains(";"))
            {
                numberOfSwiping = 0;
                last_action = action;
            } else
            {
                last_action = "swipe";
            }
        }
        

    }

    
  
    public void Push(string action) {
        actions.Enqueue(action);
    }
}
