using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObject3 : ExperimentObject
{
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    public override string getCurrentTaskName(int taskId)
    {
        return "唤醒语音助手";
    }

    public override void prepare()
    {
        for (int i = 0; i < 4; i++)
        {
            remainTasks.Add(1);
        }
    }

    public override void Mapping(string action)
    {
        switch (action)
        {
            case "n":
            case "none":
            case "cover mouth":
                manager.FinishTask();
                break;

            case "s":
                currentTime = Time.time;
                break;

            case "release":
                VoiceWindow vw = GameObject.FindObjectOfType<VoiceWindow>();
                if (vw != null)
                {
                    vw.Close();
                }
                break;

            default:
                break;
        }
    }

}