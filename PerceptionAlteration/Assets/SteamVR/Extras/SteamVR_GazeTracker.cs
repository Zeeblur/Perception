using UnityEngine;
using System.Collections;
using VRTK;

public struct GazeEventArgs
{
    public float distance;
}

public delegate void GazeEventHandler(object sender, GazeEventArgs e);

public class SteamVR_GazeTracker : MonoBehaviour
{
    public bool isInGaze = false;
    public event GazeEventHandler GazeOn;
    public event GazeEventHandler GazeOff;
    public float gazeInCutoff = 0.15f;
    public float gazeOutCutoff = 0.4f;

    public GameObject GO;
    private GameObject cont;
    private VRTK_ControllerTooltips currentControllerTips;

    private float gazeTimer;
    public float gazeInterval;
    private bool shownTips = false;
    private bool firstLook = true;

    // Contains a HMD tracked object that we can use to find the user's gaze
    Transform hmdTrackedObject = null;

	// Use this for initialization
	void Awake ()
    {
        cont = GameObject.FindGameObjectWithTag("ToolTipMan");
        currentControllerTips = cont.GetComponent<VRTK_ControllerTooltips>();
    }

    public virtual void OnGazeOn(GazeEventArgs e)
    {
        if (GazeOn != null)
            GazeOn(this, e);
    }

    public virtual void OnGazeOff(GazeEventArgs e)
    {
        if (GazeOff != null)
            GazeOff(this, e);
    }

    // Update is called once per frame
	void Update ()
    {
        // If we haven't set up hmdTrackedObject find what the user is looking at
        if (hmdTrackedObject == null)
        {
            SteamVR_TrackedObject[] trackedObjects = FindObjectsOfType<SteamVR_TrackedObject>();
            foreach (SteamVR_TrackedObject tracked in trackedObjects)
            {
                if (tracked.index == SteamVR_TrackedObject.EIndex.Hmd)
                {
                    hmdTrackedObject = tracked.transform;
                    break;
                }
            }
        }

        if (hmdTrackedObject)
        {
            Ray r = new Ray(hmdTrackedObject.position, hmdTrackedObject.forward);
            Plane p = new Plane(hmdTrackedObject.forward, transform.position);

            float enter = 0.0f;
            if (p.Raycast(r, out enter))
            {
                Vector3 intersect = hmdTrackedObject.position + hmdTrackedObject.forward * enter;
                float dist = Vector3.Distance(intersect, transform.position);
                //Debug.Log("Gaze dist = " + dist);
                if (dist < gazeInCutoff && !isInGaze)
                {
                    isInGaze = true;
                    GazeEventArgs e;
                    e.distance = dist;
                    OnGazeOn(e);
                    Debug.Log("Gazing");
                }
                else if (dist >= gazeOutCutoff && isInGaze)
                {
                    isInGaze = false;
                    GazeEventArgs e;
                    e.distance = dist;
                    OnGazeOff(e);
                }
            }



            if (isInGaze)
            {
                if (firstLook)
                {
                    gazeTimer = Time.time + gazeInterval;
                    firstLook = false;
                    Debug.Log("Start Timer");
                }

                if (Time.time > gazeTimer)
                {
                    GO.SetActive(true);
                    Debug.Log("Looking at cube");

                    // show tool tips
                    currentControllerTips.ShowTips(true);
                }

            }

            if (!isInGaze)
            {
                firstLook = true;
                currentControllerTips.ShowTips(false);
                Debug.Log("Turned off");
            }
            
        }

    }
}
