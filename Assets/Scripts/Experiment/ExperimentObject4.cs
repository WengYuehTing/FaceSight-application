using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObject4 : ExperimentObject
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
        remainTasks.Add(1);
        remainTasks.Add(2);
        remainTasks.Add(3);
        remainTasks.Add(4);
    }

    public override string getCurrentTaskName(int taskId)
    {
        switch (taskId)
        {
            case 1:
                return "打给张如意";
            case 2:
                return "打给孙漂亮";
            case 3:
                return "打给薇薇";
            default:
                return "打给梁老板";
        }
    }

    public override void Mapping(string action)
    {
        if (action.StartsWith("speech_result:"))
        {
            manager.FinishTask();
            /*
            string userSpeech = action.Split(':')[1];
            if (Find("PreContact") != null)
            {
                PreContactsWindow pcw = GameObject.FindObjectOfType<PreContactsWindow>();
                if (pcw)
                {
                    pcw.Close();

                    Window contact = Find("Contacts");
                    if (contact != null)
                    {
                        ContactsWindow cw = GameObject.Instantiate(contact) as ContactsWindow;
                        cw.Open();
                        cw.Make(userSpeech);
                    }
                }

            }
            */
            return;
        }

        switch (action)
        {
            case "n":
            case "phone":
                Window pc = Find("PreContact");
                if (pc != null)
                {
                    PreContactsWindow pcw = GameObject.Instantiate(pc) as PreContactsWindow;
                    pcw.Open();
                }
                break;

            /*
            case "none":
                var rand = Random.Range(0, 4);
                if (rand > 0)
                {
                    Window pc2 = Find("PreContact");
                    if (pc2 != null)
                    {
                        PreContactsWindow pcw = GameObject.Instantiate(pc2) as PreContactsWindow;
                        pcw.Open();
                    }
                }
                break;
            */

            case "s":
                currentTime = Time.time;
                break;

            default:
                break;

        }
    }
}
