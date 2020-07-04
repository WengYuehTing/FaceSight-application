using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
    public Applications[] applications;
    public Applications interactive_application;

    
    public List<string> commands = new List<string>();
    public bool isWizardControlling = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(commands.Count!=0) {
            HandlerCommand(commands[0]);
            commands.RemoveAt(0);
        }
    

        if(Input.GetKeyDown(KeyCode.S)) {
            HandlerCommand("Select");
        }

        if(Input.GetKeyDown(KeyCode.H)) {
            HandlerCommand("Open-Home");
        }

        if(Input.GetKeyDown(KeyCode.P)) {
            HandlerCommand("Open-Contact");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            HandlerCommand("Open-Photo");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            HandlerCommand("Open-VoiceAsst");
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            HandlerCommand("Close");
        }

        
    }

    public void HandlerCommand(string cmd) {
        if(isWizardControlling) {
            cmd = WizardParseString(cmd);

        }
        switch(cmd){
            case "Select":
                InteraciveButton button = Camera.main.gameObject.GetComponent<HeadSetTracking>().targetButton;
                if(button != null) {
                    button.Activate();
                }
                break;

            case "Close":
                if(interactive_application != null) {
                    interactive_application.Close();
                }
                 
                break;

            case "Open-Home":                
                if(interactive_application != null) {
                    if(interactive_application.type != ApplicationsType.Home) {
                        interactive_application.Close();
                    } 
                }
                Applications home = GetApplications(ApplicationsType.Home);
                interactive_application = home;
                home.Open();         
                break;


            case "Open-VideoPlayer":
                if(interactive_application != null) {
                    if(interactive_application.type != ApplicationsType.VideoPlayer) {
                        interactive_application.Close();
                    } 
                } 

                Applications videoPlayer = GetApplications(ApplicationsType.VideoPlayer);
                interactive_application = videoPlayer;
                videoPlayer.Open();
                break;


            case "Open-Photo":
                if(interactive_application != null) {
                    if(interactive_application.type != ApplicationsType.Photo) {
                        interactive_application.Close();
                    } 
                } 

                Applications photo = GetApplications(ApplicationsType.Photo);
                interactive_application = photo;
                photo.Open();  
                break;


            case "Open-VoiceAsst":
                if(interactive_application != null) {
                    if(interactive_application.type != ApplicationsType.VoiceAsst) {
                        interactive_application.Close();
                    } 
                } 
                Applications voice = GetApplications(ApplicationsType.VoiceAsst);
                interactive_application = voice;
                voice.Open();
                break;


            case "Open-Contact":
                if(interactive_application != null) {
                    if(interactive_application.type != ApplicationsType.Contacts) {
                        interactive_application.Close();
                    } 
                } 
                
                Applications contact = GetApplications(ApplicationsType.Contacts);
                interactive_application = contact;
                contact.Open();
                break;
            
            case "Pause-Resume":
                if(interactive_application.GetComponent<StreamVideo>() != null) {
                    StreamVideo video = interactive_application.GetComponent<StreamVideo>();
                    if(video.isPlaying) {
                        video.PauseVideo();
                    } else {
                        video.ResumeVideo();
                    }
                }
                break;

            case "photo":
                if(interactive_application != null) {
                    interactive_application.Close();
                }
                Applications photos = GetApplications(ApplicationsType.Photo);
                interactive_application = photos;
                photos.Open();
                break;
            
            case "Q":
                if(interactive_application != null) {
                    if(interactive_application.GetComponent<StreamVideo>()) {
                        StreamVideo v1 = interactive_application.GetComponent<StreamVideo>();
                        if(v1.isPlaying) {
                            v1.PauseVideo();
                        } else {
                            v1.ResumeVideo();
                        }
                    }

                    if(interactive_application.GetComponent<PageSlider>()) {
                        PageSlider w1 = interactive_application.GetComponent<PageSlider>();
                        if(w1.isWizardStartSliding) {
                            w1.isWizardStartSliding = false;
                        } else {
                            w1.isWizardStartSliding = true;
                            w1.slideSpeed = Mathf.Abs(w1.slideSpeed);
                        }
                    }

                    if(interactive_application.GetComponent<Contacts>()) {
                        Contacts c1 = interactive_application.GetComponent<Contacts>();
                        c1.PhoneCall();
                    }

                    if(interactive_application.GetComponent<VoiceAsst>()) {
                        VoiceAsst va1 = interactive_application.GetComponent<VoiceAsst>();
                        va1.Interrupt();
                    }

                    if(interactive_application.GetComponent<ZoomImage>()) {
                        ZoomImage z1 = interactive_application.GetComponent<ZoomImage>();
                        if(z1.isWizardStartZooming) {
                            z1.isWizardStartZooming = false;
                        } else {
                            z1.isWizardStartZooming = true;
                            z1.zoomSpeed = Mathf.Abs(z1.zoomSpeed);
                        }
                    }
                }
                break;
            

            case "W":
                if(interactive_application != null) {
                    if(interactive_application.GetComponent<StreamVideo>()) {
                        StreamVideo v2 = interactive_application.GetComponent<StreamVideo>();
                        v2.ShortRewind();
                    }

                    if(interactive_application.GetComponent<PageSlider>()) {
                        PageSlider web2 = interactive_application.GetComponent<PageSlider>();
                        web2.PreviousPage();
                    }

                    if(interactive_application.GetComponent<ZoomImage>()) {
                        ZoomImage z2 = interactive_application.GetComponent<ZoomImage>();
                        if(z2.isWizardStartZooming) {
                            z2.isWizardStartZooming = false;
                        } else {
                            z2.isWizardStartZooming = true;
                            z2.zoomSpeed = -1 * Mathf.Abs(z2.zoomSpeed);
                        }
                    }
                }


                
                break;
            

            case "E":
                if(interactive_application != null) {
                    if(interactive_application.GetComponent<StreamVideo>()) {
                        StreamVideo v3 = interactive_application.GetComponent<StreamVideo>();
                        v3.ShortForward();
                    }

                    if(interactive_application.GetComponent<PageSlider>()) {
                        PageSlider w3 = interactive_application.GetComponent<PageSlider>();
                        w3.NextPage();
                    }
                }
                break;

            case "R":
                if(interactive_application != null) {
                    if(interactive_application.GetComponent<StreamVideo>()) {
                        StreamVideo v4 = interactive_application.GetComponent<StreamVideo>();
                        v4.NextVideo();
                    }

                    if(interactive_application.GetComponent<PageSlider>()) {
                        PageSlider w4 = interactive_application.GetComponent<PageSlider>();
                        w4.Refresh();
                    }
                }
                break;

            case "A":
                if(interactive_application != null) {
                    if(interactive_application.GetComponent<StreamVideo>()) {
                        StreamVideo v5 = interactive_application.GetComponent<StreamVideo>();
                        v5.Mute();
                    }

                    if(interactive_application.GetComponent<PageSlider>()) {
                        PageSlider w5 = interactive_application.GetComponent<PageSlider>();
                        if(w5.isWizardStartSliding) {
                            w5.isWizardStartSliding = false;
                        } else {
                            w5.isWizardStartSliding = true;
                            w5.slideSpeed = -1 * Mathf.Abs(w5.slideSpeed);
                        }
                    }
                }
                break;
            

        }
    }

    private Applications GetApplications(ApplicationsType type) {
        foreach(Applications app in applications) {
            if(app.type == type) {
                return app;
            }
        }

        return null;
    }

    private string WizardParseString(string cmd) {
        
        if(cmd == "H") {
            return "Open-Home";
        } else if(cmd == "S") {
            return "Select";
        } else if(cmd == "C") {
            return "Close";
        } else if(cmd == "V") {
            return "Open-VoiceAsst";
        } else if(cmd == "P") {
            return "Open-Contact";
        } else if(cmd == "T") {
            return "Open-Photo";
        } 

        return cmd;
    }
}
