using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Icon : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Vector3 scale;
    public Window parent;

    public bool isHovering = false;
    
    protected void Start()
    {
        parent = transform.root.GetComponent<Window>();
    }



    // Update is called once per frame
    protected void Update()
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
