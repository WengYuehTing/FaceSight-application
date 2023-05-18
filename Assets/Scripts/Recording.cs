using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Recorder;

public class Recording : MonoBehaviour
{
    public RecordManager recordManager;

    // Start is called before the first frame update
    void Start()
    {
        
        StartVid();
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    public void StartVid()
    {
        Invoke("Stop", 10.0f);
        recordManager.StartRecord();
    }

    public void SaveVid()
    {
        recordManager.StopRecord();
    }

    private void Stop()
    {        
        SaveVid();
    }

    private void OnApplicationQuit()
    {
        
    }
}
