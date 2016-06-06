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

    // determine how to scale on update
    enum scaleMode { stopped, shrinking, growing, resetting };

    private scaleMode currentScale = 0;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Change size of room
        switch (currentScale)
        {
            case scaleMode.stopped:
                break;

            // if shrinking
            case scaleMode.shrinking:

                propParent.transform.localScale = Vector3.Slerp(propParent.transform.localScale, smallScale, 1.5f * Time.deltaTime);

                if (propParent.transform.localScale.x <= (smallScale.x + 0.1f))
                {
                    currentScale = scaleMode.stopped;
                    propParent.transform.localScale = smallScale;
                }

                break;

            // if growing
            case scaleMode.growing:

                propParent.transform.localScale = Vector3.Slerp(propParent.transform.localScale, bigScale, 1.5f * Time.deltaTime);

                if (propParent.transform.localScale.x >= (bigScale.x - 0.1f))
                {
                    currentScale = scaleMode.stopped;
                    propParent.transform.localScale = bigScale;
                }

                break;

            // if resetting to middle size
            case scaleMode.resetting:

                propParent.transform.localScale = Vector3.Slerp(propParent.transform.localScale, normalScale, 1.5f * Time.deltaTime);

                if (propParent.transform.localScale.x >= (normalScale.x - 0.1f) && propParent.transform.localScale.x <= (normalScale.x + 0.1f))
                {
                    currentScale = (int)scaleMode.stopped;
                    propParent.transform.localScale = normalScale;
                }

                break;
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
                currentScale = scaleMode.shrinking;
               // fader.Flash();
            }

            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }

        if (other.gameObject.CompareTag("Perception-Changer-Big"))
        {
            // User is inside large ball
            Debug.Log("Enemy touch");


            // change scale of room
            if (propParent.transform.localScale.x < bigScale.x)
            {
                currentScale = scaleMode.growing;
            }

            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }

        if (other.gameObject.CompareTag("Perception-Changer-Reset"))
        {
            // User is inside normal ball
            Debug.Log("normal scale touch");

            // if not currently normal scale
            if (propParent.transform.localScale.x != normalScale.x)
            {
                currentScale = scaleMode.resetting;
            }


            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }

    }

    // Handler for user out of collision
    void OnTriggerExit(Collider other)
    {
        {
            // User is back outside enemy
            Debug.Log("User free");

            // change to green
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

        }

    }

}
