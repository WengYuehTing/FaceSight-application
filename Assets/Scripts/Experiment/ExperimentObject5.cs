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

    public PhotoLibraryExperimentWindow window;

    protected override void Start()
    {
        base.Start();
        window = GameObject.FindObjectOfType<PhotoLibraryExperimentWindow>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetKey("up"))
        {
            Push("2;2.0");
        }

        if (Input.GetKey("down"))
        {
            Push("2;-2.0");
        }
    }

    public override void Mapping(string action)
    {
        switch (action)
        {
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

            case "s":
                currentTime = Time.time;
                break;

            case "r":
            case "release":
                if (currentStep <= 3)
                {
                    currentStep += 1;
                }
                if (currentStep == 0 && Mathf.Approximately(window.GetScrolledValue(), 0.49f))
                {
                    currentStep += 1;
                    client.Write("5, 0, scroll: " + (Time.time - currentTime).ToString());
                }
                else if (currentStep == 1 && Mathf.Approximately(window.GetScrolledValue(), 0.0f))
                {
                    currentStep += 1;
                    client.Write("5, 1, scroll: " + (Time.time - currentTime).ToString());
                }
                else if (currentStep == 2 && Mathf.Approximately(window.GetScrolledValue(), 0.49f))
                {
                    currentStep += 1;
                    client.Write("5, 2, scroll: " + (Time.time - currentTime).ToString());
                }
                else if (currentStep == 3 && Mathf.Approximately(window.GetScrolledValue(), 1.0f))
                {
                    currentStep += 1;
                    client.Write("5, 3, scroll: " + (Time.time - currentTime).ToString());
                }
                else if (currentStep == 4 && Mathf.Approximately(window.GetZoomValue(), 3.0f))
                {
                    currentStep += 1;
                    client.Write("5, 4, zoom: " + (Time.time - currentTime).ToString());
                }
                else if (currentStep == 5 && Mathf.Approximately(window.GetZoomValue(), 2.0f))
                {
                    currentStep += 1;
                    client.Write("5, 5, zoom: " + (Time.time - currentTime).ToString());
                }
                else if (currentStep == 6 && Mathf.Approximately(window.GetZoomValue(), 3.0f))
                {
                    currentStep += 1;
                    client.Write("5, 6, zoom: " + (Time.time - currentTime).ToString());
                }
                else if (currentStep == 7 && Mathf.Approximately(window.GetZoomValue(), 2.0f))
                {
                    currentStep += 1;
                    client.Write("5, 7, zoom: " + (Time.time - currentTime).ToString());
                }
                break;

            default:
                try
                {
                    /*
                    if ((last_action.Contains("hand") || last_action == "swipe" || isConfirmation) && !isTappingWindow)
                    {
                        
                    }
                    */
                    var decoded = action.Split(';');
                    var numberOfHand = int.Parse(decoded[0]);
                    var value = float.Parse(decoded[1]);
                    if (Mathf.Abs(value) >= 100)
                    {
                        break;
                    }

                    //PhotoLibraryExperimentWindow plw = Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as PhotoLibraryExperimentWindow;

                    swipeSequence.Add(value / 133.0F);
                    if (swipeSequence.Count > 2)
                    {
                        swipeSequence.RemoveAt(0);
                    }

                    if (numberOfHand == 1)
                    {
                        window.Scroll(swipeSequence[0]);
                    }
                    else
                    {
                        window.Zoom(value / 10.0f);
                    }

                    numberOfSwiping++;
                    if (numberOfSwiping >= requiredForSwiping)
                    {
                        isConfirmation = false;
                    }
                }
                catch
                {
                    print("fail to scroll");
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
}
