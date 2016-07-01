using UnityEngine;
using System.Collections;

public class body : MonoBehaviour
{
    public GameObject cap;
    private Transform capTrans;
    private Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        capTrans = cap.transform;
        offset = new Vector3(0f, -0.25f, 0);

    }
	
	// Update is called once per frame
	void Update ()
    {
        capTrans = cap.transform;
        transform.position = capTrans.position + offset;
	}
}
