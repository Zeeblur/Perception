using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private bool silent = true;
    private bool large = false;

    public bool Large
    {
        set { large = value; }
    }

    // collision detection
    void OnTriggerStay(Collider other)
    {
        if (large)
            return;

        // if other collider is plinth
        if (other.CompareTag("DogZone"))
        {
            Debug.Log("NextToDOg");
            PlayDog(int.Parse(other.gameObject.name));           
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (large)
            return;

        if (other.CompareTag("DogZone"))
        {
            Debug.Log("AwayfromDog");
            StopDog(int.Parse(other.gameObject.name));
        }
    }

    private void PlayDog(int chosenDog)
    {
        GameObject dog = GameObject.FindGameObjectWithTag("Perception-Changer-" + (EnemyType)chosenDog);

        if (dog && silent)
        {
            silent = false;
            AkSoundEngine.PostEvent("Play_Breathing_" + (EnemyType)chosenDog, dog);
        }

    }

    private void StopDog(int chosenDog)
    {
        GameObject dog = GameObject.FindGameObjectWithTag("Perception-Changer-" + (EnemyType)chosenDog);

        if (dog && !silent)
        {
            silent = true;
            AkSoundEngine.PostEvent("Stop_Breathing_" + (EnemyType)chosenDog, dog);
        }

    }


}
