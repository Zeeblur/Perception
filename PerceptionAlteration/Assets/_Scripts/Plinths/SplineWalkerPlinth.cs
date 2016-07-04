using UnityEngine;
using System.Collections;

public class SplineWalkerPlinth : MonoBehaviour
{
    public GameObject[] splineGO;
    public Spline[] splines;

    public float duration;
    private float progress;

    public bool lookForward;

    // 3 seconds
    private float timeInterval = 3f;
    private float soundTimer = 0f;

    private string chosenDog = "Play_Dog_0";
    private int splineNum = 0;

    public float angle = 180f;
    private Vector3 zAxis = new Vector3(0f, 0f, 1f);

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
       // AkSoundEngine.PostEvent(chosenDog, this.gameObject);
    }


    private void Update()
    {
        if (progress > 1)
            return;

        progress += Time.deltaTime / duration;
  
        // tell which spline to move dog along
        Move(splines[splineNum]);
    }

    private void Move(Spline chosenSpline)
    {
        Vector3 position = chosenSpline.GetPoint(progress);
        transform.localPosition = position;
        if (lookForward)
        {
            transform.LookAt(position + chosenSpline.GetDirection(progress));

            // ensure flip dog is upside down
            if (this.tag == "Perception-Changer-Flip")
            {
                transform.Rotate(zAxis, angle);
            }
        }
    }

}
