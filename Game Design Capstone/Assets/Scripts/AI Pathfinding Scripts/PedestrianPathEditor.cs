using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianPathEditor : MonoBehaviour
{
    public List<Transform> path_objs = new List<Transform>();   // List of nodes in path
    [SerializeField] private Color rayColor = Color.magenta;    // Color of the path in scene
    [SerializeField] private GameObject pedestrian;             // Pedestrian prefab
    private GameObject start;                                   // Spawner location
    [SerializeField] private float minTime;                     // Minimum time between spawns
    [SerializeField] private float maxTime;                     // Maximum time between spawns
    private float timer;                                        // Current time between spawns
    [SerializeField] private float offset;                     // The offset range each pedestrian could have walking the path

    /*
     * Name: Start (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Places a spawner at the beginning of the path
     */
    void Start()
    {
        start = new GameObject("Spawner");
        start.transform.parent = this.gameObject.transform.parent;
        start.transform.position = path_objs[0].position;
        start.transform.rotation = Quaternion.LookRotation(path_objs[1].position - path_objs[0].position);
    }

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Spawns a pedestrian after a specified range of seconds
     */
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = UnityEngine.Random.Range(minTime, maxTime);

            // Spawns a pedestrian to walk on this path
            GameObject temp_pedestrian = Instantiate(pedestrian, start.transform);
            temp_pedestrian.GetComponent<WalkOnPath>().SetPath(this.gameObject.GetComponent<PedestrianPathEditor>());
            temp_pedestrian.GetComponent<WalkOnPath>().SetOffset(offset);
        }
    }

    /*
     * Name: OnDrawGizmos (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Fills path_objs with the points in the path and draws the entire thing in the editor
     */
    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;

        // FIll path_objs with the path points
        Transform[] theArray = GetComponentsInChildren<Transform>();
        path_objs.Clear();
        foreach (Transform path_obj in theArray)
        {
            if (path_obj != this.transform)
            {
                path_objs.Add(path_obj);
            }
        }

        // Draw a large wire sphere at the start
        Vector3 position = path_objs[0].position;
        Gizmos.DrawWireSphere(position, 1.0f);

        // Draw lines between each point and smaller wire sphere at each point
        Vector3 previous;
        if (offset == 0)
            offset = 0.1f;
        Vector3 oC = new Vector3(offset, offset, offset);
        for (int i = 1; i < path_objs.Count - 1; i++)
        {
            oC.y = offset;
            position = path_objs[i].position;
            Gizmos.DrawWireCube(position, oC * 2);
            previous = path_objs[i - 1].position;
            oC.y = 0;
            Gizmos.DrawLine(previous + oC, position + oC);
            oC.x*=-1;
            Gizmos.DrawLine(previous + oC, position + oC);
            oC *= -1;
            Gizmos.DrawLine(previous + oC, position + oC);
            oC.x *= -1;
            Gizmos.DrawLine(previous + oC, position + oC);
        }

        // Draw a large wire sphere at the end
        position = path_objs[path_objs.Count - 1].position;
        Gizmos.DrawWireSphere(position, 1.0f);
        previous = path_objs[path_objs.Count - 2].position;
        Gizmos.DrawLine(previous, position);
    }
}