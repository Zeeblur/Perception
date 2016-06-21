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

    // 3 seconds
    private float timeInterval = 3f;
    private float soundTimer = 0f;

    private void Awake()
    {
        // default
        shootSpline = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Spline>();
        splineTarget = GameObject.FindGameObjectWithTag("Loop").GetComponent<Spline>();
        lookForward = true;
        mode = SplineWalkerMode.Loop;
        duration = 2f;

        soundTimer = Time.time + timeInterval;
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
                    duration = 10f;
                    AkSoundEngine.PostEvent("Play_Dog_0", this.gameObject);
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

        //if (Time.time >= soundTimer)
        //{

        //    AkSoundEngine.PostEvent("Play_Dog_0", this.gameObject);
        //    soundTimer = Time.time + timeInterval;
        //}
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
