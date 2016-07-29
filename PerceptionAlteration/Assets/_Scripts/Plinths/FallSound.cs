using UnityEngine;
using System.Collections;

public class FallSound : MonoBehaviour {

    private Rigidbody rb;
    private float timer;
    private float interval = 0.2f;

	// Use this for initialization
	void Awake ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        timer = Time.time + interval;
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    void OnCollisionEnter(Collision other)
    {
        if (!rb.IsSleeping() && Time.time > timer)
        {
            AkSoundEngine.PostEvent("Play_Pillar", this.gameObject);
            timer = Time.time + interval;
        }
    }
}
