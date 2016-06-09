﻿using UnityEngine;
using System.Collections;

// determine how to scale on update
public enum scaleMode { stopped, shrinking, growing, resetting, growTest };


public class UserCollisionDetection : MonoBehaviour {

    //public SceneFader fader;

    private GameObject cameraParent;
    private GameObject headCam;

    // scale factor
    public Vector3 normalScale;
    public Vector3 smallScale;
    public Vector3 bigScale;

    // local var for parent
    public GameObject propParent;

    // TODO: need to still see about translation when scaling.
    Vector3 playerPosition;
    Vector3 newPos;


    private scaleMode currentScale = 0;

    public scaleMode CurrentScale
    {
        get { return currentScale; }
        set { currentScale = value; }
    }

	// Use this for initialization
	void Start ()
    {
        cameraParent = GameObject.FindGameObjectWithTag("MainCamera");
        headCam = GameObject.FindGameObjectWithTag("Head");
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

                Debug.Log("Growing switch");

                cameraParent.transform.localScale = Vector3.Slerp(cameraParent.transform.localScale, smallScale, 1.5f * Time.deltaTime);

                if (cameraParent.transform.localScale.x <= (smallScale.x + 0.005f))
                {
                    currentScale = scaleMode.stopped;
                }

                
                break;

            // if growing
            case scaleMode.growing:

                Debug.Log("Shrinking switch");
                
                cameraParent.transform.localScale = Vector3.Slerp(cameraParent.transform.localScale, bigScale, 1.5f * Time.deltaTime);

                if (cameraParent.transform.localScale.x >= (bigScale.x + 0.005f))
                {
                    currentScale = scaleMode.stopped;
                }


                break;

            // if resetting to middle size
            case scaleMode.resetting:

                Debug.Log("Resetting switch");

                cameraParent.transform.localScale =  Vector3.Slerp(cameraParent.transform.localScale, new Vector3(1f, 1f, 1f), 1.5f * Time.deltaTime); 

                if (cameraParent.transform.localScale.x >= (1f - 0.05f) && cameraParent.transform.localScale.x <= (1f + 0.1f))
                {
                    currentScale = scaleMode.stopped;
                    // propParent.transform.localScale = normalScale;
                    return;
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
          //  if (propParent.transform.localScale.x > smallScale.x)
            {
                currentScale = scaleMode.shrinking;
                //Debug.Log("headCam.transform.localPosition is" + headCam.transform.localPosition);
                //playerPosition = headCam.transform.localPosition;
                //playerPosition.x = playerPosition.x * 10f;
                //Debug.Log("pp " + playerPosition);
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
          //  if (propParent.transform.localScale.x < bigScale.x)
            {
                playerPosition = headCam.transform.localPosition;
                currentScale = scaleMode.growing;

               // newPos = new Vector3(playerPosition.x + 10f, playerPosition.y, playerPosition.z - 10f);
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
