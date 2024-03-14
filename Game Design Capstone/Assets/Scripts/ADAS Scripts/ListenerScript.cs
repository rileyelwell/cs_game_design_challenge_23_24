using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerScript : MonoBehaviour
{
    private Ray raycastScript;  // Reference to RaycastScript

    void Start()
    {
        // Get reference to RaycastScript (using chosen method from step 1)
        raycastScript = GetComponent<Ray>();  // Example (assuming both scripts on same GameObject)

        // Subscribe to the event
        raycastScript.OnRaycastResultsUpdated += OnRaycastResultsReceived;
    }

    private void OnRaycastResultsReceived(object sender, RaycastResultsUpdatedEventArgs e)
    {
        // Process the received results (e.Results)
        Debug.Log("Raycast results updated! Processing data...");
    }
}

