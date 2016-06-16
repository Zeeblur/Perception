using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour
{
    // enemy classification
    private enum EnemyType { smallest, small, large, upside };
    private int currentType = 0; // default 0

    public GameObject[] enemyPrefabs;

    private List<GameObject> enemies;
    public Spline loopSpline;


    private bool listDirty = false;

    // References to player/navmesh
    private NavMeshAgent nav;         
    
    void Start ()
    {
        enemies = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // refresh list if needed. 
        if (listDirty)
        {
            // check through each current spawned dog and delete if inactive/caught
            foreach(GameObject dog in enemies)
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
        // rng god decides
        currentType = Random.Range(0, 4);
        GameObject newDog = Instantiate(enemyPrefabs[currentType], transform.position, Quaternion.identity) as GameObject;

        // add parent
        newDog.transform.parent = this.transform;
        newDog.AddComponent<SplineWalker>();
        
        // add to list
        enemies.Add(newDog);
    }

    public void SetDirty() { listDirty = true; }
}
