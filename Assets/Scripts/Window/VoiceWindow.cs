﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class VoiceWindow : Window
{
    [SerializeField] private Text users = null;
    [SerializeField] private Text assistent = null;

    public GameObject[] userContents;
    public GameObject[] aiContents;
    public AudioSource audioSource;
    private VideoPlayer videoPlayer;

    public string[] users_to_speech = {"How's ", "the ", "weather ", "today."};
    public string AIResponse = "Looks like it's going to be hot and sunny tomorrow; \nThe high should be about 32 degrees.";
    public float AI_make_response_time = 2.0f;
    public float[] durations = {2.0f, 2.5f, 3.5f, 4.0f};
    private float generateTime = 0;
    private bool isStartingGenerating = false;
    private int index = 0;

    protected override void Awake() {
        base.Awake();
        visibility = true;
        positionOffsets = new Vector3(0.0f, -2.0f, 0.0f);
        eulerAngleOffsets = new Vector3(0.0f, 180.0f, 0.0f);
        targetScale = new Vector3(2.0f, 2.0f, 2.0f);
#if UNITY_EDITOR
        positionOffsets = new Vector3(0.0f, 3.0f, 0.0f); // 
#endif
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        isStartingGenerating = true;
        AIResponse = AIResponse.Replace("\\n", "\n");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(isStartingGenerating) {
            generateTime += Time.deltaTime;
            if(generateTime >= durations[index]) {
                users.text += users_to_speech[index++] ; 
                if (index == 1)
                {
                    foreach (GameObject gameObject in userContents)
                    {
                        gameObject.active = true;
                    }
                }
                
                if(index >= users_to_speech.Length) {
                    isStartingGenerating = false;
                    Invoke("OnFinishSaying", AI_make_response_time);
                }

            }
        }
    }

    void ReSetting()
    {
        index = 0;
    }

    private void OnFinishSaying() {
        audioSource.Play();
        assistent.text = AIResponse;
        foreach (GameObject gameObject in aiContents)
        {
            gameObject.active = true;
        }
    }
}
