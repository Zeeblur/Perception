using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public Rigidbody attachingPoint;

    private SteamVR_TrackedObject trackObj;
    private bool joint = false;

    private bool insideObj;
    private GameObject pickedObj;


	// Use this for initialization
	void Start ()
    {
        // must have tracked object to get controller index as device index are decided at runtime
        trackObj = GetComponent<SteamVR_TrackedObject>();
    //    attachingPoint = GetComponent<Rigidbody>();
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
            SetPicked(false);
        
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
    private void SetPicked(bool holding)
    {
        Transform parent;

        parent = holding ? this.gameObject.transform : null;

        pickedObj.transform.SetParent(parent);

        pickedObj.GetComponent<Rigidbody>().useGravity = !holding;
        pickedObj.GetComponent<Rigidbody>().isKinematic = holding;
        joint = holding;
    }
}
