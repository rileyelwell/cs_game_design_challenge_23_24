using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianPathEditor : MonoBehaviour
{
    public Color rayColor = Color.white;
    public List<Transform> path_objs = new List<Transform> ();
    Transform[] theArray;

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        theArray = GetComponentsInChildren<Transform> ();
        path_objs.Clear ();

        foreach (Transform path_obj in theArray)
        {
            if(path_obj != this.transform)
            {
                path_objs.Add(path_obj);
            }
        }
        for(int i = 0; i < path_objs.Count; i++)
        {
            Vector3 position = path_objs[i].position;
            Gizmos.DrawWireSphere(position, 0.3f);
            if (i > 0)
            {
                Vector3 previous = path_objs[i - 1].position;
                Gizmos.DrawLine(previous, position);
            }
            if(i == path_objs.Count - 1)
            {
                Gizmos.DrawLine(path_objs[0].position, position);
            }


        }
    }
}
