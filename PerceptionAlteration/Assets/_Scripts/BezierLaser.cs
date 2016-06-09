using UnityEngine;
using System.Collections;

public class BezierLaser : MonoBehaviour
{
    public BezierCurve curve;

    public GameObject spot;

    // vars for rendering 
    public int frequency;
    public bool lookForward;
    public Transform[] items;

    private LayerMask floorMask;


    private void Awake()
    {      
        floorMask = LayerMask.GetMask("Teleport-able");
        DrawCurve();
    }

    private void DrawCurve()
    {
        // check if any items are added
        if (frequency <= 0 || items == null || items.Length == 0)
            return;

        float stepSize = 1f / (frequency * items.Length);
        for (int p = 0, f = 0; f < frequency; f++)
        {
            for (int i = 0; i < items.Length; i++, p++)
            {
                Transform item = Instantiate(items[i]) as Transform;
                Vector3 position = curve.GetPoint(p * stepSize);
                item.transform.localPosition = position;
                item.transform.parent = transform;
            }
        }
    }

    private void UpdateCurve()
    {
        float stepSize = 1f / (frequency * items.Length);
        for (int p = 0, f = 0; f < frequency; f++)
        {
            for (int i = 0; i < items.Length; i++, p++)
            {
                Vector3 position = curve.GetPoint(p * stepSize);
                items[i].transform.localPosition = position;
                items[i].transform.parent = transform;
            }
        }

    }

	// Use this for initialization
	void Start ()
    {
        // create pointer 
                        
	}


    private RaycastHit currentHit;

	// Update is called once per frame
	void Update ()
    {
        //curve.SetCurveTarget(curve.GetPoint(1f) + new Vector3(1f, 1f, 1f));
   
        //DrawCurve();
        //curve.SetCurveTarget(start, end);

        

        Ray teleRay = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(teleRay, out hit, floorMask))
        {

            if (!HitTooClose(hit))
            {

                Debug.Log("HIT THE FLOOR!");
                curve.SetCurveTarget(hit.point);
                UpdateCurve();
                spot.transform.localPosition = hit.point;

                currentHit = hit;
            }
        }

	}

    private float epsilon  = 2f;

    private bool HitTooClose(RaycastHit newHit)
    {
        if (newHit.point.sqrMagnitude >= currentHit.point.sqrMagnitude + epsilon)
        {
            return false;
        }

        if (newHit.point.sqrMagnitude <= currentHit.point.sqrMagnitude - epsilon)
        {
            return false;
        }

        return true;
    }
}
