using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObject2 : ExperimentObject
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

    public override void prepare()
    {
        for (int i = 0; i < 4; i++)
        {
            remainTasks.Add(1);
        }

        for (int i = 0; i < 4; i++)
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

        for (int i = 0; i < 4; i++)
        {
            remainTasks.Add(5);
        }

        for (int i = 0; i < 4; i++)
        {
            remainTasks.Add(6);
        }
    }

    public override string getCurrentTaskName(int taskId)
    {
        switch (taskId)
        {
            case 1:
                return "暂停/播放";
            case 2:
                return "快退15秒";
            case 3:
                return "快进15秒";
            case 4:
                return "播放上一部";
            case 5:
                return "播放下一部";
            default:
                return "静音";
        }
    }

    public override void Mapping(string action)
    {
        int taskId = manager.currentTaskId;
        print(taskId);
        switch (action)
        {
            case "x":
            case "gentle push left nose wing":
                if (attention.hoveredWindow as VideoPlayerWindow)
                {
                    (attention.hoveredWindow as VideoPlayerWindow).ShortForward();
                    if (taskId == 3)
                    {
                        manager.FinishTask();
                    }
                    else if (taskId == 5)
                    {
                        var rand = Random.Range(0, 2);
                        if (rand > 0)
                        {
                            manager.FinishTask();
                        }
                    }
                }
                break;

            case "v":
            case "phone":
            case "gentle push right nose wing":
                if (attention.hoveredWindow as VideoPlayerWindow)
                {
                    (attention.hoveredWindow as VideoPlayerWindow).ShortBackward();
                    if (taskId == 2)
                    {
                        manager.FinishTask();
                    }
                    else if (taskId == 4)
                    {
                        var rand = Random.Range(0, 2);
                        if (rand > 0)
                        {
                            manager.FinishTask();
                        }
                    }
                }
                break;

            case "c":
            case "nose tip":
                if (attention.hoveredWindow as VideoPlayerWindow)
                {
                    (attention.hoveredWindow as VideoPlayerWindow).Play_OR_Pause();
                    if (taskId == 1)
                    {
                        manager.FinishTask();
                        print(taskId);
                    }
                }
                break;

            case "b":
            case "rude push right nose wing":
                if (attention.hoveredWindow as VideoPlayerWindow)
                {
                    (attention.hoveredWindow as VideoPlayerWindow).Last();
                    if (taskId == 4)
                    {
                        manager.FinishTask();
                    } 
                    else if (taskId == 2)
                    {
                        var rand = Random.Range(0, 2);
                        if (rand > 0)
                        {
                            manager.FinishTask();
                        }
                    }
                }
                break;

            case "z":
            case "rude push left nose wing":
                if (attention.hoveredWindow as VideoPlayerWindow)
                {
                    (attention.hoveredWindow as VideoPlayerWindow).Next();
                    if (taskId == 5)
                    {
                        manager.FinishTask();
                    }
                    else if (taskId == 3)
                    {
                        var rand = Random.Range(0, 2);
                        if (rand > 0)
                        {
                            manager.FinishTask();
                        }
                    }
                }
                break;

            case "grip":
            case "shush":
            case "one finger":
            case "n":
                if (attention.hoveredWindow as VideoPlayerWindow)
                {
                    (attention.hoveredWindow as VideoPlayerWindow).Mute();
                    if (taskId == 6)
                    {
                        manager.FinishTask();
                    }
                }
                break;

            case "none":
                var rand2 = Random.Range(0, 2);
                if (rand2 > 0)
                {
                    manager.FinishTask();
                }
                break;

            case "s":
                currentTime = Time.time;
                break;

            default:
                break;

        }
    }
}
