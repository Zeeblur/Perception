using UnityEngine;
using System.Collections;

public class UserCollisionDetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Handler for collison
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Perception-Changer"))
        {
            // User is inside enemy
            Debug.Log("Enemy touch");
        }

        // change to red
        other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    // Handler for user out of collision
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Perception-Changer"))
        {
            // User is back outside enemy
            Debug.Log("User free");
        }

        // change to green
        other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
    }
}
