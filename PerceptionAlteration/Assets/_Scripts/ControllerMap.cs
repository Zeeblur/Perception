using UnityEngine;
using System.Collections;

public class ControllerMap : MonoBehaviour {
    
	private Valve.VR.EVRButtonId gripBtn = Valve.VR.EVRButtonId.k_EButton_Grip;

    private GameObject player;
    private UserCollisionDetection playerScript;

	public bool gripDown = false;
	public bool gripUp = false;
	public bool gripPressed = false;

	private Valve.VR.EVRButtonId triggerBtn = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	public bool triggerDown = false;
	public bool triggerUp = false;
	public bool triggerPressed = false;

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device myController { get { return SteamVR_Controller.Input((int)trackedObj.index); }}

	// Use this for initialization
	void Start ()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<UserCollisionDetection>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// check if controller is null or not first
		if (myController == null)
		{
			Debug.Log ("Controller not found!!");
			return;
		}

		gripDown = myController.GetPressDown (gripBtn);
        gripUp = myController.GetPressUp(gripBtn);
        gripPressed = myController.GetPress(gripBtn);

        triggerDown = myController.GetPressDown(triggerBtn);
        triggerUp = myController.GetPressUp(triggerBtn);
        triggerPressed = myController.GetPress(triggerBtn);

        if (gripDown)
        {
            Debug.Log("Trigger down");
            playerScript.CurrentScale = scaleMode.resetting;
            playerScript.Teleport();
        }

	}

    // collider check 
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("box!");
        if (other.gameObject.CompareTag("SoundPlayer"))
        {
            Debug.Log("Sound");
            // change to red
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }
}
