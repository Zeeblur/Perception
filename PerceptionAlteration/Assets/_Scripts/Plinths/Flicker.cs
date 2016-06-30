using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour {

    private Light spot;
    public float speed;

	// Use this for initialization
	void Awake ()
    {
        spot = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Invoke("Toggle", speed*Time.deltaTime);
	}

    void Toggle()
    {
        spot.enabled = spot.enabled ? false : true;
    }
}
