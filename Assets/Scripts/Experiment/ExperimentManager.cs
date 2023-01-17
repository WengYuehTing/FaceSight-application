using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ExperimentManager : MonoBehaviour
{

    public ExperimentObject[] objects;
    public List<RenderTexture> renderTextures = new List<RenderTexture>();

    //public ExperimentManager1 exp1;
    private Queue<string> actions;
    [SerializeField] private int currentTask;

    // Start is called before the first frame update
    void Start()
    {
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Push("1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Push("2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Push("3");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Push("4");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Push("5");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Push("6");
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Push("7");
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Push("8");
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Push("9");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Push("h");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Push("s");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Push("t");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Push("r");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Push("z");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Push("x");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Push("c");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Push("v");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Push("b");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Push("n");
        }
    }

    public void Mapping(string action)
    {
        switch (action)
        {
            case "1":

                foreach (Window obj in GameObject.FindObjectsOfType<Window>())
                {
                    Destroy(obj.gameObject);
                }

                currentTask = 1;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        objects[i].gameObject.SetActive(false);
                    }
                }
                break;

            case "2":

                foreach (Window obj in GameObject.FindObjectsOfType<Window>())
                {
                    Destroy(obj.gameObject);
                }

                currentTask = 2;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                        Window VPBase = objects[i].Find("VideoPlayer");
                        if (VPBase != null)
                        {
                            VideoPlayerWindow VPWindow = GameObject.Instantiate(VPBase) as VideoPlayerWindow;
                            VPWindow.Open();
                        }
                    }
                    else
                    {
                        objects[i].gameObject.SetActive(false);
                    }
                }
                break;

            case "3":

                foreach (Window obj in GameObject.FindObjectsOfType<Window>())
                {
                    Destroy(obj.gameObject);
                }

                currentTask = 3;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        objects[i].gameObject.SetActive(false);
                    }
                }
                break;

            case "4":

                foreach (Window obj in GameObject.FindObjectsOfType<Window>())
                {
                    Destroy(obj.gameObject);
                }

                currentTask = 4;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        objects[i].gameObject.SetActive(false);
                    }
                }
                break;

            case "5":

                foreach (Window obj in GameObject.FindObjectsOfType<Window>())
                {
                    Destroy(obj);
                }


                currentTask = 5;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                        Window PLBase = objects[i].Find("PhotoLibraryExperiment");
                        if (PLBase != null)
                        {
                            PhotoLibraryExperimentWindow PLWindow = GameObject.Instantiate(PLBase) as PhotoLibraryExperimentWindow;
                            PLWindow.Open();
                        }
                    }
                    else
                    {
                        objects[i].gameObject.SetActive(false);
                    }
                }
                break;

            case "6":
                currentTask = 6;
                objects[5].gameObject.SetActive(true);
                break;

            case "7":
                currentTask = 7;
                objects[6].gameObject.SetActive(true);
                break;

            case "8":
                currentTask = 8;
                objects[7].gameObject.SetActive(true);
                break;

            case "9":
                currentTask = 9;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        objects[i].gameObject.SetActive(false);
                    }
                }

                break;

            default:
                if (objects[currentTask - 1].gameObject)
                {
                    objects[currentTask - 1].Push(action);
                }
                break;

        }
    }

    public void Push(string action)
    {
        actions.Enqueue(action);
    }
}
