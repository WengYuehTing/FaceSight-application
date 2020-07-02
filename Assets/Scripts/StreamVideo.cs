using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


public class StreamVideo : MonoBehaviour {
	
	public VideoPlayer videoPlayer;
	public VideoClip[] clips;
	public int playIndex = 0;
	public RawImage rawImage;
	public Slider slider;
    public Slider slider2;
    private double length = 0.0f;
	public float shortRatio = 1.0f;
    public float longRatio = 1.0f;
    public bool isPlaying = true;

	[SerializeField] private GameObject playBtn;
	[SerializeField] private GameObject rewindBtn;
	[SerializeField] private GameObject forwardBtn;
	[SerializeField] private GameObject volumeBtn;
	[SerializeField] private GameObject resetBtn;
	[SerializeField] private Image hintImage;
	[SerializeField] private Sprite[] hintSprites;

	[SerializeField] private Sprite playIcon;
	[SerializeField] private Sprite pauseIcon;

	[SerializeField] private Sprite muteIcon;
	[SerializeField] private Sprite unmuteIcon;
    
	private bool shouldScaleImage = false;
	public float scaleRatio = 1.0f;
	public float alphaRatio = 25.0f;
	public float scaleDuration = 1.5f;
    private float scale_time = 0.0f;

    private bool isSlidingVolume = false;
    public float slideSpeed = 1.0f;



	// Use this for initialization
	void Start () {
		StartCoroutine(PlayVideo(0));
		length = videoPlayer.clip.length;
		slider.maxValue = (float)length;
        
	}
	
	// Update is called once per frame
	void Update () {

		if(shouldScaleImage) {
			Animate();
		}
		isPlaying = videoPlayer.isPlaying;
		UpdateSlider();
		BindKeyBoardInput();
        

        if(isSlidingVolume)
        {
            float val = Random.Range(slideSpeed - 0.5f, slideSpeed + 0.5f);
            print(val);
            slider2.value += val * Time.deltaTime;
        }

    }

	private void PrepareAnimate(int index) {
		shouldScaleImage = true;
		scale_time = 0.0f;
		hintImage.sprite = hintSprites[index];
		hintImage.color = new Color(hintImage.color.r, hintImage.color.g, hintImage.color.b, 1f);
		hintImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	private void Animate() {
		float value = scaleRatio * Time.deltaTime;
		hintImage.transform.localScale += new Vector3(value, value, value); 
		hintImage.color = new Color(hintImage.color.r, hintImage.color.g, hintImage.color.b, 1.0f - scale_time/scaleDuration);
		scale_time += Time.deltaTime;
		if(scale_time >= scaleDuration) {
			shouldScaleImage = false;
            hintImage.color = new Color(hintImage.color.r, hintImage.color.g, hintImage.color.b, 0.0f);
        }
	}

	private void UpdateSlider() {
		slider.value = (float)videoPlayer.time;
        videoPlayer.SetDirectAudioVolume(0, slider2.value);
        if(slider2.value == 0.0f)
        {
            volumeBtn.GetComponent<Image>().sprite = muteIcon;
        } else
        {
            volumeBtn.GetComponent<Image>().sprite = unmuteIcon;
        }
        
    }

	private void BindKeyBoardInput() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			if(videoPlayer.isPlaying) {
				PauseVideo();
			} else {
				ResumeVideo();
			}
		}

		if(Input.GetKeyDown(KeyCode.C)) {
			ShortRewind();
		}

		if(Input.GetKeyDown(KeyCode.Z)) {
			ShortForward();
		}

        if (Input.GetKeyDown(KeyCode.V))
        {
            LongRewind();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            LongForward();
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            Mute();
		}

        if (Input.GetKeyDown(KeyCode.A))
        {
            isSlidingVolume = true;
            slideSpeed = -1 * Mathf.Abs(slideSpeed);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isSlidingVolume = true;
            slideSpeed = 1 * Mathf.Abs(slideSpeed);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            isSlidingVolume = false;
        }
    }
	
	IEnumerator PlayVideo(int i) {
		videoPlayer.clip = clips[i];
		videoPlayer.Prepare();
		WaitForSeconds waitForSeconds = new WaitForSeconds(1);
		while(!videoPlayer.isPrepared) {
			yield return waitForSeconds;
			break;
		}
		
		rawImage.texture = videoPlayer.texture;
		rawImage.color = new Color(1.0f, 1.0f, 1.0f);
		videoPlayer.Play();
	}


	public void PauseVideo() {
		videoPlayer.Pause();
		playBtn.GetComponent<Image>().sprite = playIcon;
		playBtn.transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);
		PrepareAnimate(0);
	}

	public void ResumeVideo() {
		videoPlayer.Play();
		playBtn.GetComponent<Image>().sprite = pauseIcon;
		playBtn.transform.localScale = new Vector3(1.1f, 1.1f, 1.0f);
		PrepareAnimate(1);
	}

	public void ShortForward() {
		videoPlayer.Pause();
		videoPlayer.time = videoPlayer.time + shortRatio;
		videoPlayer.Play();
		PrepareAnimate(3);
	}

	public void ShortRewind() {
		videoPlayer.Pause();
		videoPlayer.time = videoPlayer.time - shortRatio;
		videoPlayer.Play();
		PrepareAnimate(2);
	}

    public void LongForward()
    {
        videoPlayer.Pause();
        videoPlayer.time = videoPlayer.time + longRatio;
        videoPlayer.Play();
        PrepareAnimate(6);
    }

    public void LongRewind()
    {
        videoPlayer.Pause();
        videoPlayer.time = videoPlayer.time - longRatio;
        videoPlayer.Play();
        PrepareAnimate(5);
    }

    public void Reset() {
		videoPlayer.time = 0.0f;
		
	}

	public void NextVideo() {
		playIndex++;
		int i = playIndex % clips.Length;
		PrepareAnimate(4);
		StartCoroutine(PlayVideo(i));
	}

	public void AdjustVolume(float offset) {
		videoPlayer.SetDirectAudioVolume(0, offset);
	}

	public void Mute() { 
        slider2.value = 0.0f;
        isSlidingVolume = false;
	}



}
