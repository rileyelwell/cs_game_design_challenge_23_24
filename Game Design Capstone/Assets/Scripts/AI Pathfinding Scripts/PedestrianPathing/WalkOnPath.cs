using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOnPath : MonoBehaviour
{

    public PedestrianPathEditor PathToFollow;

    public int StartingWayPointID = 0;
    private int CurrentWayPointID;
    public float speed;
    public float reachDistance = 1.0f;
    public float rotationSpeed = 5.0f;

    void Start()
    {
        CurrentWayPointID = StartingWayPointID;
    }

    void Update()
    {
        Vector3 next_waypoint = new Vector3(PathToFollow.path_objs[CurrentWayPointID].position.x, transform.position.y, PathToFollow.path_objs[CurrentWayPointID].position.z);

        float distance = Vector3.Distance(next_waypoint, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, next_waypoint, Time.deltaTime * speed);

        var rotation = Quaternion.LookRotation(next_waypoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        if (distance <= reachDistance )
        {
            CurrentWayPointID++;
        }
        if (CurrentWayPointID >= PathToFollow.path_objs.Count)
        {
            Destroy(gameObject);
        }
    }
}
