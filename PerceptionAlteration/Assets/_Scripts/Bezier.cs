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

        t = Mathf.Clamp01(t); // clamp t to avoid nan
        float oneMinusT = 1f - t;
        float oneMTSq = oneMinusT * oneMinusT;
        float tSq = t * t;

        return (oneMTSq * p0) + (2f * oneMinusT * t * p1) + (tSq * p2);
    }
}
