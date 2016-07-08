using UnityEngine;
using System.Collections.Generic;

// enemy classification
public enum EnemyType { smallest, small, large, upside };

public class PlinthSpawner : MonoBehaviour
{

    private int currentType = 0; // default 0

    public GameObject[] enemyPrefabs;

    private List<GameObject> enemies;

    // var for hatch
    private HatchMovement hatch;

    private bool listDirty = false;

    // References to player/navmesh
    private NavMeshAgent nav;

    private float timerInt = 10;
    private float timer = 0;

    void Start()
    {
        enemies = new List<GameObject>();
        hatch = GameObject.FindGameObjectWithTag("Hatch").GetComponent<HatchMovement>();

        timer = Time.time + timerInt;
    }

    // Update is called once per frame
    void Update()
    {
        // refresh list if needed. 
        if (listDirty)
        {
            // check through each current spawned dog and delete if inactive/caught
            foreach (GameObject dog in enemies)
            {

                // if the dog is not active delete and remove from list
                if (!dog.activeSelf)
                {
                    enemies.Remove(dog);
                    DestroyObject(dog);
                    break;
                }
            }

            // reset list
            listDirty = false;
        }

        // spawn Timer
        if (Time.time >= timer)
        {
            Spawn();
            timer = Time.time + timerInt;
        }
    }


    // spawn random enemy
    public void Spawn()
    {
        // no doggo for more than 3
        if (currentType > 3)
            return;

        hatch.OpenHatch();

        GameObject newDog = Instantiate(enemyPrefabs[currentType], transform.position, Quaternion.identity) as GameObject;

        // add parent
        newDog.transform.parent = this.transform;
        newDog.AddComponent<SplineWalkerPlinth>();
        newDog.GetComponent<SplineWalkerPlinth>().ChooseDog(currentType);

        // add to list
        enemies.Add(newDog);

        currentType++;
    }

    public void SetDirty() { listDirty = true; }
}
