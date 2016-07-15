using UnityEngine;
using System.Collections;

public class RotatingCube : MonoBehaviour
{
    public float speed;

    void Start ()
    {
        AkSoundEngine.PostEvent("Play_Cube", this.gameObject);
    }

    void Update ()
    {
        transform.Rotate(new Vector3(0, 1, 0), speed * Time.deltaTime);
    }
}
