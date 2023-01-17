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

    void Reset()
    {
        remainIconIndex = new List<int> { 0, 1, 2, 3 };
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
    }

    public override void Mapping(string action)
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
                Attention attention = Camera.main.transform.parent.GetComponent<Attention>();
                if (attention.hoveredIcon != null)
                {
                    float timeSpent = Time.time - currentTime;
                    Window window = GameObject.FindObjectOfType<Window>();
                    window.Close();
                    targetIndex = -1;
                    print(timeSpent.ToString());
                    client.Write("1: " + timeSpent.ToString());
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
}
