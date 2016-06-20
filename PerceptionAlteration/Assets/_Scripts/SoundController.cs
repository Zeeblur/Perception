using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{

    private AudioReverbFilter reverb;
    private AudioLowPassFilter LPF;
    private AudioHighPassFilter HPF;

    public float initialRoomSize;
    public float maxRoomSize;

    public float initialLPF;
    public float lowestLPF;

    public float initialHPF;
    public float highestHPF;
    

    // script to find user state
    private GameObject player;
    private UserCollisionDetection playerScript;

    public float speed;
    private float ep = 0.5f;

    private scaleMode soundMode;

    private void Start()
    {
        // get script
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<UserCollisionDetection>();

        reverb = GetComponent<AudioReverbFilter>();
        LPF = GetComponent<AudioLowPassFilter>();
        HPF = GetComponent<AudioHighPassFilter>();
    }

    private void Update()
    {
        // reset current sound mode
        if (soundMode == scaleMode.stopped && playerScript.CurrentScale != scaleMode.stopped)
        {
            soundMode = playerScript.CurrentScale;
        }

        switch (soundMode)
        {
            case scaleMode.shrinking:
                Debug.Log("STart shrinking");
                Shrink();
                break;
            
            case scaleMode.resetting:
                Reset();
                break;

            case scaleMode.growing:
                Grow();
                break;
        }

    }

    private void Shrink()
    {
        reverb.room = Mathf.Lerp(reverb.room, maxRoomSize, speed * Time.deltaTime);

        LPF.cutoffFrequency = Mathf.Lerp(LPF.cutoffFrequency, lowestLPF, speed * Time.deltaTime);

        if (reverb.room >= maxRoomSize - ep)
        {
            Debug.Log("shinking sound stopped");
            soundMode = scaleMode.stopped;
        }
    }


    private void Reset()
    {
        LPF.cutoffFrequency = Mathf.Lerp(LPF.cutoffFrequency, initialLPF, speed * Time.deltaTime);
        reverb.room = Mathf.Lerp(reverb.room, initialRoomSize, speed * Time.deltaTime);
        HPF.cutoffFrequency = Mathf.Lerp(HPF.cutoffFrequency, initialHPF, speed * Time.deltaTime);

        if (reverb.room <= initialRoomSize + ep)
        {
            Debug.Log("restting stopped");
            soundMode = scaleMode.stopped;
        }
    }

    private void Grow()
    {
        HPF.cutoffFrequency = Mathf.Lerp(HPF.cutoffFrequency, highestHPF, speed * Time.deltaTime);

        if (HPF.cutoffFrequency >= highestHPF - ep)
        {
            Debug.Log("growing stopped");
            soundMode = scaleMode.stopped;
        }
    }
}
