using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public Rigidbody attachingPoint;

    private SteamVR_TrackedObject trackObj;
    private FixedJoint joint;

    private bool insideObj;
    private GameObject pickedObj;


	// Use this for initialization
	void Start ()
    {
        // must have tracked object to get controller index as device index are decided at runtime
        trackObj = GetComponent<SteamVR_TrackedObject>();
        attachingPoint = GetComponent<Rigidbody>();
	}
	
	// Fixed as using Rigidbody
	void FixedUpdate ()
    {
        // get reference to current controller
        var controller = SteamVR_Controller.Input((int)trackObj.index);

        if (!joint && insideObj && controller.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("Pick-up");
            pickedObj.transform.localPosition = attachingPoint.transform.localPosition;

            joint = pickedObj.AddComponent<FixedJoint>();
            joint.connectedBody = attachingPoint;
        }

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
}
