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

    public float sizeValue = 50f;

    private void Start()
    {
        // get script
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Changer>();


        // start play of ambient
        AkSoundEngine.PostEvent("Play_Ambient", this.gameObject);
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
    }


    private void Reset()
    {
    }

    private void Grow()
    {
      //  AkSoundEngine.PostEvent("Play_Ambient", gameObject);
        sizeValue -=  (speed * Time.deltaTime);
        AkSoundEngine.SetRTPCValue("Elevation", sizeValue);
        float value = 0f;
        int rtcpType = 0;
       // GetRTPCValue(string in_pszRtpcName, UnityEngine.GameObject in_gameObjectID, uint in_playingID, out float out_rValue, ref int io_rValueType) {
        AkSoundEngine.GetRTPCValue("Elevation", this.gameObject, 1, out value, ref rtcpType);
        Debug.Log("Chnage sound " + value + " " + sizeValue);

    }
}
