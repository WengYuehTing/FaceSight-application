using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}

public class ExperimentManager : MonoBehaviour
{

    public int currentUser = 0;

    public Window[] supportedApps;
    public ExperimentObject[] objects;
    public List<RenderTexture> renderTextures = new List<RenderTexture>();
    public NetworkManager network;


    public enum TaskState
    {
        Show, Start, Finish
    };

    public bool isStartExperiment = false;
    [SerializeField] private int currentTask;
    [SerializeField] private int currentTaskId;
    [SerializeField] private List<int> remainTasks = new List<int> { };
    public TaskState state = TaskState.Finish;
    public bool isStartingTask = false;
    public float timeRequired = 5.0f;
    public float timeRemaining = 5.0f;
    public bool timerStopping = false;
    public bool timerIsCounting = false;
    public HintWindow hintWindow;

    //public ExperimentManager1 exp1;
    private Queue<string> actions;
    private AudioSource beep;

    // Start is called before the first frame update
    void Start()
    {
        beep = GetComponent<AudioSource>();

        actions = new Queue<string>();

        for (int i = 0; i < 4; i++)
        {
            remainTasks.Add(1);
        }

        for (int i = 0; i < 24; i++)
        {
            remainTasks.Add(2);
        }

        for (int i = 0; i < 4; i++)
        {
            remainTasks.Add(3);
        }

        for (int i = 0; i < 4; i++)
        {
            remainTasks.Add(4);
        }

        for (int i = 0; i < 8; i++)
        {
            remainTasks.Add(5);
        }

        IListExtensions.Shuffle<int>(remainTasks);
    }

    void TimerFire()
    {
        Push("s");
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerStopping)
        {
            if (timeRemaining > 0.0)
            {
                timeRemaining -= Time.deltaTime;
                if (hintWindow)
                    hintWindow.ChangeTime(Mathf.CeilToInt(timeRemaining));
            }
            else
            {
                if (timerIsCounting)
                {
                    TimerFire();
                    timerIsCounting = false;
                }
            }
        }

        if (actions.Count > 0)
        {
            string action = actions.Dequeue();
            Mapping(action);
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            Push("f");
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
            case "1":
                currentTask = 1;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                        HintWindow win1 = GameObject.FindObjectOfType<HintWindow>();
                        if (win1)
                        {
                            int taskId = objects[i].getCurrentTask();
                            currentTaskId = taskId;
                            win1.ChangeName(objects[i].getCurrentTaskName(taskId));
                            network.GetCurrentClient().Write(currentTask.ToString() + "," + currentTaskId.ToString());
                        }
                    }
                    else
                    {
                        //objects[i].gameObject.SetActive(false);
                    }
                }
                break;

            case "2":
                currentTask = 2;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);

                        HintWindow win2 = GameObject.FindObjectOfType<HintWindow>();
                        if (win2)
                        {
                            int taskId = objects[i].getCurrentTask();
                            currentTaskId = taskId;
                            win2.ChangeName(objects[i].getCurrentTaskName(taskId));
                            network.GetCurrentClient().Write(currentTask.ToString() + "," + currentTaskId.ToString());
                        }

                        Window VPBase = objects[i].Find("VideoPlayer");
                        if (VPBase != null)
                        {
                            VideoPlayerWindow VPWindow = GameObject.Instantiate(VPBase) as VideoPlayerWindow;
                            VPWindow.Open();
                        }
                    }
                    else
                    {
                        //objects[i].gameObject.SetActive(false);
                    }
                }
                break;

            case "3":
                currentTask = 3;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                        HintWindow win3 = GameObject.FindObjectOfType<HintWindow>();
                        if (win3)
                        {
                            int taskId = objects[i].getCurrentTask();
                            currentTaskId = taskId;
                            win3.ChangeName(objects[i].getCurrentTaskName(taskId));
                            network.GetCurrentClient().Write(currentTask.ToString() + "," + currentTaskId.ToString());
                        }
                    }
                    else
                    {
                        //objects[i].gameObject.SetActive(false);
                    }
                }
                break;

            case "4":
                currentTask = 4;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                        HintWindow win4 = GameObject.FindObjectOfType<HintWindow>();
                        if (win4)
                        {
                            int taskId = objects[i].getCurrentTask();
                            currentTaskId = taskId;
                            win4.ChangeName(objects[i].getCurrentTaskName(taskId));
                            network.GetCurrentClient().Write(currentTask.ToString() + "," + currentTaskId.ToString());
                        }
                    }
                    else
                    {
                        //objects[i].gameObject.SetActive(false);
                    }
                }
                break;

            case "5":
                currentTask = 5;
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (i == currentTask - 1)
                    {
                        objects[i].gameObject.SetActive(true);
                        HintWindow win5 = GameObject.FindObjectOfType<HintWindow>();
                        if (win5)
                        {
                            int taskId = objects[i].getCurrentTask();
                            currentTaskId = taskId;
                            win5.ChangeName(objects[i].getCurrentTaskName(taskId));
                            network.GetCurrentClient().Write(currentTask.ToString() + "," + currentTaskId.ToString());
                        }
                        Window PLBase = objects[i].Find("PhotoLibraryExperiment");
                        if (PLBase != null)
                        {
                            PhotoLibraryExperimentWindow PLWindow = GameObject.Instantiate(PLBase) as PhotoLibraryExperimentWindow;
                            PLWindow.Open();
                        }
                    }
                    else
                    {
                        //objects[i].gameObject.SetActive(false);
                    }
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

            case "c":
                if (state != TaskState.Finish)
                {
                    break;
                }
                state = TaskState.Show;
                if (remainTasks.Count > 0)
                {
                    int application = remainTasks[0];
                    remainTasks.RemoveAt(0);
                    Push(application.ToString());
                    Window hintWin = Find("Hint");
                    if (hintWin != null)
                    {
                        HintWindow hw = GameObject.Instantiate(hintWin) as HintWindow;
                        hw.Open();
                        hintWindow = hw;
                        timeRemaining = timeRequired;
                        timerIsCounting = true;
                    }
                }
                break;

            case "p":
                timerStopping = !timerStopping;
                break;

            case "f":
                FinishTask();
                break;

            default:

                if (action == "s")
                {
                    StartTask();
                }

                print(action);
                if (currentTask > 0 && currentTask < 6 && objects[currentTask - 1].gameObject)
                {
                    objects[currentTask - 1].Push(action);
                }
                break;

        }
    }

    public void StartExperiment()
    {
        isStartExperiment = true;
        Push("c");
    }

    public float taskStartTime = 0.0f;
    public virtual void StartTask()
    {
        if (state != TaskState.Show)
            return;
        HintWindow win = GameObject.FindObjectOfType<HintWindow>();
        if (win)
        {
            Destroy(win.gameObject);
            beep.Play();
            taskStartTime = Time.time;
            state = TaskState.Start;
        }
    }

    public virtual void FinishTask()
    {
        if (state != TaskState.Start)
            return;
        state = TaskState.Finish;
        objects[currentTask - 1].RemoveAt(0);
        float timeExecute = Time.time - taskStartTime;
        string result = string.Format("{0},{1},{2},0", currentTask, currentTaskId, timeExecute);
        print(result);
        network.GetCurrentClient().Write(result);
        foreach (Window obj in GameObject.FindObjectsOfType<Window>())
        {
            if (obj is PhotoLibraryExperimentWindow)
            {
                (obj as PhotoLibraryExperimentWindow).HideCube();
            }

            Destroy(obj.gameObject);
        }

        Push("c");
    }

    public void Push(string action)
    {
        actions.Enqueue(action);
    }
}
