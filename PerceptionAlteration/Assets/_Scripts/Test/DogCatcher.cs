using UnityEngine;
using System.Collections;

public class DogCatcher : MonoBehaviour
{
    public GameObject outerRing;


    // var for ref to script
    private GameObject spawner;

    private GameObject[] catcherLights;

    private int currentLightIndex = 0;

	// Use this for initialization
	void Awake ()
    {
        catcherLights = new GameObject[12];
        catcherLights = GameObject.FindGameObjectsWithTag("PointLight DogCatcher");
        spawner = GameObject.FindGameObjectWithTag("Respawn");
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        // when ball is caught rmeove from play + flash light
        other.gameObject.SetActive(false);

        // renderer of current light
        Renderer rend = catcherLights[currentLightIndex].gameObject.GetComponent<Renderer>();

        // get colour of dog
        Color newColour = other.gameObject.GetComponent<Renderer>().material.color;

        //change Emissive colour to colour of dog
        rend.material.SetColor("_EmissionColor", newColour);
        GameObject lightGO = catcherLights[currentLightIndex].gameObject.transform.GetChild(0).gameObject;

        lightGO.GetComponent<Light>().color = newColour;
        lightGO.SetActive(true);

        // if last light start at beginning again
        currentLightIndex = (currentLightIndex == catcherLights.Length - 1) ? 0 : currentLightIndex + 1;

        spawner.GetComponent<EnemyGenerator>().SetDirty();

    }
}
