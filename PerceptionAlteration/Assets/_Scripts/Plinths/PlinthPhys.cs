using UnityEngine;
using System.Collections;

public class PlinthPhys : MonoBehaviour
{
    // get refs
    private Rigidbody[] plinthBodies;

    private bool dirty = false;

    public float[] masses;
    private float currentMass = 1f;

    public float newTime = 1f;

    // Use this for initialization
    void Start()
    {
        plinthBodies = gameObject.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in plinthBodies)
        {
            rb.centerOfMass = new Vector3(0, -0.3f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dirty)
            return;

        // change mass
        foreach (Rigidbody rb in plinthBodies)
        {
            rb.mass = currentMass;
        }

        dirty = false;

    }

    public void SetState(int choice)
    {
        currentMass = masses[choice - 1];
        dirty = true;
    }

    public void SetPickable(bool big)
    {

        foreach (Rigidbody rb in plinthBodies)
        {
            rb.gameObject.GetComponent<CapsuleCollider>().enabled = big;
        }
    }

}
