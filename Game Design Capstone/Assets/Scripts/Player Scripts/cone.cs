using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cone : MonoBehaviour
{

    // This function is called when another object enters the trigger collider attached to this object.
    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has entered the trigger.");
            // Perform actions here, such as changing the scene, playing a sound, etc.
        }
        else
        {
            Debug.Log("An object has entered the trigger, but it's not the player.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get all MeshCollider components in the children of the GameObject this script is attached to
        MeshCollider[] childMeshColliders = GetComponentsInChildren<MeshCollider>();

        if (childMeshColliders.Length >  0)
        {
            Debug.Log("Found " + childMeshColliders.Length + " MeshCollider components in child GameObjects.");
            // You can now iterate over childMeshColliders to perform actions with each MeshCollider
            foreach (MeshCollider meshCollider in childMeshColliders)
            {
                // Perform actions with meshCollider
                Debug.Log("MeshCollider found: " + meshCollider.name);
            }
        }
        else
        {
            Debug.Log("No MeshCollider components found in any child GameObject.");
        }
    }
}

