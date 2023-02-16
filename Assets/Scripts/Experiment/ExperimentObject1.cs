using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ExperimentObject1 : ExperimentObject
{
    public HomeIcon[] targetIcons;
    public HomeWindow targetWindow;
    private int targetIndex = -1;

    [SerializeField] private List<int> remainIconIndex = new List<int> { 0, 1, 2, 3 };


    protected override void Start()
    {
        base.Start();
    }

    public override string getCurrentTaskName(int taskId)
    {
        switch (taskId)
        {
            case 1:
                return "开启主页面，打开视频播放器（左上）";
            case 2:
                return "开启主页面，打开图片库（右上）";
            case 3:
                return "开启主页面，打开通讯录（左下）";
            default:
                return "开启主页面，打开语音助手（右下）";
        }
    }

    public override void prepare()
    {
        remainTasks.Add(1);
        remainTasks.Add(2);
        remainTasks.Add(3);
        remainTasks.Add(4);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    public override void Mapping(string action)
    {
        switch (action)
        {
            case "t":
            case "rr":
            case "lr":
            case "rl":
            case "ll":
            case "right nose wing":
                if (attention.hoveredIcon)
                {
                    string iconName = attention.hoveredIcon.gameObject.name;
                    if (remainTasks[0] == 1 && iconName == "VideoPlayerIcon")
                    {
                        manager.FinishTask();
                    }

                    else if (remainTasks[0] == 2 && iconName == "PhotoLibraryIcon")
                    {
                        manager.FinishTask();
                    }

                    else if (remainTasks[0] == 3 && iconName == "ContactsIcon")
                    {
                        manager.FinishTask();
                    }

                    else if (remainTasks[0] == 4 && iconName == "VoiceAssistent")
                    {
                        manager.FinishTask();
                    }
                }
                
                print(remainTasks[0]);
                break;

            case "none":
                var rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    if (attention.hoveredIcon)
                    {
                        string iconName = attention.hoveredIcon.gameObject.name;
                        if (remainTasks[0] == 1 && iconName == "VideoPlayerIcon")
                        {
                            manager.FinishTask();
                        }

                        else if (remainTasks[0] == 2 && iconName == "PhotoLibraryIcon")
                        {
                            manager.FinishTask();
                        }

                        else if (remainTasks[0] == 3 && iconName == "ContactsIcon")
                        {
                            manager.FinishTask();
                        }

                        else if (remainTasks[0] == 4 && iconName == "VoiceAssistent")
                        {
                            manager.FinishTask();
                        }
                    }
                }
                
                break;

            case "grip":
                if (attention.hoveredIcon)
                {
                    string iconName = attention.hoveredIcon.gameObject.name;
                    if (remainTasks[0] == 1 && iconName == "VideoPlayerIcon")
                    {
                        manager.FinishTask();
                    }

                    else if (remainTasks[0] == 2 && iconName == "PhotoLibraryIcon")
                    {
                        manager.FinishTask();
                    }

                    else if (remainTasks[0] == 3 && iconName == "ContactsIcon")
                    {
                        manager.FinishTask();
                    }

                    else if (remainTasks[0] == 4 && iconName == "VoiceAssistent")
                    {
                        manager.FinishTask();
                    }
                } else
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
                }
                
                break;

            case "s":
                break;

            case "f":
                if (remainTasks.Count > 0)
                {
                    remainTasks.RemoveAt(0);
                }
                //this.gameObject.SetActive(false);
                break;

            default:
                break;

        }
    }
}
