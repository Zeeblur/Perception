using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    bool silent = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // collision detection
    void OnTriggerStay(Collider other)
    {
        // if other collider is plinth
        if (other.CompareTag("DogZone"))
        {
            Debug.Log("NextToDOg");
            PlayDog(int.Parse(other.gameObject.name));           
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DogZone"))
        {
            Debug.Log("Awayfro,mToDOg");
            StopDog(int.Parse(other.gameObject.name));
        }
    }

    private void PlayDog(int chosenDog)
    {
        GameObject dog = GameObject.FindGameObjectWithTag("Perception-Changer-" + (EnemyType)chosenDog);

        if (dog && silent)
        {
            silent = false;
            AkSoundEngine.PostEvent("Play_" + (EnemyType)chosenDog, dog);
        }

    }

    private void StopDog(int chosenDog)
    {
        GameObject dog = GameObject.FindGameObjectWithTag("Perception-Changer-" + (EnemyType)chosenDog);

        if (dog && !silent)
        {
            silent = true;
            AkSoundEngine.PostEvent("Stop_" + (EnemyType)chosenDog, dog);
        }

    }


}
