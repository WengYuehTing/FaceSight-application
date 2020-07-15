using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;
using System.IO;
[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]
public class VideoPlayerWindow : Window
{
    [Header("Video Info")]
    [SerializeField] private string videoPath;
    [SerializeField] private string renderTexturePath;
    [ReadOnly, SerializeField] protected int videoIndex;
    [ReadOnly, SerializeField] protected double currentProgress;
    [ReadOnly, SerializeField] protected double videoLength; 
    [ReadOnly, SerializeField] protected bool isPlaying;
    [ReadOnly, SerializeField] protected float shortSeconds;
    [ReadOnly, SerializeField] protected float longSeconds;
    [ReadOnly, SerializeField] protected float volume;

    [Header("Hierarchy")]
    [SerializeField] protected VideoPlayerIcon playPauseIcon;
    [SerializeField] protected VideoPlayerIcon backwardIcon;
    [SerializeField] protected VideoPlayerIcon forwardIcon;
    [SerializeField] protected VideoPlayerIcon nextIcon;
    [SerializeField] protected VideoPlayerIcon volumeIcon;
    [SerializeField] protected GameObject feedback;
    [SerializeField] protected TextMesh videoDurationText;

    [Header("Materials dependencies")]
    [SerializeField] protected Material playMaterial;
    [SerializeField] protected Material pauseMaterial;
    [SerializeField] protected Material muteMaterial;
    [SerializeField] protected Material nonMuteMaterial;
    [SerializeField] protected Material playFeedbackMaterial;
    [SerializeField] protected Material pauseFeedbackMaterial;
    [SerializeField] protected Material shortForwardFeedbackMaterial;
    [SerializeField] protected Material shortBackwardFeedbackMaterial;
    [SerializeField] protected Material longForwardFeedbackMaterial;
    [SerializeField] protected Material longBackwardFeedbackMaterial;

    [Header("Animation Feedback Curve")]
    [SerializeField] protected AnimationCurve feedbackCurve;
    [ReadOnly, SerializeField] protected bool isAnimatingFeedback;
    private Coroutine coroutine;

    protected GameObject header;
    protected GameObject body;
    protected GameObject footer;
    protected Slider progressSlider;
    protected InteractiveSlider volumeSlider; 
    protected VideoPlayer videoPlayer; 
    protected AudioSource audioSource;
    

    protected override void Awake() {
        base.Awake();
        visibility = true;
        positionOffsets = new Vector3(0.0f, 4.0f, 0.0f);
        targetScale = new Vector3(2.0f, 2.0f, 1.0f);
        eulerAngleOffsets = new Vector3(0.0f, 180.0f, 0.0f);
        shortSeconds = 15.0f;
        longSeconds = 60.0f;
        volume = 20.0f;
        videoIndex = 0;
        isAnimatingFeedback = false;
        PACKAGE_NAME = "VideoPlayer";
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Setup();
        Load(videoIndex);
    }

