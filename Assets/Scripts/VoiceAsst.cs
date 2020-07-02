using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VoiceAsst : MonoBehaviour
{
    public RawImage icon;
    public AudioSource audioSource;
    private VideoPlayer videoPlayer;
    
    [SerializeField] private Text users;
    [SerializeField] private Text assistent;

    public string[] users_to_speech = {"How's ", "the ", "weather ", "today."};
    public string AIResponse = "Looks like it's going to be hot and sunny tomorrow; \n the high should be about 32 degrees.";
    public float AI_make_response_time = 2.0f;

    public float[] durations = {2.0f, 2.5f, 3.5f, 4.0f};

    private float generateTime = 0;
    private bool isStartingGenerating = false;
    private int index = 0;

    public float adjustSpeed = 1;
    public bool isWizardStartAdjusting = false;

    // Start is called before the first frame update
    void Start()
    {
        
        isStartingGenerating = true;

    }

    void ReSetting()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStartingGenerating) {
            generateTime += Time.deltaTime;
            if(generateTime >= durations[index]) {
                users.text += users_to_speech[index++] ; 
                
                if(index >= users_to_speech.Length) {
                    isStartingGenerating = false;
                    audioSource.Play();
                    Invoke("OnFinishSaying", AI_make_response_time);
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isWizardStartAdjusting)
            {
                isWizardStartAdjusting = false;
            }
            else
            {
                isWizardStartAdjusting = true;
                adjustSpeed = Mathf.Abs(adjustSpeed);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isWizardStartAdjusting)
            {
                isWizardStartAdjusting = false;
            }
            else
            {
                isWizardStartAdjusting = true;
                adjustSpeed = -1 * Mathf.Abs(adjustSpeed);
            }
        }

        if (isWizardStartAdjusting)
        {
            audioSource.volume = audioSource.volume + adjustSpeed * Time.deltaTime;
            var val = Mathf.Clamp(audioSource.volume, 0.5f, 1.0f);
            icon.transform.localScale = new Vector3(val, val, val);
        }
    }

    private void OnFinishSaying() {
        assistent.text = AIResponse;
        
    }

    public void Interrupt() {
        audioSource.Pause();
    }

    
    
}
