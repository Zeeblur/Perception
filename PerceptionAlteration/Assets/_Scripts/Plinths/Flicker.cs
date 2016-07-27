using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour {

    // sound
    // https://freesound.org/people/mmaruska/sounds/232447/

    private Light[] spot;
    private Renderer[] emissive;
    private Color baseColour;

    private bool flicker = false;

    private float timer = 0;
    private float timerInt = 5;

    private float flashTime = 0;
    private float flashTimeInt = 2;

    // Use this for initialization
    void Awake ()
    {
        spot = GetComponentsInChildren<Light>();
        emissive = gameObject.GetComponentsInChildren<Renderer>();
    }

    void Start ()
    {
        timer = Time.time + timerInt;
        flashTime = Time.time + flashTimeInt;

        AkSoundEngine.PostEvent("Play_Lighthum", this.gameObject);

        foreach (Renderer r in emissive)
        {
            r.material.EnableKeyword("_EMISSION");
            baseColour = r.material.GetColor("_Color");
        }
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

            if (flashTimeInt >= 1)
                AkSoundEngine.PostEvent("Play_flickerLong", this.gameObject);
            else
                AkSoundEngine.PostEvent("Play_flicker", this.gameObject);

        }

        if (flicker)
        {
            float newIntensity =  Random.Range(spot[0].intensity - 1, spot[0].intensity + 0.5f);

            foreach (Light s in spot)
            {
                s.intensity = newIntensity;
            }
            

            Color emissiveCol = baseColour * Mathf.LinearToGammaSpace(spot[0].intensity);

            foreach (Renderer r in emissive)
            {
                r.material.SetColor("_EmissionColor", emissiveCol);
            }

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
            foreach (Light s in spot)
            {
                s.intensity = 1f;
            }

            foreach (Renderer r in emissive)
            {
                r.material.SetColor("_EmissionColor", baseColour);
            }
        }
        
	}
}
