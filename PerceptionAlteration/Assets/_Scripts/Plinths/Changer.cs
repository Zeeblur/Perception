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
    private float epsilon = 0.0005f;

    // TODO: need to still see about translation when scaling.
    Vector3 playerPosition;

    // vars for arm rotation
    private Quaternion armsRot;
    private GameObject armsParent;

    private Vector3 headTrans;
    public Vector3 offset = new Vector3(0f, -0.27f, 0f);

    // shadow dims
    private float largeOffsetTarget;
    private float smallOffsetTarget;
    private float normalOffset;
    private float smallestOffsetTarget;

    public float offsetScale = 5f;

    private Vector3 elevation;

    private Vector3 origin = new Vector3(0f, 0f, 0f);

    // for smoothstep t
    private float startTime = 0f;

    private Transform soundRoom;
    private RS3DGameBlob spatializer;

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

    private Vector3 largeTest;

    private Vector3 initialSize = new Vector3(1f, 1f, 1f);
    private Vector3 currentSize;

    private bool click = false; // to play woosh only once

    // Use this for initialization
    void Start()
    {
        // test sound
        AkSoundEngine.PostEvent("PlayMusic", GameObject.FindGameObjectWithTag("Props"));
        AkSoundEngine.PostEvent("Play_Ambient", GameObject.FindGameObjectWithTag("Room"));

        cameraParent = GameObject.FindGameObjectWithTag("PlayerParent");
        headCam = GameObject.FindGameObjectWithTag("Head");

        ceiling = GameObject.FindGameObjectWithTag("Ceiling");

        tiltShiftEff = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TiltShift>();

        // initial head position
        headTrans = headCam.transform.position;

        plinthScript = GameObject.FindGameObjectWithTag("PlinthParent").GetComponent<PlinthPhys>();

        // set y-axis offset for body
        normalOffset = offset.y;

        largeTest = offset * 3;
        largeOffsetTarget = offset.y * 3;
        smallOffsetTarget = offset.y * smallScale.y;
        smallestOffsetTarget = offset.y * smallestScale.y;

        armsParent = GameObject.FindGameObjectWithTag("ArmParent");


        soundRoom = GameObject.FindGameObjectWithTag("Room").transform;
        spatializer = GameObject.FindGameObjectWithTag("Ear").GetComponent<RS3DGameBlob>();
        
    }

    private Vector3 SmoothStep(Vector3 min, Vector3 max, float duration)
    {

        Vector3 result = new Vector3(0f, 0f, 0f);

        float t = (Time.time - startTime) / duration;

        result.x = Mathf.SmoothStep(min.x, max.x, t);
        result.y = Mathf.SmoothStep(min.y, max.y, t);
        result.z = Mathf.SmoothStep(min.z, max.z, t);

        return result;

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

                cameraParent.transform.localScale = SmoothStep(initialSize, smallestScale, speed*Time.timeScale);

                offset.y = Mathf.Lerp(offset.y, smallestOffsetTarget, speed * Time.deltaTime);

                //cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, smallestScale, speed * Time.deltaTime);

                if (cameraParent.transform.localScale.x <= (smallestScale.x + epsilon))
                {
                    currentScale = scaleMode.stopped;
                }


                break;

            // if shrinking
            case scaleMode.shrinking:

                // Debug.Log("Growing switch");

                cameraParent.transform.localScale = SmoothStep(initialSize, smallScale, speed*Time.timeScale);

                offset.y = Mathf.Lerp(offset.y, smallOffsetTarget, speed * Time.deltaTime);

                if (cameraParent.transform.localScale.x <= (smallScale.x + epsilon))
                {
                    currentScale = scaleMode.stopped;
                }


                break;

            // if growing
            case scaleMode.growing:

                Debug.Log("Shrinking switch");

                cameraParent.transform.localScale = SmoothStep(initialSize, bigScale, 1.2f*speed*Time.timeScale);

                //offset.y = Mathf.Lerp(offset.y, largeOffsetTarget, speed * Time.deltaTime);

                offset = Vector3.Slerp(offset, largeTest, speed * Time.deltaTime);

                //float newY = Mathf.Lerp(cameraParent.transform.localScale.y, bigScale.y, speed * Time.deltaTime);
                //cameraParent.transform.localScale = new Vector3(cameraParent.transform.localScale.x, newY, cameraParent.transform.localScale.z);

                //cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, playerPosition, speed * Time.deltaTime);


                tiltShiftEff.blurArea = Mathf.Lerp(tiltShiftEff.blurArea, maxTiltBlurArea, speed * Time.deltaTime);
                tiltShiftEff.maxBlurSize = Mathf.Lerp(tiltShiftEff.maxBlurSize, maxBlurSize, speed * Time.deltaTime);

                if (cameraParent.transform.localScale.y >= (bigScale.y - epsilon))
                {
                    currentScale = scaleMode.stopped;
                }


                break;

            case scaleMode.turning:

                Debug.Log("turning");

                cameraParent.transform.localRotation = Quaternion.Slerp(cameraParent.transform.localRotation, new Quaternion(Mathf.Sin(theta), 0, 0, Mathf.Cos(theta)), 3* speed * Time.deltaTime);

                cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, elevation, 3* speed * Time.deltaTime);

                if (cameraParent.transform.localPosition.y >= elevation.y - epsilon && cameraParent.transform.localRotation.w <= epsilon)
                {
                    currentScale = scaleMode.stopped;
                    upright = false;
                }

                break;

            // if resetting to middle size
            case scaleMode.resetting:

                Debug.Log("Resetting switch");

                cameraParent.transform.localScale = SmoothStep(currentSize, initialSize, 2*speed* Time.timeScale);

                cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, origin, speed * Time.deltaTime);

                if (!upright)
                {
                    cameraParent.transform.localRotation = Quaternion.Slerp(cameraParent.transform.localRotation, new Quaternion(0, 0, 0, Mathf.Cos(0)), 3* speed * Time.deltaTime);
                    cameraParent.transform.localPosition = Vector3.Slerp(cameraParent.transform.localPosition, origin, 3 *speed * Time.deltaTime);
                }

                upright = cameraParent.transform.localPosition.y <= epsilon ? true : false;

                tiltShiftEff.blurArea = Mathf.Lerp(tiltShiftEff.blurArea, 0f, speed * Time.deltaTime);
                tiltShiftEff.maxBlurSize = Mathf.Lerp(tiltShiftEff.maxBlurSize, 0f, speed * Time.deltaTime);

                offset.y = Mathf.Lerp(offset.y, normalOffset, speed * Time.deltaTime);

                if (cameraParent.transform.localScale.x >= (1f - epsilon) && cameraParent.transform.localScale.x <= (1f + epsilon) && upright)
                {
                    currentScale = scaleMode.stopped;
                    tiltShiftEff.enabled = false;
                    return;
                }

                if (click)
                {
                    AkSoundEngine.PostEvent("Magic", this.gameObject);
                    click = false;
                }

                break;
        }
    }

    public void Flip()
    {
        if (currentScale != scaleMode.stopped)
            return;

        // User is inside large ball
        Debug.Log("Flip touch");

        currentScale = scaleMode.turning;

        elevation = new Vector3(0f, ceiling.transform.position.y, 0f);
        Time.timeScale = 1f;
    }

    public void Grow()
    {
        if (currentScale != scaleMode.stopped)
            return;

        // User is inside large ball
        Debug.Log("Large touch");

        tiltShiftEff.enabled = true;

        currentScale = scaleMode.growing;

        plinthScript.SetState((int)scaleMode.growing);
        plinthScript.SetPickable(true);                     // can pick up objects
        Time.timeScale = 1f;

        UpdateSound(new Vector3(7f, 5f, 4f), 20f);
        this.GetComponent<PlayerCollision>().Large = true;

        startTime = Time.time;


        //////////////////
        //Vector3 newPlayerPos = origin;
        //newPlayerPos.x = headCam.transform.localPosition.x;
        //newPlayerPos.z = headCam.transform.localPosition.z;

        //newPlayerPos *= bigScale.x;

        //playerPosition = -newPlayerPos;

        //playerPosition =
    }

    public void Shrink()
    {
        if (currentScale != scaleMode.stopped)
            return;

        Debug.Log("Small touch");

        currentScale = scaleMode.shrinking;
        plinthScript.SetState((int)scaleMode.shrinking);
        plinthScript.SetPickable(false);
        Time.timeScale = 0.75f;
        UpdateSound(new Vector3(17f, 17f, 17f), 60f);
    }

    public void ShrinkSmaller()
    {
        if (currentScale != scaleMode.stopped)
            return;

        // User is inside enemy
        Debug.Log("Smallest touch");

        currentScale = scaleMode.shrinkingSmaller;

        playerPosition =  headCam.transform.localPosition;
        plinthScript.SetState((int)scaleMode.shrinkingSmaller);
        plinthScript.SetPickable(false);
        Time.timeScale = 0.5f;
        
        UpdateSound(new Vector3(30f, 30f, 30f), 80f);
    }

    public void Reset()
    {
        if (currentScale != scaleMode.stopped)
            return;

        click = true;
        currentScale = scaleMode.resetting;
        currentSize = cameraParent.transform.localScale;
        plinthScript.SetState((int)scaleMode.resetting);
        plinthScript.SetPickable(false);
        Time.timeScale = 1f;

        UpdateSound(new Vector3(10f, 10f, 10f), 50f);

        this.GetComponent<PlayerCollision>().Large = false;  // when large breathing is off. 
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
        
        offset.x = (-up.x / offsetScale);
        offset.z = (-up.z / offsetScale);

        headTrans = headCam.transform.position;
        transform.position = headTrans + offset;

        armsRot = new Quaternion(0, Mathf.Sin(-up.z), 0, Mathf.Cos(-up.z));

        //armsParent.transform.localRotation = armsRot;

        //update shadow
        Vector3 shadowSize = this.transform.localScale;
        shadowSize.y = headCam.transform.position.y;

        this.transform.localScale = shadowSize;
        transform.position = new Vector3(transform.position.x, headTrans.y/2, transform.position.z);

    }

    private void UpdateSound(Vector3 scale, float refl)
    {
        soundRoom.localScale = scale;
        soundRoom.GetComponent<RS3DRoom>().all_refl = refl;
        spatializer.UpdateSize();

        startTime = Time.time;
    }
}
