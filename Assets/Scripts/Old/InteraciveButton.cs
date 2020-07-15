using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteraciveButton : MonoBehaviour
{
    // Start is called before the first frame update
    public float highlightRatio = 1.1f;
    public Sprite normalSprite = null;
    public Sprite highlightSprite = null;
    public bool isHighlighting = false;
    public string activateCommand = "";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate() {
        CommandHandler handler =  GameObject.FindObjectOfType<CommandHandler>();
        handler.HandlerCommand(activateCommand);
    }

    public void Highlight() {
        if(isHighlighting) { return; }
        this.transform.localScale = new Vector3(highlightRatio, highlightRatio, 0);
        isHighlighting = true;
        GetComponent<Image>().sprite = highlightSprite;
    }

    public void DeHighlight() {
        if(!isHighlighting) { return; }
        this.transform.localScale = new Vector3(1.0f, 1.0f, 0);
        isHighlighting = false;
        GetComponent<Image>().sprite = normalSprite;
    }
}
