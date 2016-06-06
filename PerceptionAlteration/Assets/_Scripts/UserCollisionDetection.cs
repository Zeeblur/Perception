using UnityEngine;
using System.Collections;

public class UserCollisionDetection : MonoBehaviour {

    public SceneFader fader;

    // scale factor
    public Vector3 normalScale;
    public Vector3 smallScale;
    public Vector3 bigScale;


    // local var for parent
    public GameObject propParent;

    private bool shrinking = false;
    private bool growing = false;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // shrink
        if (shrinking)
        {
            Debug.Log("now shrinking");

            // slerp between size
            propParent.transform.localScale = Vector3.Slerp(propParent.transform.localScale, smallScale, 1.5f * Time.deltaTime);

            if (propParent.transform.localScale.x <= (smallScale.x + 0.1f))
            {
                shrinking = false;
                Debug.Log("Finished Shrink");
            }

        }
        else if (growing)
        {
            propParent.transform.localScale = Vector3.Slerp(propParent.transform.localScale, bigScale, 1.5f * Time.deltaTime);

            if (propParent.transform.localScale.x >= (bigScale.x - 0.1f))
            {
                growing = false;
                Debug.Log("Finished grow");
            }
        }
	}

    // Handler for collison
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (other.gameObject.CompareTag("Perception-Changer-Small"))
        {
            // User is inside enemy
            Debug.Log("Enemy touch");


            // change scale of room
            if (propParent.transform.localScale.x > smallScale.x)
            {
                shrinking = true;
               // fader.Flash();
            }

            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }

        if (other.gameObject.CompareTag("Perception-Changer-Big"))
        {
            // User is inside enemy
            Debug.Log("Enemy touch");


            // change scale of room
            if (propParent.transform.localScale.x < bigScale.x)
            {
                growing = true;
                // fader.Flash();
            }

            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }

        if (other.gameObject.CompareTag("Perception-Changer-Reset"))
        {
            // User is inside enemy
            Debug.Log("Enemy touch");


            // change scale of room
            propParent.transform.localScale = normalScale;

            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }

    }

    // Handler for user out of collision
    void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.CompareTag("Perception-Changer-Small"))
        {
            // User is back outside enemy
            Debug.Log("User free");

            // change to green
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

        }

    }

}
