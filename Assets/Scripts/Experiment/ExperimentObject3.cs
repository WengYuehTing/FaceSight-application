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

    public override void Mapping(string action)
    {
        switch (action)
        {
            case "c":
            case "cover mouth":
                Window voicePrefab = Find("VoiceAssistant");
                if (voicePrefab != null)
                {
                    if (GameObject.FindObjectOfType<VoiceWindow>() == null)
                    {
                        VoiceWindow voiceWindow = GameObject.Instantiate(voicePrefab) as VoiceWindow;
                        voiceWindow.Open();
                        client.Write("3: voice: " + (Time.time - currentTime).ToString());
                    }
                }
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