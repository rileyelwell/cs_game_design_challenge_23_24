using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignSemisphere : MonoBehaviour
{
    public GameObject playerReference; // Assign the player object in the Inspector

    void Update()
    {
        if (playerReference != null)
        {
            Transform playerTransform = playerReference.transform;

            // Check if the required components are attached
            if (GetComponent<MeshRenderer>() != null && GetComponent<MeshFilter>() != null)
            {
                transform.position = playerTransform.position;

                // Optional: Set the semisphere's rotation to face the player's forward direction
                transform.rotation = Quaternion.LookRotation(playerTransform.forward);
            }
            else
            {
                Debug.LogError("Semisphere object is missing MeshRenderer or MeshFilter component!");
            }
        }
        else
        {
            Debug.LogError("playerReference is not assigned! Please set it in the Inspector.");
        }
    }
}