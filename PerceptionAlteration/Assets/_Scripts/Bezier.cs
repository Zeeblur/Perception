using UnityEngine;

public static class Bezier
{
    // Get point returns linear interpolation from points given 
    public static Vector3 GetPoint (Vector3 p0,
        Vector3 p1,
        Vector3 p2,
        float t)
    {
        
        // quadratic Bezier curve -> B(t) = (1-t)^2 P0 + 2(1-t)t P1 + t^2 P2

        //   2(1-t)P0 + 2(1-t)t P1 + t2 P2

        // 2(1-t)(p0 + p1) 

        t = Mathf.Clamp01(t); // clamp t to avoid nan
        float oneMinusT = 1f - t;
        float oneMTSq = oneMinusT * oneMinusT;
        float tSq = t * t;

        return (oneMTSq * p0) + (2f * oneMinusT * t * p1) + (tSq * p2);
    }

    public static Vector3 GetFirstDerivative(Vector3 p0,
        Vector3 p1,
        Vector3 p2,
        float t)
    {
        return (2f * (1 - t) * (p1 - p0)) + 2f * t * (p2 - p1);
    }

    // cubic Bezier
    public static Vector3 GetPoint(Vector3 p0,
        Vector3 p1,
        Vector3 p2,
        Vector3 p3,
        float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;

        // B(t) = (1-t)^3 P0 + 3(1-t)^2t P1 + 3(1-t)t^2 P2 + t^3 P3

        return (oneMinusT * oneMinusT * oneMinusT * p0) +
            (3 * (1 - t)*2 * t * p1) +
            (3 * (1-t) * t* t * p2) +
            (t * t * t * p3);
    }

    public static Vector3 GetFirstDerivative(Vector3 p0,
        Vector3 p1,
        Vector3 p2,
        Vector3 p3,
        float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;

        // B'(t) = 3(1-t)^2(P1 -P0) + 6(1-t)t(P2 - P1) + 3t^2(P3 - P2)
        return (3 * oneMinusT * oneMinusT * (p1 - p0)) +
            (6f * oneMinusT * t * (p2 - p1)) +
            (3f * t * t * (p3 - p2));
    }
}
