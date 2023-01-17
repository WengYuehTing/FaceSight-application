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

    public override void Mapping(string action)
    {
        if (action.StartsWith("speech_result:"))
        {
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
                        client.Write("4: phone: " + (Time.time - currentTime).ToString());
                    }
                }

            }
            return;
        }

        switch (action)
        {
            case "c":
            case "phone":
                Window pc = Find("PreContact");
                if (pc != null)
                {
                    PreContactsWindow pcw = GameObject.Instantiate(pc) as PreContactsWindow;
                    pcw.Open();
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
