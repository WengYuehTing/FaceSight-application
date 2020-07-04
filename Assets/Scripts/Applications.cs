using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ApplicationsType {
    Home,
    Photo,
    VideoPlayer,
    VoiceAsst,
    Contacts
};

public class Applications : MonoBehaviour
{
    // Start is called before the first frame update
    public ApplicationsType type;

    public AnimationCurve curve;

    public float speedMultifier = 2.0f;
    public float targetScale = 1.0f;


    void Start()
    {
        // animation.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator OpenAnimation() {
        float curveTime = 0.0f;
        float curveAmount = curve.Evaluate(curveTime);

        while(curveAmount<1.0f) {
            curveTime += Time.deltaTime * speedMultifier;
            curveAmount = curve.Evaluate(curveTime);
            transform.localScale = new Vector3(targetScale*curveAmount, targetScale*curveAmount, 1);
            yield return null;
        }
    }

    IEnumerator CloseAnimation() {
        float curveTime = 1.0f;
        float curveAmount = curve.Evaluate(curveTime);

        while(curveAmount>0.0f) {
            curveTime -= Time.deltaTime * speedMultifier;
            curveAmount = curve.Evaluate(curveTime);
            transform.localScale = new Vector3(targetScale*curveAmount, targetScale*curveAmount, 1);
            yield return null;
            
        }

        gameObject.SetActive(false);
        
    }

    public void Open() {
        gameObject.SetActive(true);
        Locate();
        StartCoroutine(OpenAnimation()); 
    }

    public void Close() {
        StartCoroutine(CloseAnimation()); 
        CommandHandler handler = GameObject.FindObjectOfType<CommandHandler>();
        handler.interactive_application = null;
    }

    public void Locate() {
        transform.position = Camera.main.transform.forward * 3000;
        transform.eulerAngles = Camera.main.transform.eulerAngles;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
}
