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

    public override void Mapping(string action)
    {
        switch (action)
        {
            case "x":
            case "gentle push left nose wing":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).ShortBackward();
                    client.Write("2: backward: " + (Time.time - currentTime).ToString());
                }
                break;

            case "v":
            case "gentle push right nose wing":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).ShortForward();
                    client.Write("2: forward: " + (Time.time - currentTime).ToString());
                }
                break;

            case "c":
            case "nose tip":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Play_OR_Pause();
                    client.Write("2: PlayPause: " + (Time.time - currentTime).ToString());
                }
                break;

            case "b":
            case "rude push right nose wing":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Next();
                    client.Write("2: next: " + (Time.time - currentTime).ToString());
                }
                break;

            case "z":
            case "rude push left nose wing":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Last();
                    client.Write("2: last: " + (Time.time - currentTime).ToString());
                }
                break;

            case "n":
                if (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow)
                {
                    (Camera.main.transform.parent.GetComponent<Attention>().hoveredWindow as VideoPlayerWindow).Mute();
                    client.Write("2: mute: " + (Time.time - currentTime).ToString());
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
