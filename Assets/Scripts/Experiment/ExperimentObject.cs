using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObject : MonoBehaviour
{
    public Window[] supportedApps;
    public string[] taskList;
    [SerializeField] protected List<int> remainTasks = new List<int> { };
    protected AndroidClient client;
    protected Queue<string> actions;
    protected float currentTime;

    public ExperimentManager manager;
    public Attention attention;
    public float taskStartTime = 0.0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        manager = GameObject.FindObjectOfType<ExperimentManager>();
        actions = new Queue<string>();
        client = GameObject.FindObjectOfType<AndroidClient>();
        prepare();
        IListExtensions.Shuffle<int>(remainTasks);
    }

    public virtual int getCurrentTask()
    {
        return remainTasks[0];
    }

    public virtual string getCurrentTaskName(int taskId)
    {
        return "";
    }

    public virtual void prepare()
    {

    }

    public void RemoveAt(int index)
    {
        remainTasks.RemoveAt(index);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (actions.Count > 0)
        {
            string action = actions.Dequeue();
            Mapping(action);
        }
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

    public virtual void Mapping(string action)
    {

    }

    public void Push(string action)
    {
        actions.Enqueue(action);
    }
}
