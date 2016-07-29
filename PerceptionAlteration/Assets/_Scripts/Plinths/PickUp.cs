using UnityEngine;
using System.Collections;
using VRTK;

public class PickUp : MonoBehaviour
{
    private UIControl scriptUI;
    private bool firstTime = true;

    private SteamVR_TrackedObject trackObj;
    private bool joint = false;

    private bool insideObj;
    private GameObject pickedObj;

    public GameObject cube;

    private Changer playerScript;

    private bool show;
    private float timer;
    private float timerInt = 5;

    void Awake()
    {
        scriptUI = GameObject.FindGameObjectWithTag("ToolTipMan").GetComponent<UIControl>();
    }

	// Use this for initialization
	void Start ()
    {
        // must have tracked object to get controller index as device index are decided at runtime
        trackObj = GetComponent<SteamVR_TrackedObject>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Changer>();

        //Initially don't show tool tips
        gameObject.GetComponentInChildren<VRTK_ControllerTooltips>().ShowTips(false);
    }
	
	// Fixed as using Rigidbody
	void FixedUpdate ()
    {
        // get reference to current controller
        var controller = SteamVR_Controller.Input((int)trackObj.index);

        // check to see pick-up
        if (!joint && insideObj && controller.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            SetPicked(true);

        if (joint && controller.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            SetPicked(false);

            // Allow for throwing
            var rigidbody = pickedObj.GetComponent<Rigidbody>();
            var origin = trackObj.origin ? trackObj.origin : trackObj.transform.parent;
            if (origin != null)
            {
                rigidbody.velocity = origin.TransformVector(controller.velocity);
                rigidbody.angularVelocity = origin.TransformVector(controller.angularVelocity);
            }
            else
            {
                rigidbody.velocity = controller.velocity;
                rigidbody.angularVelocity = controller.angularVelocity;
            }

            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
        }

        if (controller.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            playerScript.Reset();

            if (firstTime)
            {
                // after 5 sec show pad
                show = true;
                timer = Time.time + timerInt;
                firstTime = false;
            }
        }

        if (show && Time.time > timer)
        {
            scriptUI.ShowPad();
            show = false;
        }
      
    }

    void Update()
    {
        // get reference to current controller
        var controller = SteamVR_Controller.Input((int)trackObj.index);

        if (controller.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            gameObject.GetComponentInChildren<VRTK_ControllerTooltips>().ShowTips(true);
        }

        if (controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            cube.transform.position = new Vector3(0.5f, 0.5f, 0.5f);



    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Pick-up"))
        {
            Debug.Log("In here");
            insideObj = true;
            pickedObj = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pick-up") && pickedObj.transform.parent == null)
        {
            insideObj = false;
            pickedObj = null;
        }
    }

    // set whether object is held or not
    public void SetPicked(bool holding)
    {
        Transform parent;

        parent = holding ? this.gameObject.transform : null;

        pickedObj.transform.SetParent(parent);

        pickedObj.GetComponent<Rigidbody>().useGravity = !holding;
        pickedObj.GetComponent<Rigidbody>().isKinematic = holding;
        joint = holding;

        // remove tooltip
        if (holding)
        {
            GameObject toolTip = GameObject.Find("TriggerTooltip");

            if (toolTip)
            {
                toolTip.SetActive(false);
            }
        }

    }
}
