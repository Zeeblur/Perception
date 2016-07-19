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

    void Update ()
    {

        if (Input.GetKey("1"))
        {
            playerScript.ShrinkSmaller();
            Selected((int)EnemyType.smallest, GameObject.FindGameObjectWithTag("Perception-Changer-smallest"));
        }

        if (Input.GetKey("2"))
        {
            playerScript.Shrink();
            Selected((int)EnemyType.small, GameObject.FindGameObjectWithTag("Perception-Changer-small"));
        }


        if (Input.GetKey("3"))
        {
            playerScript.Grow();
            Selected((int)EnemyType.large, GameObject.FindGameObjectWithTag("Perception-Changer-large"));
        }
        
        if (Input.GetKey("4"))
        {
            playerScript.Flip();
            Selected((int)EnemyType.upside, GameObject.FindGameObjectWithTag("Perception-Changer-upside"));
        }

        if (Input.GetKey("5"))
        {
            playerScript.Reset();
        }

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
        if (transform.parent)
            transform.parent.GetComponent<PickUp>().SetPicked(false);


        AkSoundEngine.PostEvent("Play_Dog_" + chosenDog, GO);
        GO.gameObject.GetComponent<SplineWalkerPlinth>().DogState = true;
        GO.gameObject.GetComponent<SplineWalkerPlinth>().SoundTimer = Time.time + 3f;

        AkSoundEngine.PostEvent("Magic", this.gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (Time.time < 3f)
            return;

        // bang when colliding with anything but player
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("GameController"))
        {
            AkSoundEngine.PostEvent("Play_Cube_Col", this.gameObject);
        }
    }
}
