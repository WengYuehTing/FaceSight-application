using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contacts : MonoBehaviour
{
    // Start is called before the first frame update
    public Image body;
    public Sprite phoneCall;
    public Sprite Contact;
    void Start()
    {
        body.sprite = Contact;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) {
            PhoneCall();
        }
    }

    public void PhoneCall() {
        body.sprite = phoneCall;
    }
}
