using UnityEngine;
using System.Collections;

public class HatchMovement : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OpenHatch()
    {
        // play sound
        Debug.Log("Sound");
        AkSoundEngine.PostEvent("HatchOpen", this.gameObject);
    }
}
