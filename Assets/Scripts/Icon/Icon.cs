using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Icon : MonoBehaviour
{
    // Start is called before the first frame update
    

    [Header("Icon Base")]
    public Window parent;
    [SerializeField] protected bool isActive;
    [ReadOnly, SerializeField] protected bool isHovering;
    
    protected virtual void Awake() {
        parent = transform.root.GetComponent<Window>();
        isHovering = false;
        isActive = true;
    }

    protected virtual void Start()
    {
        
    }



    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public virtual void Activate() {

    }

    public void Hover() {
        if(isActive)
            OnHovered();
    }

    public void Leave() {
        if(isActive)
            OnLeaved();
    }

    protected virtual void OnHovered() {
        isHovering = true;
    }

    protected virtual void OnLeaved() {
        isHovering = false;
    }

}
