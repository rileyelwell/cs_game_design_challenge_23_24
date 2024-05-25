using System;
using UnityEngine;

public class RaycastListener : MonoBehaviour
{
    public Ray rayComponent;
    private PlayerController playercontroller;

    void Awake()
    {
        if (rayComponent == null)
        {
            rayComponent = FindObjectOfType<Ray>();
            if (rayComponent == null)
            {
                Debug.LogError("Ray component not found in the scene.");
            }
        }

        playercontroller = gameObject.GetComponentInParent<PlayerController>();
    }

    void OnEnable()
    {
        if (rayComponent != null)
        {
            rayComponent.OnRaycastResultsUpdated += HandleRaycastResultsUpdated;
        }
    }

    void OnDisable()
    {
        if (rayComponent != null)
        {
            rayComponent.OnRaycastResultsUpdated -= HandleRaycastResultsUpdated;
        }
    }

    private void HandleRaycastResultsUpdated(object sender, RaycastResultsUpdatedEventArgs e)
    {
        RaycastHit[,] results = e.Results;
        bool foundHit = false;
        // Process the results as needed
        //Debug.Log("Raycast results received:");
        for (int i = 0; i < results.GetLength(0); i++)
        {
            for (int j = 0; j < results.GetLength(1); j++)
            {
                if (results[i, j].collider != null)
                {
                    Debug.Log($"Hit {results[i, j].collider.name} at ({i}, {j})");
                    playercontroller.GetWarnings(i, j);
                    foundHit = true;
                }
            }
        }
        if (!foundHit)
            playercontroller.ResetSensorWarnings();
        
    }
}
