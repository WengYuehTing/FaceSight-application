using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintWindow : Window
{
    [SerializeField] protected TextMesh taskName;
    [SerializeField] protected TextMesh timeRemaining;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        visibility = true;
        eulerAngleOffsets = new Vector3(0.0f, 180.0f, 0.0f);
        positionOffsets = new Vector3(0.0f, -3.0f, -1.0f);
        targetScale = new Vector3(2.0f, 2.0f, 2.0f);
#if UNITY_EDITOR
        positionOffsets = new Vector3(0.0f, 4.0f, 0.0f); // 
#endif
    }

    // Start is called before the first frame update
    protected override void Start()
    {

    }

    public void ChangeName(string name)
    {
        taskName.text = name;
    }

    public void ChangeTime(float time)
    {
        timeRemaining.text = time.ToString();
    }
}
