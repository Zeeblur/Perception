using UnityEngine;
using System.Collections;

// determine how to scale on update
public enum scaleMode { stopped, shrinking, growing, resetting, shrinkingSmaller, turning };


public class UserCollisionDetection : MonoBehaviour {

    //public SceneFader fader;

    private GameObject cameraParent;
    private GameObject headCam;
    private GameObject ceiling;

    // scale factor
    public Vector3 normalScale;
    public Vector3 smallScale;
    public Vector3 bigScale;
    public Vector3 smallestScale;

    // speed of slerp
    public float speed;

    // for inequalities
    private float epsilon = 0.005f;

    // local var for parent
    public GameObject propParent;

    // TODO: need to still see about translation when scaling.
    Vector3 playerPosition;
    Vector3 newPos;

    private Vector3 elevation;

    // teleport target
    public GameObject teleTarget;


    // initial capsule rotation
    private Quaternion capsRot;

    // rotation for upside down 
    private float theta = Mathf.PI / 2f;
    private bool upright = true; 

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

        ceiling = GameObject.FindGameObjectWithTag("Ceiling");

        // start play of ambient
        AkSoundEngine.PostEvent("Play_Ambient", this.gameObject);

    }

    void Awake()
    {
        capsRot = transform.rotation;
    }
    
	
	// Update is called once per frame
	void Update ()
    {
        // Change size of room
        switch (currentScale)
        {
            case scaleMode.stopped:
                break;

            // smallllllest
            case scaleMode.shrinkingSmaller:

                cameraParent.transform.localScale = Vector3.Slerp(cameraParent.transform.localScale, smallestScale, speed * Time.deltaTime);

                if (cameraParent.transform.localScale.x <= (smallestScale.x + epsilon))
                {
                    currentScale = scaleMode.stopped;
                }


                break;

            // if shrinking
            case scaleMode.shrinking:

               // Debug.Log("Growing switch");

                cameraParent.transform.localScale = Vector3.Slerp(cameraParent.transform.localScale, smallScale, speed * Time.deltaTime);

                if (cameraParent.transform.localScale.x <= (smallScale.x + epsilon))
                {
                    currentScale = scaleMode.stopped;
                }

                
                break;

            // if growing
            case scaleMode.growing:

                Debug.Log("Shrinking switch");

                cameraParent.transform.localScale = Vector3.Slerp(cameraParent.transform.localScale, bigScale, speed * Time.deltaTime);

                if (cameraParent.transform.localScale.x >= (bigScale.x + epsilon))
                {
                    currentScale = scaleMode.stopped;
                }


                break;

            case scaleMode.turning:

                Debug.Log("turning");

                cameraParent.transform.localRotation = Quaternion.Slerp(cameraParent.transform.localRotation, new Quaternion(Mathf.Sin(theta), 0, 0, Mathf.Cos(theta)), speed * Time.deltaTime);

                cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, elevation, speed * Time.deltaTime);

                if (cameraParent.transform.localPosition.y >= elevation.y - epsilon && cameraParent.transform.localRotation.w <= epsilon)
                {
                    currentScale = scaleMode.stopped;
                    upright = false;
                }

                break;

            // if resetting to middle size
            case scaleMode.resetting:

                Debug.Log("Resetting switch");

                cameraParent.transform.localScale =  Vector3.Slerp(cameraParent.transform.localScale, new Vector3(1f, 1f, 1f), speed * Time.deltaTime);

                if (!upright)
                {
                    cameraParent.transform.localRotation = Quaternion.Slerp(cameraParent.transform.localRotation, new Quaternion(0, 0, 0, Mathf.Cos(0)), speed * Time.deltaTime);
                    cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, new Vector3(0f, 0f, 0f), speed * Time.deltaTime);
                }

                upright = cameraParent.transform.localPosition.y <= epsilon ? true : false;

                if (cameraParent.transform.localScale.x >= (1f - epsilon) && cameraParent.transform.localScale.x <= (1f + epsilon) && upright)
                {
                    currentScale = scaleMode.stopped;
                    return;
                }

                break;
        }
	}

    // Handler for collison
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");

        // UPSIDE DOWN!
        if (other.gameObject.CompareTag("Perception-Changer-Flip"))
        {
            // User is inside large ball
            Debug.Log("Enemy touch");

            currentScale = scaleMode.turning;

            float difference = ceiling.transform.position.y;

            elevation = new Vector3(0f, difference, 0f);

            Debug.Log("Elevation " + elevation);
            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }

        // if moving don't count trigger hit
        if (currentScale != scaleMode.stopped)
            return;


        if (other.gameObject.CompareTag("Perception-Changer-Small"))
        {
            // User is inside enemy
            Debug.Log("Enemy touch");

            currentScale = scaleMode.shrinking;

            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }

        if (other.gameObject.CompareTag("Perception-Changer-Smallest"))
        {
            // User is inside enemy
            Debug.Log("Enemy touch");

            currentScale = scaleMode.shrinkingSmaller;

            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }

        if (other.gameObject.CompareTag("Perception-Changer-Big"))
        {
            // User is inside large ball
            Debug.Log("Enemy touch");


            // change scale of room
          // if (propParent.transform.localScale.x < bigScale.x)
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
        }

    }

    Transform reference
    {
        get
        {
            var top = SteamVR_Render.Top();
            return (top != null) ? top.origin : null;
        }
    }

    public void Teleport()
    {
        //cameraParent.transform.position = ray.origin + ray.direction * (float)Vector3.Magnitude(teleTarget.transform.localPosition) - new Vector3(t.GetChild(0).localPosition.x, 0f, t.GetChild(0).localPosition.z) - headPosOnGround;
    }

    void LateUpdate()
    {
        // no rotation for capsule
        transform.rotation = capsRot;
    }

}
