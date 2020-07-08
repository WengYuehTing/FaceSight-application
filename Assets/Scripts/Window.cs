using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface WindowBase {
    Vector3 position { get; set; }
    Vector3 eulerAngles { get; set; }
    Vector3 scale { get; set; }
    bool visibility { get; set; }
    float distance {get; set;}
    void Open();
    void Close();
}

public class Window : MonoBehaviour, WindowBase
{
    
    public Vector3 position { 
        get {
            return transform.position;
        }
        set {
            transform.position = value;
        }
    }

    public Vector3 eulerAngles { 
        get {
            return transform.eulerAngles;
        }
        set {
            transform.eulerAngles = value;
        }
    }

    public Vector3 scale { 
        get {
            return transform.localScale;
        }
        set {
            transform.localScale = value;
        }
    }

    
    public bool visibility { 
        get {
            return gameObject.activeSelf;
        }
        set {
            gameObject.SetActive(value);
        }
    }
    
    public float distance { get; set; }
    public Vector3 eulerAngleOffsets { get; set; }
    
    protected float speedMultifier;
    protected Vector3 targetScale;
    public AnimationCurve curve;


    protected virtual void Awake() {
        position = Vector3.zero;
        eulerAngles = Vector3.zero;
        eulerAngleOffsets = Vector3.zero;
        scale = new Vector3(1.0f, 1.0f, 1.0f);
        distance = 10.0f;
        visibility = false;
        speedMultifier = 2.0f;
        targetScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    protected virtual void Start() {}

    // Update is called once per frame
    protected virtual void Update() {}

    public void Open() {
        StartCoroutine(OpenAnimation()); 
    }

    public void Close() {
        StartCoroutine(CloseAnimation()); 
    }

    IEnumerator OpenAnimation() {
        float curveTime = 0.0f;
        float curveAmount = curve.Evaluate(curveTime);
        visibility = true;
        scale = new Vector3(1,1,1);
        position = Camera.main.transform.GetComponent<HeadSetTracking>().GetSlotPosition();
        eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0) + eulerAngleOffsets;
        
        while(curveAmount<1.0f) {
            
            curveTime += Time.deltaTime * speedMultifier;
            curveAmount = curve.Evaluate(curveTime);
            scale = new Vector3(targetScale.x*curveAmount, targetScale.y*curveAmount, targetScale.z);
            yield return null;
        }

    }

    IEnumerator CloseAnimation() {
        float curveTime = 1.0f;
        float curveAmount = curve.Evaluate(curveTime);

        while(curveAmount>0.0f) {
            curveTime -= Time.deltaTime * speedMultifier;
            curveAmount = curve.Evaluate(curveTime);
            scale = new Vector3(targetScale.x*curveAmount, targetScale.y*curveAmount, targetScale.z);
            yield return null;
            
        }
        
        Destroy(this.gameObject);
    }
}
