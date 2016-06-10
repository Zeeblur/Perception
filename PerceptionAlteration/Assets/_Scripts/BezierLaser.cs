using UnityEngine;
using System.Collections;

public class BezierLaser : MonoBehaviour
{
    public BezierCurve curve;

    public GameObject spot;

    // vars for rendering 
    public int frequency;
    public bool lookForward;
    public Transform prefab;

    private Transform[] sections;
    private LayerMask floorMask;

    private RaycastHit currentHit;
    private float epsilon = 2f;

    private void Awake()
    {
        sections = new Transform[frequency];
        floorMask = LayerMask.GetMask("Teleport-able");
        DrawCurve();
    }

    private void DrawCurve()
    {
        // check if any items are added
        if (frequency <= 0 || prefab == null)
            return;

        float stepSize = 1f / frequency;
        for (int p = 0, f = 0; f < frequency; f++, p++)
        {
            Transform item = Instantiate(prefab) as Transform;
            Vector3 position = curve.GetPoint(p * stepSize);
            item.transform.localPosition = position;
            item.transform.parent = transform;

            sections[p] = item;
        }
    }

    private bool del = false;

    private void UpdateCurve()
    {

        if (!del)
        {
            // delete
            for (int i = 0; i < sections.Length; i++)
            {
                Destroy(sections[i].gameObject);
            }

            del = true;


            float stepSize = 1f / frequency;
            for (int p = 0, f = 0; f < frequency; f++, p++)
            {
                Transform item = Instantiate(prefab) as Transform;
                Vector3 position = curve.GetPoint(p * stepSize);
                item.transform.localPosition = position;
                item.transform.parent = transform;

                sections[p] = item;
            }
        }

    }



	// Update is called once per frame
	void Update ()
    {      

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
