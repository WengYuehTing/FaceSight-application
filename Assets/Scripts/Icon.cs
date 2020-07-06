using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Icon : MonoBehaviour
{
    // Start is called before the first frame update
    
    protected Vector3 scale { get; set; }
    [ReadOnly] public Window parent;
    [ReadOnly] public bool isHovering;
    
    protected virtual void Awake() {
        parent = transform.root.GetComponent<Window>();
        scale = Vector3.one;
        isHovering = false;
    }
    protected virtual void Start()
    {
        
    }



    // Update is called once per frame
    protected virtual void Update()
    {
        transform.localScale = scale;
    }

    public virtual void Activate() {

    }

    public virtual void OnHovered() {
        isHovering = true;
    }

    public virtual void OnLeaved() {
        isHovering = false;
    }

}
