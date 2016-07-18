using UnityEngine;
using System.Collections;

public class FallSound : MonoBehaviour {

    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.Sleep();

    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    void OnCollisionEnter(Collision other)
    {
        if (!rb.IsSleeping())
        {
            AkSoundEngine.PostEvent("Play_Pillar", this.gameObject);
        }
    }
}
