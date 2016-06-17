using UnityEngine;
using System.Collections;
public enum SplineWalkerMode { Once, Loop, PingPong }

public class SplineWalker : MonoBehaviour
{
    public Spline splineTarget;
    public Spline shootSpline;

    public float duration;
    private float progress;

    public bool lookForward;

    public SplineWalkerMode mode;

    private bool goingForward = true;
    private bool shoot = true;

    private void Awake()
    {
        // default
        shootSpline = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Spline>();
        splineTarget = GameObject.FindGameObjectWithTag("Loop").GetComponent<Spline>();
        lookForward = true;
        mode = SplineWalkerMode.Loop;
        duration = 2f;
    }


    private void Update()
    {
        if (goingForward)
        {
            progress += Time.deltaTime / duration;
            if (progress > 1f)
            {
                if (shoot)
                {
                    shoot = false;
                    progress -= 0f;
                    duration = 10f;
                }

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
              //  -1.600769 -1.038768 0.5232809
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

        if (shoot)
        {
            Move(shootSpline);
        }
        else
        {
            Move(splineTarget);
        }


    }

    private void Move(Spline chosenSpline)
    {
        Vector3 position = chosenSpline.GetPoint(progress);
        transform.localPosition = position;
        if (lookForward)
        {
            transform.LookAt(position + chosenSpline.GetDirection(progress));
        }
    }

}
