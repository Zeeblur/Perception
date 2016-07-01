using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{    
    // script to find user state
    private GameObject player;
    private Changer playerScript;

    public float speed = 1.5f;
    private float ep = 0.5f;

    private scaleMode soundMode;

    public float normalValue = 50f;
    public float flipValue;
    public float smallestValue;
    public float smallValue;
    public float bigValue;

    private float currentVal = 50f;

    private void Start()
    {
        // get script
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Changer>();


        // start play of ambient
        //AkSoundEngine.PostEvent("Play_Ambient", this.gameObject);
    }

    private void Update()
    {
            
        // reset current sound mode
        soundMode = playerScript.CurrentScale;

        float lerpTarget = currentVal;

        switch (soundMode)
        {
            case scaleMode.shrinking:
                Debug.Log("shrink");
                lerpTarget = smallValue;
                break;
            
            case scaleMode.resetting:
                Debug.Log("reset");
                lerpTarget = normalValue;
                break;

            case scaleMode.growing:
                // Grow();
                Debug.Log("Grow");
                lerpTarget = bigValue;
                break;

            case scaleMode.shrinkingSmaller:
                Debug.Log("shrinkest");
                lerpTarget = smallestValue;
                break;

            case scaleMode.turning:
                Debug.Log("flip");
                lerpTarget = flipValue;
                break;
        }

        currentVal = Mathf.Lerp(currentVal, lerpTarget, speed * Time.deltaTime);
     //   AkSoundEngine.SetRTPCValue("Elevation", currentVal);
        //Debug.Log("curr " + currentVal);

    }
}