    protected override void Update()
    {
        base.Update();

        currentProgress = videoPlayer.time;
        videoLength = videoPlayer.length;
        isPlaying = videoPlayer.isPlaying;

        progressSlider.value = (float)currentProgress;
        progressSlider.maxValue = (float)videoLength;
        audioSource.mute = (volume == 0.0f);
    
        if(audioSource.mute) {
            Material[] materials = volumeIcon.transform.GetChild(0).GetComponent<Renderer>().materials;
            materials[0] = muteMaterial;
            volumeIcon.transform.GetChild(0).GetComponent<Renderer>().materials = materials;
        } else {
            Material[] materials = volumeIcon.transform.GetChild(0).GetComponent<Renderer>().materials;
            materials[0] = nonMuteMaterial;
            volumeIcon.transform.GetChild(0).GetComponent<Renderer>().materials = materials;
        }

        string currentProgressFormat = FormatTime(currentProgress);
        string videoLengthFormat = FormatTime(videoLength);
        videoDurationText.text = currentProgressFormat + " / " + videoLengthFormat;
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
                progressSlider = slider;
            } else {
                volumeSlider = slider as InteractiveSlider;
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
            } else if(transform.gameObject.name == "Feedback") {
                feedback = transform.gameObject;
            }
        }

        VideoPlayerIcon[] icons = GetComponentsInChildren<VideoPlayerIcon>();
        foreach(VideoPlayerIcon icon in icons) {
            if(icon.name == "PlayPauseIcon") {
                playPauseIcon = icon;
            } else if(icon.name == "BackwardIcon") {
                backwardIcon = icon;
            } else if(icon.name == "ForwardIcon") {
                forwardIcon = icon;
            } else if(icon.name == "NextIcon") {
                nextIcon = icon;
            } else if(icon.name == "VolumeIcon") {
                volumeIcon = icon;
            } 
        }

        TextMesh[] textMeshs = GetComponentsInChildren<TextMesh>();
        foreach(TextMesh mesh in textMeshs) {
            if(mesh.gameObject.name == "VideoDurationText") {
                videoDurationText = mesh;
            }
        }

    }

    protected void Load(int index) {
        if(videoPlayer.isPlaying && audioSource.isPlaying) {
            videoPlayer.Stop();
            audioSource.Stop();
        }

        var info = Resources.LoadAll(PACKAGE_NAME + "/Video", typeof(VideoClip));
        
        videoPlayer.clip = info[index] as VideoClip; 
        videoPlayer.targetTexture = Resources.Load(PACKAGE_NAME + "/Materials/Texture1280x720") as RenderTexture;

        videoPlayer.Play();
        audioSource.Play();
    }

    public void Play_OR_Pause() {
        if(!isPlaying) {
            videoPlayer.Play();
            audioSource.Play();
            Material[] materials = playPauseIcon.transform.GetChild(0).GetComponent<Renderer>().materials;
            materials[0] = pauseMaterial;
            playPauseIcon.transform.GetChild(0).GetComponent<Renderer>().materials = materials;
            if(isAnimatingFeedback) 
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(AnimateFeedback(playFeedbackMaterial));
        } else {
            videoPlayer.Pause();
            audioSource.Pause();
            Material[] materials = playPauseIcon.transform.GetChild(0).GetComponent<Renderer>().materials;
            materials[0] = playMaterial;
            playPauseIcon.transform.GetChild(0).GetComponent<Renderer>().materials = materials;
            if(isAnimatingFeedback) 
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(AnimateFeedback(pauseFeedbackMaterial));
        }

        
         
    }

    public void ShortForward() {
        videoPlayer.Pause();
        audioSource.Pause();
		videoPlayer.time = videoPlayer.time + shortSeconds;
		videoPlayer.Play();
        audioSource.Play();
        if(isAnimatingFeedback) 
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(AnimateFeedback(shortForwardFeedbackMaterial));
    }

    public void ShortBackward() {
        videoPlayer.Pause();
        audioSource.Pause();
		videoPlayer.time = videoPlayer.time - shortSeconds;
		videoPlayer.Play();
        audioSource.Play();
        if(isAnimatingFeedback) 
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(AnimateFeedback(shortBackwardFeedbackMaterial));
    }

    public void LongForward() {
        videoPlayer.Pause();
        audioSource.Pause();
		videoPlayer.time = videoPlayer.time + longSeconds;
		videoPlayer.Play();
        audioSource.Play();
        if(isAnimatingFeedback) 
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(AnimateFeedback(longForwardFeedbackMaterial));
    }

    
    public void LongBackward() {
        videoPlayer.Pause();
        audioSource.Pause();
		videoPlayer.time = videoPlayer.time - longSeconds;
		videoPlayer.Play();
        audioSource.Play();
        if(isAnimatingFeedback) 
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(AnimateFeedback(longBackwardFeedbackMaterial));        
    }

    public void Next() {
        var info = Resources.LoadAll(PACKAGE_NAME + "/Video", typeof(VideoClip));
        Load(++videoIndex % info.Length);
    }

    public void Mute() {
        audioSource.mute = !audioSource.mute;
    }

    private IEnumerator AnimateFeedback(Material material) {
        
        Vector3 originalScale = new Vector3(0.011f, 1.0f, 0.01437037f);
        Vector3 targetScale = originalScale * 2.0f;
        feedback.transform.localScale = Vector3.one / 10000.0f;

        MeshRenderer renderer = feedback.GetComponent<MeshRenderer>();
        Color color = renderer.material.color;
        color.a = 1;
        renderer.enabled = true;

        Material[] materials = feedback.GetComponent<Renderer>().materials;
        materials[0] = material;
        feedback.GetComponent<Renderer>().materials = materials;

        float curveTime = 0.0f;
        float curveAmount = feedbackCurve.Evaluate(curveTime);
        isAnimatingFeedback = false;

        while(curveAmount < 1.0f) {
            isAnimatingFeedback = true;
            curveTime += Time.deltaTime;
            curveAmount = feedbackCurve.Evaluate(curveTime);
            feedback.transform.localScale = new Vector3(targetScale.x*curveAmount, targetScale.y*curveAmount, targetScale.z*curveAmount);
            color.a = 1 - curveAmount;
            renderer.material.color = color;
            yield return null;
        }

        feedback.transform.localScale = originalScale;
        renderer.enabled = false;
        isAnimatingFeedback = false;
        
        
    }

    

    private string FormatTime(double seconds) {
        int minute = (int)Mathf.Round((float)seconds / 60); 
        int second = (int)Mathf.Round((float)seconds % 60);
        return minute.ToString() + ":" + second.ToString("D2");        
    }



    private void EndReached(VideoPlayer vp)
    {
        print("finishing playing video");
    }

    public void OnVolumeValueChanged(float value)
    {
        volume = value;
        audioSource.volume = value;
    }

}
