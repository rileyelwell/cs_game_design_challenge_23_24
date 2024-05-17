using System;
using UnityEngine;

public class RaycastListener : MonoBehaviour
{
    public Ray rayComponent;

    void OnEnable()
    {
        if (rayComponent != null)
        {
            rayComponent.OnRayFUpdated += HandleRayFUpdated;
            rayComponent.OnRayLUpdated += HandleRayLUpdated;
            rayComponent.OnRayRUpdated += HandleRayRUpdated;
            rayComponent.OnRayBUpdated += HandleRayBUpdated;
        }
    }

    void OnDisable()
    {
        if (rayComponent != null)
        {
            rayComponent.OnRayFUpdated -= HandleRayFUpdated;
            rayComponent.OnRayLUpdated -= HandleRayLUpdated;
            rayComponent.OnRayRUpdated -= HandleRayRUpdated;
            rayComponent.OnRayBUpdated -= HandleRayBUpdated;
        }
    }

    private void HandleRayFUpdated(object sender, RaycastResultsUpdatedEventArgs e)
    {
        ProcessResults("Front", e.Results);
    }

    private void HandleRayLUpdated(object sender, RaycastResultsUpdatedEventArgs e)
    {
        ProcessResults("Left", e.Results);
    }

    private void HandleRayRUpdated(object sender, RaycastResultsUpdatedEventArgs e)
    {
        ProcessResults("Right", e.Results);
    }

    private void HandleRayBUpdated(object sender, RaycastResultsUpdatedEventArgs e)
    {
        ProcessResults("Back", e.Results);
    }

    private void ProcessResults(string direction, RaycastHit[,] results)
    {
        Debug.Log($"{direction} Raycast results received:");
        for (int i = 0; i < results.GetLength(0); i++)
        {
            for (int j = 0; j < results.GetLength(1); j++)
            {
                if (results[i, j].collider != null)
                {
                    Debug.Log($"Hit {results[i, j].collider.name} at ({i}, {j})");
                }
            }
        }
    }
}
