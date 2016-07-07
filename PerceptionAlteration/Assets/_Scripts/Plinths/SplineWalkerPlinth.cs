using UnityEngine;
using System.Collections;

public class SplineWalkerPlinth : MonoBehaviour
{
    // CC dog noises
    // https://www.freesound.org/people/smokum/sounds/89213/
    // https://www.freesound.org/people/nielstii/sounds/346320/
    // https://www.freesound.org/people/felix.blume/sounds/199261/
    // https://www.freesound.org/people/Robinhood76/sounds/327813/


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

    private GameObject player;

    public void ChooseDog(int num)
    {
        chosenDog = "Play_Dog_" + num;
        splineNum = num;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

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
        AkSoundEngine.PostEvent(chosenDog, this.gameObject);
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

    private void CheckSwitch()
    {

        // find distance squared (no need for expensive sqrt) of world position
        Vector3 play = player.transform.position;
        Vector3 dog = this.transform.position;

        float distance = ((play.x - dog.x) * (play.x - dog.x) +
            (play.y - dog.y) * (play.y - dog.y) +
            (play.z - dog.z) * (play.z - dog.z));


        if (distance <= 2f)
        {
            AkSoundEngine.SetSwitch("DistanceDog", "VNear", gameObject);
            Debug.Log("VNEAR");
        }
        else if (distance >= 2f && distance <= 5f)
        {
            AkSoundEngine.SetSwitch("DistanceDog", "Near", gameObject);
            Debug.Log("near");
        }
        else if (distance >= 5f)
        { 
            AkSoundEngine.SetSwitch("DistanceDog", "Far", gameObject);
            Debug.Log("far");
        }
    }
}
