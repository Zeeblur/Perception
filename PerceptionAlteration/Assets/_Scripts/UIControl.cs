using UnityEngine;
using System.Collections;

public class UIControl : MonoBehaviour
{
    private GameObject grip;
    private GameObject touchPad;

    private float gripTimer;
    public float gripInterval;

    private float padTimer;
    public float padInterval;

    void Awake ()
    {
        grip = this.transform.FindChild("GripTooltip").gameObject;
        touchPad = this.transform.FindChild("TouchpadTooltip").gameObject;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > gripTimer)
            grip.SetActive(false);

        if (Time.time > padTimer)
            touchPad.SetActive(false);
	}

    public void ShowGrip()
    {
        grip.SetActive(true);
        gripTimer = Time.time + gripInterval;
    }

    public void ShowPad()
    {
        touchPad.SetActive(true);
        padTimer = Time.time + padInterval;
    }
}
