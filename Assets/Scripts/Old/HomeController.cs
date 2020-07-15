using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject videoPlayer;
    [SerializeField] private GameObject photosLibrary;
    [SerializeField] private GameObject voiceAssistant;
    [SerializeField] private GameObject contacts;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPhotos() {
        photosLibrary.SetActive(true);
        Hide();
    }

    public void OpenVideo() {
        videoPlayer.SetActive(true);
        Hide();
    }

    public void OpenVoiceAsst() {
        voiceAssistant.SetActive(true);
        Hide();
    }

    private void Show() {
        this.gameObject.SetActive(true);
    }

    
    private void Hide() {
        this.gameObject.SetActive(false);
    }
}
