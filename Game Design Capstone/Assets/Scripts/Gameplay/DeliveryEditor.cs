using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryEditor : MonoBehaviour
{
    [SerializeField] private Color startColor = Color.red;  // Delivery start color
    [SerializeField] private Color endColor = Color.green;  // Delivery end color
    [SerializeField] private GameObject startsObj;          // Object with all starts as children
    [SerializeField] private GameObject endsObj;            // Object with all ends as children

    /*
     * Name: OnDrawGizmos (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Visual shows all of the deliveries in the editor
     */
    void OnDrawGizmos()
    {
        // Fill arrays
        List<Transform> starts = new List<Transform>();
        List<Transform> ends = new List<Transform>();
        Transform[] theStartsArray = startsObj.GetComponentsInChildren<Transform>();
        Transform[] theEndsArray = endsObj.GetComponentsInChildren<Transform>();
        starts.Clear();
        ends.Clear();

        // Fill start lists
        foreach (Transform start in theStartsArray)
        {
            if (start != startsObj.transform)
            {
                starts.Add(start);
            }
        }

        // Fill ends lists
        foreach (Transform end in theEndsArray)
        {
            if (end != endsObj.transform)
            {
                ends.Add(end);
            }
        }

        // Dispaly all of the starts
        Gizmos.color = startColor;
        for (int i = 0; i < starts.Count; i++)
        {
            Vector3 position = starts[i].position;
            Gizmos.DrawWireSphere(position, 3.0f);
        }

        // Displays all of the ends
        Gizmos.color = endColor;
        for (int i = 0; i < ends.Count; i++)
        {
            Vector3 position = ends[i].position;
            Gizmos.DrawWireSphere(position, 3.0f);
        }
    }
}