using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEditor : MonoBehaviour
{
    public Color startColor = Color.red;
    public Color endColor = Color.green;
    public GameObject startsObj;
    public GameObject endsObj;
    public List<Transform> starts = new List<Transform>();
    public List<Transform> ends = new List<Transform>();
    Transform[] theStartsArray;
    Transform[] theEndsArray;

    void OnDrawGizmos()
    {
        theStartsArray = startsObj.GetComponentsInChildren<Transform>();
        theEndsArray = endsObj.GetComponentsInChildren<Transform>();
        starts.Clear();
        ends.Clear();

        foreach (Transform start in theStartsArray)
        {
            if (start != startsObj.transform)
            {
                starts.Add(start);
            }
        }
        foreach (Transform end in theEndsArray)
        {
            if (end != endsObj.transform)
            {
                ends.Add(end);
            }
        }
        Gizmos.color = startColor;
        for (int i = 0; i < starts.Count; i++)
        {
            Vector3 position = starts[i].position;
            Gizmos.DrawWireSphere(position, 3.0f);
        }
        Gizmos.color = endColor;
        for (int i = 0; i < ends.Count; i++)
        {
            Vector3 position = ends[i].position;
            Gizmos.DrawWireSphere(position, 3.0f);
        }
    }
}