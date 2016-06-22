using UnityEngine;
using System.Collections.Generic;

public class PlinthSpawner : MonoBehaviour
{
    // enemy classification
    private enum EnemyType { smallest, small, large, upside };
    private int currentType = 0; // default 0

    public GameObject[] enemyPrefabs;

    private List<GameObject> enemies;

    // var for hatch
    private HatchMovement hatch;

    private bool listDirty = false;

    // References to player/navmesh
    private NavMeshAgent nav;

    void Start()
    {
        enemies = new List<GameObject>();
        hatch = GameObject.FindGameObjectWithTag("Hatch").GetComponent<HatchMovement>();
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
    }


    // spawn random enemy
    public void Spawn()
    {
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
