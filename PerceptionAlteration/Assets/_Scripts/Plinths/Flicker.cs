using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour {

    // sound
    // https://freesound.org/people/mmaruska/sounds/232447/

    private Light spot;

    private bool flicker = false;

    private float timer = 0;
    private float timerInt = 5;

    private float flashTime = 0;
    private float flashTimeInt = 2;

    // Use this for initialization
    void Awake ()
    {
        spot = GetComponent<Light>();
    }

    void Start ()
    {
        timer = Time.time + timerInt;
        flashTime = Time.time + flashTimeInt;
    }

    // Update is called once per frame
    void Update ()
    {
        // start flickering (main timer)
        if (!flicker && Time.time >= timer)
        {
            // reset timers about to flicker.
            flashTimeInt = Random.Range(0.5f, 2f);

            flicker = true;
            timer = Time.time + timerInt;
            flashTime = Time.time + flashTimeInt;
        }

        if (flicker)
        {
            spot.intensity = Random.Range(spot.intensity-1, spot.intensity+0.5f);

            // flashTimer
            if (Time.time >= flashTime)
            {
                timerInt = Random.Range(3f, 10f);

                flicker = false;
                timer = Time.time + timerInt;
                flashTime = Time.time + flashTimeInt;
            }
        }
        else
        {
            // reset
            spot.intensity = 2.2f;
        }
        
	}
}
