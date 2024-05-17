using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePathEditor : MonoBehaviour
{
    public Color rayColor = Color.yellow;
    public List<Transform> path_objs = new List<Transform>();
    private GameObject start;
    private Transform[] theArray;

    public GameObject vehicle;
    public float minTime;
    public float maxTime;

    private float timer;

    void Start()
    {
        start = new GameObject("Spawner");
        start.transform.parent = this.gameObject.transform.parent;
        start.transform.position = path_objs[0].position;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = UnityEngine.Random.Range(minTime, maxTime);

            GameObject temp_vehicle = Instantiate(vehicle, start.transform);
            temp_vehicle.GetComponent<DriveOnPath>().PathToFollow = this.gameObject.GetComponent<VehiclePathEditor>();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        theArray = GetComponentsInChildren<Transform>();
        path_objs.Clear();

        foreach (Transform path_obj in theArray)
        {
            if (path_obj != this.transform)
            {
                path_objs.Add(path_obj);
            }
        }

        Vector3 position = path_objs[0].position;
        Gizmos.DrawWireSphere(position, 1.0f);

        Vector3 previous;
        for (int i = 1; i < path_objs.Count - 1; i++)
        {
            position = path_objs[i].position;
            Gizmos.DrawWireSphere(position, 0.3f);
            previous = path_objs[i - 1].position;
            Gizmos.DrawLine(previous, position);
        }

        position = path_objs[path_objs.Count - 1].position;
        Gizmos.DrawWireSphere(position, 1.0f);
        previous = path_objs[path_objs.Count - 2].position;
        Gizmos.DrawLine(previous, position);
    }
}