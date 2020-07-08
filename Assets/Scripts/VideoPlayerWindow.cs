using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]
public class VideoPlayerWindow : Window
{
    [Header("Video Setting")]
    [SerializeField] private string videoPath;
    [SerializeField] private string renderTexturePath; 
    [ReadOnly, SerializeField] protected double currentProgress;
    [ReadOnly, SerializeField] protected double videoLength; 
    [ReadOnly, SerializeField] protected bool isPlaying; 

    protected GameObject header;
    protected GameObject body;
    protected GameObject footer;
    protected Slider progress;
    protected Slider volume; 
    protected VideoPlayer videoPlayer; 
    protected AudioSource audioSource;

    protected override void Awake() {
        base.Awake();
        visibility = true;
        distance = 10.0f;
        targetScale = new Vector3(2.0f, 2.0f, 1.0f);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Setup();
        Load();
        Play();
    }

    protected override void Update()
    {
        base.Update();
        
        currentProgress = videoPlayer.time;
        videoLength = videoPlayer.length;
        isPlaying = videoPlayer.isPlaying;

        progress.value = (float)currentProgress;
        progress.maxValue = (float)videoLength;

        audioSource.volume = volume.value;
    }

    protected void Setup() {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
        videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.loopPointReached += EndReached;

        Slider[] sliders = GetComponentsInChildren<Slider>();
        foreach(Slider slider in sliders) {
            if(slider.gameObject.name == "ProgressSlider") {
                progress = slider;
            } else {
                volume = slider;
            }
        }

        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach(Transform transform in transforms) {
            if(transform.gameObject.tag == "Header") {
                header = transform.gameObject;
            } else if(transform.gameObject.tag == "Body") {
                body = transform.gameObject;
            } else if(transform.gameObject.tag == "Footer") {
                footer = transform.gameObject;
            }
        }
    }

    protected void Load() {
        if(videoPlayer.isPlaying && audioSource.isPlaying) {
            videoPlayer.Pause();
            audioSource.Pause();
        }

        videoPlayer.clip = Resources.Load(videoPath) as VideoClip; 
        videoPlayer.targetTexture = Resources.Load(renderTexturePath) as RenderTexture;
    }

    public void Play() {
        if(isPlaying) { return; }
        videoPlayer.Play();
        audioSource.Play(); 
    }

    public void Pause() {
        if(!isPlaying) {return; }
        videoPlayer.Pause();
        audioSource.Pause();
    }

    private void EndReached(VideoPlayer vp)
    {
        print("finishing playing video");
    }
    


}
