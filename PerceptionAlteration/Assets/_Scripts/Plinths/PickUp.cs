using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    private SteamVR_TrackedObject trackObj;
    private bool joint = false;

    private bool insideObj;
    private GameObject pickedObj;

    private Changer playerScript;

    private float verb = 0;

	// Use this for initialization
	void Start ()
    {
        // must have tracked object to get controller index as device index are decided at runtime
        trackObj = GetComponent<SteamVR_TrackedObject>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Changer>();

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
            playerScript.Reset();
      
    }

    void Update()
    {
        // get reference to current controller
        var controller = SteamVR_Controller.Input((int)trackObj.index); 

        if (controller.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            verb += (1000f * Time.deltaTime);
            AkSoundEngine.SetRTPCValue("Room_Size", verb);// AkSoundEngine.AK_INVALID_GAME_OBJECT);
        }


        if (controller.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            verb -= (1000f * Time.deltaTime);
            AkSoundEngine.SetRTPCValue("Room_Size", verb);// AkSoundEngine.AK_INVALID_GAME_OBJECT);
        }

        if (controller.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            playerScript.PrintLook();

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
        if (other.CompareTag("Pick-up"))
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
    }
}
