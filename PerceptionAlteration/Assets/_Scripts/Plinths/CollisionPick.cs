using UnityEngine;
using System.Collections;

public class CollisionPick : MonoBehaviour
{
    private Changer playerScript;
    private Rigidbody body;

	void Start ()
    {
        // get reference to script on player
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Changer>();
        body = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Perception-Changer-large"))
        {
            playerScript.Grow();
            transform.parent.GetComponent<PickUp>().SetPicked(false);
            AkSoundEngine.PostEvent("Play_Dog_" + (int)EnemyType.large, other.gameObject);
            other.gameObject.GetComponent<SplineWalkerPlinth>().DogState = true;
        }

        if (other.CompareTag("Perception-Changer-small"))
        { 
            playerScript.Shrink();
            transform.parent.GetComponent<PickUp>().SetPicked(false);
            AkSoundEngine.PostEvent("Play_Dog_" + (int)EnemyType.small, other.gameObject);
            other.gameObject.GetComponent<SplineWalkerPlinth>().DogState = true;
        }

        if (other.CompareTag("Perception-Changer-smallest"))
        {
            playerScript.ShrinkSmaller();
            transform.parent.GetComponent<PickUp>().SetPicked(false);

            AkSoundEngine.PostEvent("Play_Dog_" + (int)EnemyType.smallest, other.gameObject);
            other.gameObject.GetComponent<SplineWalkerPlinth>().DogState = true;
            other.gameObject.GetComponent<SplineWalkerPlinth>().SoundTimer = Time.time + 3f;
        }

        if (other.CompareTag("Perception-Changer-upside"))
        {
            playerScript.Flip();
            transform.parent.GetComponent<PickUp>().SetPicked(false);
            AkSoundEngine.PostEvent("Play_Dog_" + (int)EnemyType.upside, other.gameObject);
            other.gameObject.GetComponent<SplineWalkerPlinth>().DogState = true;
        }
    }
}
