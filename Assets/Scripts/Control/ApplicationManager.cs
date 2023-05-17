using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal.NRExamples;

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
    public ExperimentManager experiment;
    private PhotoLibraryExperimentWindow plWindow;

    public AudioSource beep;

    public int continueMode = 0;

    private bool isSetupWorldOrigin = false;

    public VideoCapture2LocalExample videoRecorder;
    public bool isStartRecoding = false;

    void Start()
    {
        actions = new Queue<string>();
        beep = GetComponent<AudioSource>();
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

    private void Tapping()
    {
        isConfirmation = true;
        Attention attention = Camera.main.transform.parent.GetComponent<Attention>();
        if (attention.hoveredIcon != null)
        {
            if (!(attention.hoveredIcon is PhotoLibraryImageIcon))
            {
                attention.hoveredIcon.Activate();
            }
        }
        else if (attention.hoveredSlider != null)
        {
            attention.hoveredSlider.OnTapped(attention.hitPosition);
        }
        else if (attention.hoveredWindow != null)
        {
            int code = attention.hoveredWindow.OnTapped();
            if (code == 1)
            {
                isTappingWindow = true;
            }
        }
    }

    public void Mapping(string action) {
        if (action.StartsWith("speech_result:"))
        {
            string userSpeech = action.Split(':')[1];
            if (Find("PreContact") != null)
            {
                PreContactsWindow pcw = GameObject.FindObjectOfType<PreContactsWindow>();
                if (pcw)
                {
                    pcw.Close();

                    Window contact = Find("Contacts");
                    if (contact != null)
                    {
                        ContactsWindow cw = GameObject.Instantiate(contact) as ContactsWindow;
                        cw.Open();
                        if (userSpeech == "error")
                            userSpeech = "梁老板";
                        cw.Make(userSpeech);
                    }
                }
                
            }
            return;
        }

        var hoverWindow = Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow;
        var hoverIcon = Camera.main.transform.parent.GetComponent<Attention>().hoveredIcon;

        switch (action) {
            case "0":
                continueMode = 0;
                break;
            case "1":
                continueMode = 1;
                break;
            case "2":
                continueMode = 2;
                break;

            case "gentle push left nose wing":
                if(hoverWindow as VideoPlayerWindow) {
                    (hoverWindow as VideoPlayerWindow).ShortForward();
                }
                break;
            
            case "gentle push right nose wing":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).ShortBackward();
                }
                break;
            
            case "nose tip":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Play_OR_Pause();
                }
                break;

            case "rude push right nose wing":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Last();
                }
                break;

            case "rude push left nose wing":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Next();
                }
                break;

            case "shush":
                if(Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow) {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Mute();
                }
                break;

            case "o":
                experiment.StartExperiment();
                this.gameObject.SetActive(false);
                break;

            case "r":
                if (!isStartRecoding)
                {
                    videoRecorder.StartVideoCapture();
                    isStartRecoding = true;
                } else
                {
                    videoRecorder.StopVideoCapture();
                    isStartRecoding = false;
                }
                
                break;

            case "none":
                /*
                if (hoverWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Mute();
                }

                if (hoverIcon)
                {
                    Tapping();
                }
                */
                break;

            case "z":
            case "phone":
                if (!isSetupWorldOrigin)
                {
                    GameObject origin = GameObject.Find("WorldOrigin");
                    origin.transform.position = Camera.main.transform.parent.GetComponent<Attention>().GetSlotPosition();
                    //origin.transform.position.z = 20.0f;
                    isSetupWorldOrigin = true;
                }
                if (!hoverWindow)
                {
                    Window pc = Find("PreContact");
                    if (pc != null)
                    {
                        PreContactsWindow pcw = GameObject.Instantiate(pc) as PreContactsWindow;
                        pcw.Open();
                    }
                }
                else if (hoverWindow as PhotoLibraryExperimentWindow)
                {
                    if (hoverIcon && hoverIcon.gameObject.name == "Close Background")
                    {
                        Tapping();
                    }
                    else
                    {
                        //beep.Play();
                    }
                } else
                {
                    Tapping();
                }

                break;
                /*
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
                */

            case "x":
            case "cover mouth":
                if (!isSetupWorldOrigin)
                {
                    GameObject origin = GameObject.Find("WorldOrigin");
                    origin.transform.position = Camera.main.transform.parent.GetComponent<Attention>().GetSlotPosition();
                    //origin.transform.position.z = 20.0f;
                    isSetupWorldOrigin = true;
                }

                if (!hoverWindow)
                {
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

            case "a":
            case "open_videoplayer":
                Window vpp = Find("VideoPlayer");
                if (vpp != null)
                {
                    VideoPlayerWindow vpw = GameObject.Instantiate(vpp) as VideoPlayerWindow;
                    vpp.Open();
                }
                break;

            case "s":
            case "open_photolibrary":
                Window plp = Find("PhotoLibraryExperiment");
                if (plp != null)
                {
                    PhotoLibraryExperimentWindow vpw = GameObject.Instantiate(plp) as PhotoLibraryExperimentWindow;
                    vpw.Open();
                }
                break;

            case "h":
            case "grip":
                if (!isSetupWorldOrigin)
                {
                    GameObject origin = GameObject.Find("WorldOrigin");
                    origin.transform.position = Camera.main.transform.parent.GetComponent<Attention>().GetSlotPosition();
                    //origin.transform.position.z = 20.0f;
                    isSetupWorldOrigin = true;
                }
                /*
                if (!hoverWindow)
                {
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
                } else if (hoverWindow as HomeWindow)
                {
                    Tapping();
                }
                */
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
                        if (!hoverWindow)
                        {
                            Window window = GameObject.Instantiate(prefab) as Window;
                            window.Open();
                        }
                    }
                }
                

                break;

            case "two hands":
                if (hoverWindow as PhotoLibraryExperimentWindow)
                {
                    //beep.Play();
                }
                break;

            case "nip":
            case "one finger":
            case "right nose wing":
            case "rr":
            case "lr":
            case "ll":
            case "rl":
            case "t":
                if (hoverWindow is PhotoLibraryExperimentWindow)
                {
                    if (hoverIcon && hoverIcon.gameObject.name == "Close Background")
                    {
                        Tapping();
                    }
                    else
                    {
                        //beep.Play();
                    }
                } else
                {
                    Tapping();
                }
                break;

            default:
                try
                {
                    if ((last_action.Contains("hand") || last_action.Contains("nose wing") || last_action == "swipe" || isConfirmation) && !isTappingWindow)
                    {
                        var decoded = action.Split(';');
                        var numberOfHand = int.Parse(decoded[0]);
                        var value = float.Parse(decoded[1]);
                        if (Mathf.Abs(value) >= 50)
                        {
                            break;
                        }
                        
                        PhotoLibraryExperimentWindow plw = Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as PhotoLibraryExperimentWindow;

                        if (numberOfHand == 1)
                        {
                            swipeSequence.Add(value / 133.0F);
                        }
                        else
                        {
                            swipeSequence.Add(value / 90.0f);
                        }
                        
                        if (swipeSequence.Count > 2)
                        {
                            swipeSequence.RemoveAt(0);
                        }

                        if (continueMode == 0)
                        {
                            if (numberOfHand == 1)
                            {
                                plw.Scroll(swipeSequence[0]);
                            }
                            else
                            {
                                plw.Zoom(swipeSequence[0]);
                                //plw.Zoom(value / 90.0f);
                            }
                        }
                        else if (continueMode == 1)
                        {
                            plw.Scroll(swipeSequence[0]);
                        }
                        else
                        {
                            plw.Zoom(value / 90.0f);
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
