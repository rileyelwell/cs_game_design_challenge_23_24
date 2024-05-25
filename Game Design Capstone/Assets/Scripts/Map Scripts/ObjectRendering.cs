using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRendering : MonoBehaviour
{
    [SerializeField] private float maxDistance = 50f; // Maximum render distance
    [SerializeField] private Transform visualContainer; // Reference to a child object/container with Renderer components
    private Transform player; // Reference to the player (main camera)

    
    private void Start() {
        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).transform;
    }

    void Update()
    {
        // Calculate the distance between the pedestrian and the player (main camera)
        float distance = Vector3.Distance(transform.position, player.position);

        // Enable/disable renderers of child objects based on distance
        if (visualContainer != null)
        {
            if (visualContainer.parent.gameObject.tag == TagManager.PEDESTRIAN_TAG)
                visualContainer.GetComponent<Renderer>().enabled = distance <= maxDistance;
            else if (visualContainer.parent.gameObject.tag == TagManager.VEHICLE_TAG)
            {
                visualContainer.GetComponent<Renderer>().enabled = distance <= maxDistance;
                foreach (Renderer renderer in visualContainer.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = distance <= maxDistance;
                }
            }
            else 
            {
                visualContainer.GetComponent<Renderer>().enabled = distance <= maxDistance;
            }
                
            
        }
    }
}
