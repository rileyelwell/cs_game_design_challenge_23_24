using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveOnPath : MonoBehaviour
{
    public VehiclePathEditor PathToFollow;                  // Path for the pedestrian to follow
    [SerializeField] private int startingWaypointID = 0;    // Start point on path
    private int currentWaypointID;                          // Current point on path
    private Vector3 currentWaypoint;                        // Current point on path         
    [SerializeField] private float speed;                   // Walk speed
    [SerializeField] private float reachDistance = 1.0f;    // Distance from point on path before turning
    [SerializeField] private float rotationSpeed = 5.0f;    // Rotation speed

    /*
     * Name: Start (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Places a spawner at the beginning of the path
     */
    void Start()
    {
        currentWaypointID = startingWaypointID;
        currentWaypoint = new Vector3(PathToFollow.path_objs[currentWaypointID].position.x, transform.position.y + 1, PathToFollow.path_objs[currentWaypointID].position.z);
    }

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Moves the vehicle across the given path
     */
    void Update()
    {
        // Negate height differences
        currentWaypoint.y = transform.position.y;
        // Get distance from point
        float distance = Vector3.Distance(currentWaypoint, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Time.deltaTime * speed);

        // Get rotation from point
        var rotation = Quaternion.LookRotation(currentWaypoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        // Check to continue to next point
        if (distance <= reachDistance )
        {
            currentWaypointID++;

            // Check to despawn
            if (currentWaypointID >= PathToFollow.path_objs.Count)
            {
                Destroy(gameObject);
                return;
            }
            currentWaypoint = new Vector3(PathToFollow.path_objs[currentWaypointID].position.x, transform.position.y, PathToFollow.path_objs[currentWaypointID].position.z);
        }

    }
}