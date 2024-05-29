using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePathEditor : MonoBehaviour
{
    public List<Transform> path_objs = new List<Transform>();   // List of nodes in path
    [SerializeField] private Color rayColor = Color.yellow;     // Color of the path in scene
    [SerializeField] private GameObject vehicle;                // Pedestrian prefab
    private GameObject start;                                   // Spawner location
    [SerializeField] private float minTime;                     // Minimum time between spawns
    [SerializeField] private float maxTime;                     // Maximum time between spawns
    private float timer;                                        // Current time between spawns
    [SerializeField] private float startTimer;                  // Starting time before spawn


    /*
     * Name: Start (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Place a spawner at the beginning of the path and set timer
     */
    void Start()
    {
        start = new GameObject("Spawner");
        start.transform.parent = this.gameObject.transform.parent;
        start.transform.position = path_objs[0].position;
        start.transform.rotation = Quaternion.LookRotation(path_objs[1].position - path_objs[0].position);
        timer = startTimer;
    }

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Spawns a vehicle after a specified range of seconds
     */
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = UnityEngine.Random.Range(minTime, maxTime);

            // Spawns a vehicle to drive on this path
            GameObject temp_vehicle = Instantiate(vehicle, start.transform);
            temp_vehicle.GetComponent<DriveOnPath>().PathToFollow = this.gameObject.GetComponent<VehiclePathEditor>();
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
        for (int i = 1; i < path_objs.Count - 1; i++)
        {
            position = path_objs[i].position;
            Gizmos.DrawWireSphere(position, 0.3f);
            previous = path_objs[i - 1].position;
            Gizmos.DrawLine(previous, position);
        }

        // Draw a large wire sphere at the end
        position = path_objs[path_objs.Count - 1].position;
        Gizmos.DrawWireSphere(position, 1.0f);
        previous = path_objs[path_objs.Count - 2].position;
        Gizmos.DrawLine(previous, position);
    }
}