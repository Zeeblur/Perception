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


    private Transform[] sections;
    private LayerMask floorMask;


    private void Awake()
    {
        sections = new Transform[frequency];
        floorMask = LayerMask.GetMask("Teleport-able");
        DrawCurve();
    }

    private void DrawCurve()
    {
        // check if any items are added
        if (frequency <= 0 || items == null || items.Length == 0)
            return;

        float stepSize = 1f / frequency;
        for (int p = 0, f = 0; f < frequency; f++, p++)
        {
            Transform item = Instantiate(items[0]) as Transform;
            Vector3 position = curve.GetPoint(p * stepSize);
            item.transform.localPosition = position;
            item.transform.parent = transform;

            sections[p] = item;
        }
    }

    private void UpdateCurve()
    {
        sections[4].gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].transform.localPosition = curve.GetPoint(i * (1f / frequency));
        }

        //float newStepSize = 1f / frequency;
        //for (int i = 0; i < sections.Length; i++)
        //{
        //    Debug.Log("Curve get point " + i + ": " + curve.GetPoint(i * newStepSize));
        //    sections[i].transform.localPosition = sections[i].transform.worldToLocalMatrix * curve.GetPoint(i * newStepSize);// curve.GetPoint(i * newStepSize);
        //    //sections[i].transform.parent = transform;

        //    if (i == 2)
        //    {
        //        sections[i].transform.localPosition = sections[i].transform.InverseTransformPoint(curve.GetPoint(i * newStepSize));
        //    }

        //    if (i == 3)
        //    {
        //        sections[i].transform.localPosition = sections[i].transform.TransformPoint(curve.GetPoint(i * newStepSize));
        //    }

        //    if (i == 1)
        //    {
        //        sections[i].transform.localPosition = currentHit.point;
        //    }

        //}
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
                Debug.Log("Curve start: " + curve.points[0]);
                Debug.Log("Curve end: " + curve.points[2]);
                curve.SetCurveTarget(hit.point);
                Debug.Log("Curve start: " + curve.points[0]);
                Debug.Log("Curve end: " + curve.points[2]);
                spot.transform.localPosition = hit.point;

                currentHit = hit;
                UpdateCurve();
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
