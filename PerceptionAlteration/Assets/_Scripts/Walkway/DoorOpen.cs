using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour
{
    private GameObject doorPane;
    private GameObject topFrame;
    private bool open = false;
    private bool close = false;

    public float speed;

    private Vector3 initialSize;
    private Vector3 initialPosition;

    private float newScale;
    private float newPos;

	void Start ()
    {
        // reference door to be moved
        doorPane = transform.FindChild("Pane").gameObject;
        topFrame = transform.FindChild("Frame").FindChild("TopFrame").gameObject;

        initialPosition = doorPane.transform.localPosition;
        initialSize = doorPane.transform.localScale;

        // get new scale and pos
        newScale = topFrame.transform.localScale.y;
        newPos = topFrame.transform.localPosition.y;
        
	}
	
	void Update ()
    {
        float yPos = doorPane.transform.localPosition.y;
        float yScale = doorPane.transform.localScale.y;

	    if (open)
        {
            yPos = Mathf.Lerp(doorPane.transform.localPosition.y, newPos, speed * Time.deltaTime);
            yScale = Mathf.Lerp(doorPane.transform.localScale.y, newScale, speed * Time.deltaTime);
        }
        

        if (close)
        {
            yPos = Mathf.Lerp(doorPane.transform.localPosition.y, initialPosition.y, speed * Time.deltaTime);
            yScale = Mathf.Lerp(doorPane.transform.localScale.y, initialSize.y, speed * Time.deltaTime);
        }
        
        doorPane.transform.localPosition = new Vector3(initialPosition.x, yPos, initialPosition.z);
        doorPane.transform.localScale = new Vector3(initialSize.x, yScale, initialSize.z);
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Swwwush");

        if (other.gameObject.CompareTag("Player"))
        {
            open = true;
            close = false;

            // openDoor sound
            AkSoundEngine.PostEvent("OpenDoor", this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Unswuh");

        if (other.gameObject.CompareTag("Player"))
        {
            open = false;
            close = true;
        }
    }
}
