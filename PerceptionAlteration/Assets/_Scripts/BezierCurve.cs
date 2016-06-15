using UnityEngine;
using System.Collections;

public class BezierCurve : MonoBehaviour
{
    // Class used to create Bezier Curve paths

    public Vector3[] points;

    // reset initialises new curve with 3 points
    public void Reset ()
    {
        points = new Vector3[] {
            new Vector3(-1f, 0f, 0f),
            new Vector3(-2f, 0f, 0f),
            new Vector3(-3f, 0f, 0f),
            new Vector3(-4f, 0f, 0f)
        };
    }

    // return point lerped
    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(
            Bezier.GetPoint(
                points[0],
                points[1],
                points[2],
                points[3],
                t));
    }

    public Vector3 GetVelocity (float t)
    {
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0],
            points[1],
            points[2], 
            points[3],
            t)) - transform.position;
    }

    public Vector3 GetDirection (float t)
    {
        return GetVelocity(t).normalized;
    }

    public void SetCurveTarget(Vector3 end)
    {
        // changes target point (end is world space)
        points[2] = transform.InverseTransformPoint(end);
    }

}
