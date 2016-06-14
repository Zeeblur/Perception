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

    // References to player/navmesh
    private NavMeshAgent nav;         
    
    void Start ()
    {
        enemies = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (enemies != null)
        {
            foreach(GameObject g in enemies)
            {
                g.transform.localPosition += new Vector3(-0.1f, 0.0f, 0.0f);
            }
        }
	
	}
    

    // spawn random enemy
    public void Spawn()
    {
        // rng god decides
        currentType = Random.Range(0, 4);
        GameObject newDog = Instantiate(enemyPrefabs[currentType], new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

        // add parent
        newDog.transform.parent = this.transform;

        // add to list
        enemies.Add(newDog);
    }
}
