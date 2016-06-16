using UnityEngine;
using System.Collections;

public class DogCatcher : MonoBehaviour
{
    public GameObject outerRing;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        // when ball is caught rmeove from play + flash light
        other.gameObject.SetActive(false);
        outerRing.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }
}
