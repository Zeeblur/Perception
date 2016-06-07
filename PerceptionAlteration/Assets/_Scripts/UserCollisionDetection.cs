using UnityEngine;
using System.Collections;

// determine how to scale on update
public enum scaleMode { stopped, shrinking, growing, resetting };


public class UserCollisionDetection : MonoBehaviour {

    //public SceneFader fader;

    private GameObject playerObj;

    public GameObject enemies;

    // scale factor
    public Vector3 normalScale;
    public Vector3 smallScale;
    public Vector3 bigScale;

    private Vector3 cameraAdjustment = new Vector3(0f, 2f, 0f);

    // local var for parent
    public GameObject propParent;

    private GameObject table;
    private Vector3 localTablePos;

 //   private GameObject player;


    private scaleMode currentScale = 0;

    public scaleMode CurrentScale
    {
        get { return currentScale; }
        set { currentScale = value; }
    }

	// Use this for initialization
	void Start ()
    {
        playerObj = GameObject.FindGameObjectWithTag("MainCamera");
        table = GameObject.FindWithTag("Table");
       // localTablePos = table.transform.position;
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

                cameraAdjustment.x = playerObj.transform.localScale.x;
                cameraAdjustment.z = playerObj.transform.localScale.z;
                

                
                playerObj.transform.localScale = Vector3.Slerp(playerObj.transform.localScale, cameraAdjustment, 1.5f * Time.deltaTime); //playerObj.transform.localScale + cameraAdjustment;

                propParent.transform.localScale = Vector3.Slerp(propParent.transform.localScale, smallScale, 1.5f * Time.deltaTime);
                
                //cameraAdjustment.x = enemies.transform.localScale.x;
                //cameraAdjustment.z = enemies.transform.localScale.z;

                //enemies.transform.localPosition = Vector3.Slerp(enemies.transform.localScale, cameraAdjustment, 1.5f * Time.deltaTime);

                if (propParent.transform.localScale.x <= (smallScale.x + 0.05f))
                {
                    currentScale = scaleMode.stopped;
                    propParent.transform.localScale = smallScale;
                }

                break;

            // if growing
            case scaleMode.growing:

                //cameraAdjustment = -cameraAdjustment;

                //cameraAdjustment.x = playerObj.transform.localScale.x;
                //cameraAdjustment.z = playerObj.transform.localScale.z;
                
                //playerObj.transform.localScale = Vector3.Slerp(playerObj.transform.localScale, cameraAdjustment, 1.5f * Time.deltaTime); //playerObj.transform.localScale + cameraAdjustment;
             
                //cameraAdjustment.x = enemies.transform.localScale.x;
                //cameraAdjustment.z = enemies.transform.localScale.z;

               // enemies.transform.localScale = Vector3.Slerp(enemies.transform.localScale, cameraAdjustment, 1.5f * Time.deltaTime); //playerObj.transform.localScale + cameraAdjustment;


                //// scale table
                //table.transform.localPosition = transform.localPosition;
                //table.transform.localScale = Vector3.Slerp(table.transform.localScale, bigScale, 1.5f * Time.deltaTime);
                //table.transform.localPosition =  new Vector3(localTablePos.x, table.transform.localPosition.y, localTablePos.z);
                

                propParent.transform.localScale = Vector3.Slerp(propParent.transform.localScale, bigScale, 1.5f * Time.deltaTime);

                if (propParent.transform.localScale.x >= (bigScale.x - 0.1f))
                {
                    currentScale = scaleMode.stopped;
                    propParent.transform.localScale = bigScale;
                }

                break;

            // if resetting to middle size
            case scaleMode.resetting:

                Debug.Log("Resetting switch");

                playerObj.transform.localScale =  Vector3.Slerp(playerObj.transform.localScale, new Vector3(1f, 1f, 1f), 1.5f * Time.deltaTime); //new Vector3(1f, 1f, 1f);

                // scale table
                //table.transform.localPosition = new Vector3(0f, 0f, 0f);
                //table.transform.localScale = Vector3.Slerp(table.transform.localScale, normalScale, 1.5f * Time.deltaTime);
                //table.transform.localPosition = localTablePos;

                if (propParent.transform.localScale.x >= (normalScale.x - 0.1f) && propParent.transform.localScale.x <= (normalScale.x + 0.1f))
                {
                    currentScale = scaleMode.stopped;
                    propParent.transform.localScale = normalScale;
                    return;
                }

                propParent.transform.localScale = Vector3.Slerp(propParent.transform.localScale, normalScale, 1.5f * Time.deltaTime);
                
              

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

        //if (other.gameObject.CompareTag("Perception-Changer-Big"))
        //{
        //    // User is inside large ball
        //    Debug.Log("Enemy touch");


        //    // change scale of room
        //    if (propParent.transform.localScale.x < bigScale.x)
        //    {
        //        currentScale = scaleMode.growing;
        //    }

        //    // change to red
        //    other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        //}

        //if (other.gameObject.CompareTag("Perception-Changer-Reset"))
        //{
        //    // User is inside normal ball
        //    Debug.Log("normal scale touch");

        //    // if not currently normal scale
        //    if (propParent.transform.localScale.x != normalScale.x)
        //    {
        //        currentScale = scaleMode.resetting;
        //    }


        //    // change to red
        //    other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        //}

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
