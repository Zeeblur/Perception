using UnityEngine;
using System.Collections;

public class SplineWalkerPlinth : MonoBehaviour
{
    public GameObject[] splineGO;
    public Spline[] splines;

    public float duration;
    private float progress;

    public bool lookForward;

    private bool goingForward = true;
    private bool shoot = true;

    // 3 seconds
    private float timeInterval = 3f;
    private float soundTimer = 0f;

    private string chosenDog = "Play_Dog_0";
    private int splineNum = 0;

    public void ChooseDog(int num)
    {
        chosenDog = "Play_Zoe_" + num;
        splineNum = num;
    }

    private void Awake()
    {
        // default
        splineGO = GameObject.FindGameObjectsWithTag("Shoot");

        splines = new Spline[splineGO.Length];

        for (int i = 0; i < splineGO.Length; i++)
        {
            splines[i] = splineGO[i].GetComponent<Spline>();
        }

        lookForward = true;
        duration = 2f;

        soundTimer = Time.time + timeInterval;
    }

    private void Start()
    {
        //if (splineNum ==0)
            AkSoundEngine.PostEvent(chosenDog, this.gameObject);
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
                }

                // stop moving after reached end
                progress = 1f;
                
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
        
        // tell which spline to move dog along
        Move(splines[splineNum]);

        //if (Time.time >= soundTimer)
        //{
        //    // timer for sound play
        //    AkSoundEngine.PostEvent(chosenDog, this.gameObject);
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
