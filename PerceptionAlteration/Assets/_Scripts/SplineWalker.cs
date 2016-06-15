using UnityEngine;
using System.Collections;
public enum SplineWalkerMode { Once, Loop, PingPong }

public class SplineWalker : MonoBehaviour
{
    public Spline splineTarget;

    public float duration;
    private float progress;

    public bool lookForward;

    public SplineWalkerMode mode;

    private bool goingForward = true;

    private void Update()
    {
        if (goingForward)
        {
            progress += Time.deltaTime / duration;
            if (progress > 1f)
            {
                switch(mode)
                {
                    case SplineWalkerMode.Once:
                        progress = 1f;
                        break;
                    case SplineWalkerMode.Loop:
                        progress -= 1f;
                        break;
                    case SplineWalkerMode.PingPong:
                        progress = 2f - progress;
                        goingForward = false;
                        break;

                }
                
            }
        }
        else
        {
            progress -= Time.deltaTime / duration;
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
            }
        }

        Vector3 position = splineTarget.GetPoint(progress);
        transform.localPosition = position;
        if (lookForward)
        {
            transform.LookAt(position + splineTarget.GetDirection(progress));
        }
    }

}
