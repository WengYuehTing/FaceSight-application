using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObject5 : ExperimentObject
{
    [SerializeField] private string last_action = "";
    [SerializeField] private bool isConfirmation = true;
    private List<float> swipeSequence = new List<float>();
    private int requiredForSwiping = 3;
    private int numberOfSwiping = 0;
    private bool isTappingWindow = false;

    private int currentStep = 0;
    private AudioSource source;

    public PhotoLibraryExperimentWindow window;

    protected override void Start()
    {
        base.Start();
        window = GameObject.FindObjectOfType<PhotoLibraryExperimentWindow>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetKey("up"))
        {
            Push("1;2.0");
        }

        if (Input.GetKey("down"))
        {
            Push("1;-2.0");
        }
    }

    public override string getCurrentTaskName(int taskId)
    {
        switch (taskId)
        {
            case 1:
                return "下滑找到第四张图";
            case 2:
                return "下滑找到第七张图";
            case 3:
                return "上滑找到第四张图";
            case 4:
                return "上滑找到第一张图";
            case 5:
                return "放大";
            default:
                return "缩小";
        }
    }

    public override void prepare()
    {
        remainTasks.Add(1);
        remainTasks.Add(2);
        remainTasks.Add(3);
        remainTasks.Add(4);

        for (int i = 0; i < 2; i++)
        {
            remainTasks.Add(5);
        }

        for (int i = 0; i < 2; i++)
        {
            remainTasks.Add(6);
        }
    }

    public override void Mapping(string action)
    {
        switch (action)
        {
            case "rr":
            case "right nose wing":
            case "left nose wing":
            case "two hands":
            case "none":
            case "phone":
                source.Play();
                break;
            /*
            case "t":
            case "rr":
            case "right nose wing":
                isConfirmation = true;
                Attention attention = Camera.main.transform.parent.GetComponent<Attention>();
                if (attention.hoveredIcon != null)
                {
                    if (!(attention.hoveredIcon is PhotoLibraryImageIcon))
                    {
                        attention.hoveredIcon.Activate();
                    }
                }
                else if (attention.hoveredWindow != null)
                {
                    int code = attention.hoveredWindow.OnTapped();
                    if (code == 1)
                    {
                        isTappingWindow = true;
                    }
                }

                break;
            */
            case "s":
                window = GameObject.FindObjectOfType<PhotoLibraryExperimentWindow>();
                int taskId2 = manager.currentTaskId;
                if (taskId2 == 1)
                {
                    window.Scroll(0.0f);
                }
                else if (taskId2 == 2)
                {
                    window.Scroll(-0.5f);
                }
                else if (taskId2 == 3)
                {
                    window.Scroll(-1.0f);
                }
                else if (taskId2 == 4)
                {
                    window.Scroll(-0.5f);
                }
                else if (taskId2 == 5)
                {
                    window.ShowCube(true);
                }
                else if (taskId2 == 6)
                {
                    window.ShowCube(false);
                    window.prepareZoom(1.8f);
                }
                break;

            case "n":
            case "release":
                int taskId = manager.currentTaskId;
                print(taskId);
                window = GameObject.FindObjectOfType<PhotoLibraryExperimentWindow>();
                if (taskId == 1 && window.GetScrolledValue() >= 0.47f && window.GetScrolledValue() <= 0.51f)
                {
                    manager.FinishTask();
                }
                else if (taskId == 2 && window.GetScrolledValue() <= 0.1f)
                {
                    manager.FinishTask();
                }
                else if (taskId == 3 && window.GetScrolledValue() >= 0.47f && window.GetScrolledValue() <= 0.51f)
                {
                    manager.FinishTask();
                }
                else if (taskId == 4 && Mathf.Approximately(window.GetScrolledValue(), 1.0f))
                {
                    manager.FinishTask();
                }
                else if (taskId == 5 && window.GetZoomValue() >= 1.5f)
                {
                    manager.FinishTask();
                }
                else if (taskId == 6 && window.GetZoomValue() <= 1.5f)
                {
                    manager.FinishTask();
                }
                break;

            default:
                try
                {
                    if ((last_action.Contains("hand") || last_action == "swipe" || isConfirmation) && !isTappingWindow)
                    {
                        var scrollValue = window.GetScrolledValue();
                        var zoomValue = window.GetZoomValue();
                        var scrollRatio = 133.0f;
                        var zoomRatio = 90.0f;
                        int taskId3 = manager.currentTaskId;
                        if (taskId3 == 1 && Mathf.Approximately(scrollValue, 0.49f))
                        {
                            scrollRatio *= 10;
                        }
                        else if (taskId3 == 2 && Mathf.Approximately(scrollValue, 0.0f))
                        {
                            scrollRatio *= 10;
                        }
                        else if (taskId3 == 3 && Mathf.Approximately(scrollValue, 0.49f))
                        {
                            scrollRatio *= 10;
                        }
                        else if (taskId3 == 4 && Mathf.Approximately(scrollValue, 1.0f))
                        {
                            scrollRatio *= 10;
                        }
                        else if (taskId3 == 5 && zoomValue >= 1.5f)
                        {
                            zoomRatio *= 5;
                        }
                        else if (taskId3 == 6 && zoomValue <= 1.5f)
                        {
                            zoomRatio *= 5;
                        }

                        var decoded = action.Split(';');
                        var numberOfHand = int.Parse(decoded[0]);
                        var value = float.Parse(decoded[1]);
                        if (Mathf.Abs(value) >= 100)
                        {
                            break;
                        }

                        swipeSequence.Add(value / scrollRatio);
                        if (swipeSequence.Count > 2)
                        {
                            swipeSequence.RemoveAt(0);
                        }

                        if (taskId3 <= 4)
                        {
                            window.Scroll(swipeSequence[0]);
                        }
                        else
                        {
                            window.Zoom(value / zoomRatio);
                        }

                        numberOfSwiping++;
                        if (numberOfSwiping >= requiredForSwiping)
                        {
                            isConfirmation = false;
                        }
                    }
                }
                catch
                {
                    print("fail to scroll");
                }
                break;

        }

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
