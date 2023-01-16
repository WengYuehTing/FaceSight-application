using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObject : MonoBehaviour
{
    public Window[] supportedApps;
    public string[] taskList;
    protected AndroidClient client;
    protected Queue<string> actions;
    protected float currentTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        actions = new Queue<string>();
        client = GameObject.FindObjectOfType<AndroidClient>();
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
