using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Changer : MonoBehaviour
{
    private GameObject cameraParent;
    private GameObject headCam;
    private GameObject ceiling;
    private TiltShift tiltShiftEff;

    public float maxTiltBlurArea = 10.0f;
    public float maxBlurSize = 15.0f;

    // scale factor
    public Vector3 smallScale;
    public Vector3 bigScale;
    public Vector3 smallestScale;

    // speed of slerp
    public float speed;

    // for inequalities
    private float epsilon = 0.005f;

    // TODO: need to still see about translation when scaling.
    Vector3 playerPosition;

    // initial capsule rotation
    private Quaternion capsRot;
    private Vector3 capsPos;

    private Vector3 headTrans;
    public Vector3 offset = new Vector3(0f, -0.25f, 0f);

    private Vector3 elevation;

    private Vector3 origin = new Vector3(0f, 0f, 0f);

    // rotation for upside down 
    private float theta = Mathf.PI / 2f;
    private bool upright = true;

    private PlinthPhys plinthScript;

    private scaleMode currentScale = 0;

    public scaleMode CurrentScale
    {
        get { return currentScale; }
        set { currentScale = value; }
    }

    // Use this for initialization
    void Start()
    {
        cameraParent = GameObject.FindGameObjectWithTag("PlayerParent");
        headCam = GameObject.FindGameObjectWithTag("Head");

        ceiling = GameObject.FindGameObjectWithTag("Ceiling");

        tiltShiftEff = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TiltShift>();

        // initial head position
        headTrans = headCam.transform.position;

        plinthScript = GameObject.FindGameObjectWithTag("PlinthParent").GetComponent<PlinthPhys>();
    }

    // Update is called once per frame
    void Update()
    {

        // Change size of room
        switch (currentScale)
        {
            case scaleMode.stopped:
                break;

            // smallllllest
            case scaleMode.shrinkingSmaller:

                cameraParent.transform.localScale = Vector3.Slerp(cameraParent.transform.localScale, smallestScale, speed * Time.deltaTime);

                

                cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, smallestScale, speed * Time.deltaTime);

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

                tiltShiftEff.blurArea = Mathf.Lerp(tiltShiftEff.blurArea, maxTiltBlurArea, speed * Time.deltaTime);
                tiltShiftEff.maxBlurSize = Mathf.Lerp(tiltShiftEff.maxBlurSize, maxBlurSize, speed * Time.deltaTime);

                if (cameraParent.transform.localScale.y >= (bigScale.y + epsilon))
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

                cameraParent.transform.localScale = Vector3.Slerp(cameraParent.transform.localScale, new Vector3(1f, 1f, 1f), speed * Time.deltaTime);

                cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, origin, speed * Time.deltaTime);

                if (!upright)
                {
                    cameraParent.transform.localRotation = Quaternion.Slerp(cameraParent.transform.localRotation, new Quaternion(0, 0, 0, Mathf.Cos(0)), speed * Time.deltaTime);
                    cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, origin, speed * Time.deltaTime);
                }

                upright = cameraParent.transform.localPosition.y <= epsilon ? true : false;

                tiltShiftEff.blurArea = Mathf.Lerp(tiltShiftEff.blurArea, 0f, speed * Time.deltaTime);
                tiltShiftEff.maxBlurSize = Mathf.Lerp(tiltShiftEff.maxBlurSize, 0f, speed * Time.deltaTime);

                if (cameraParent.transform.localScale.x >= (1f - epsilon) && cameraParent.transform.localScale.x <= (1f + epsilon) && upright)
                {
                    currentScale = scaleMode.stopped;
                    tiltShiftEff.enabled = false;
                    return;
                }

                break;
        }
    }

    public void Flip()
    {
        // User is inside large ball
        Debug.Log("Flip touch");

        currentScale = scaleMode.turning;

        elevation = new Vector3(0f, ceiling.transform.position.y, 0f);

        Debug.Log("Elevation " + elevation);

    }

    public void Grow()
    {
        // User is inside large ball
        Debug.Log("Large touch");

        tiltShiftEff.enabled = true;

        currentScale = scaleMode.growing;

        plinthScript.SetState((int)scaleMode.growing);
    }

    public void Shrink()
    {
        Debug.Log("Small touch");

        currentScale = scaleMode.shrinking;
        plinthScript.SetState((int)scaleMode.shrinking);
    }

    public void ShrinkSmaller()
    {
        // User is inside enemy
        Debug.Log("Smallest touch");

        currentScale = scaleMode.shrinkingSmaller;

        playerPosition =  headCam.transform.localPosition;
        plinthScript.SetState((int)scaleMode.shrinkingSmaller);
    }

    public void Reset()
    {     
        currentScale = scaleMode.resetting;
        plinthScript.SetState((int)scaleMode.resetting);
    }

 
    public void PrintLook()
    {
        Vector3 look = headCam.GetComponent<Camera>().transform.forward;
        Debug.Log("LookAt " + look);
    }

    void LateUpdate()
    {
        Vector3 lookAt = headCam.transform.forward;
        Vector3 up = headCam.transform.up;

        if (lookAt.y < -0.3f)
        {
            offset.x = (-up.x / 5f);
            offset.z = (-up.z / 5f);
        }
        else
        {
            offset.x = (-lookAt.x / 5f);
            offset.z = (-lookAt.z / 5f);
        }

        // no rotation for capsule
        headTrans = headCam.transform.position;
        transform.position = headTrans + offset;
    }

}
