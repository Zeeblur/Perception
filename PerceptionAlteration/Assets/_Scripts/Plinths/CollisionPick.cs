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
        if (other.CompareTag("Perception-Changer-Big"))
        {
            playerScript.Grow();
            transform.parent.GetComponent<PickUp>().SetPicked(false);
            AkSoundEngine.PostEvent("Play_Dog_" + (int)EnemyType.large, other.gameObject);

            //     body.mass = 0.1f;
        }

        if (other.CompareTag("Perception-Changer-Small"))
        { 
            playerScript.Shrink();
            transform.parent.GetComponent<PickUp>().SetPicked(false);
            AkSoundEngine.PostEvent("Play_Dog_" + (int)EnemyType.small, other.gameObject);

        }

        if (other.CompareTag("Perception-Changer-Smallest"))
        {
            playerScript.ShrinkSmaller();
            transform.parent.GetComponent<PickUp>().SetPicked(false);

            AkSoundEngine.PostEvent("Play_Dog_" + (int)EnemyType.smallest, other.gameObject);
        }

        if (other.CompareTag("Perception-Changer-Flip"))
        {
            playerScript.Flip();
            transform.parent.GetComponent<PickUp>().SetPicked(false);
            AkSoundEngine.PostEvent("Play_Dog_" + (int)EnemyType.upside, other.gameObject);
        }
    }
}
