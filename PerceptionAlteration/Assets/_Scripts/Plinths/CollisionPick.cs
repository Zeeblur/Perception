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
            Selected((int)EnemyType.large, other.gameObject);
        }

        if (other.CompareTag("Perception-Changer-small"))
        { 
            playerScript.Shrink();
            Selected((int)EnemyType.small, other.gameObject);
        }

        if (other.CompareTag("Perception-Changer-smallest"))
        {
            playerScript.ShrinkSmaller();
            Selected((int)EnemyType.smallest, other.gameObject);
        }

        if (other.CompareTag("Perception-Changer-upside"))
        {
            playerScript.Flip();
            Selected((int)EnemyType.upside, other.gameObject);
        }
    }

    private void Selected(int chosenDog, GameObject GO)
    {
        transform.parent.GetComponent<PickUp>().SetPicked(false);
        AkSoundEngine.PostEvent("Play_Dog_" + chosenDog, GO);
        GO.gameObject.GetComponent<SplineWalkerPlinth>().DogState = true;
        GO.gameObject.GetComponent<SplineWalkerPlinth>().SoundTimer = Time.time + 3f;

        AkSoundEngine.PostEvent("Magic", this.gameObject);
    }
}
