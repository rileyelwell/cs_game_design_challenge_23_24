using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DeliveryHandler : MonoBehaviour
{
    public GameObject player;

    public GameObject startsObj;
    public GameObject endsObj;

    public List<Transform> starts = new List<Transform>();
    public List<Transform> ends = new List<Transform>();
    private Transform[] theStartsArray;
    private Transform[] theEndsArray;
    public Transform currStart;
    public Transform currEnd;

    public GameObject waypoint;
    public bool ExpoMode;

    bool startReached;
    bool endReached;

    bool printedStart;
    bool printedEnd;

    float goalRange = 3.0f;

    void Start()
    {
        startReached = true;
        endReached = true;

        printedStart = false;
        printedEnd = false;

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
    }

    void Update()
    {
        if (startReached && endReached)
        {
            startReached = false;
            endReached = false;
            if(ExpoMode == false)
            {
                GetDelivery();
            }
            else
            {
                ExpoDeliveryComplete();
            }
            CreateWaypoint(currStart);
            if (!printedStart)
            {
                printedStart = true;
                UnityEngine.Debug.Log("Goal: Pickup order at location: " + currStart.name, player);
            }
        }
        if (Vector3.Distance(currStart.position, player.transform.position) < goalRange)
        {
            startReached = true;
            CreateWaypoint(currEnd);
            if (!printedEnd)
            {
                printedEnd = true;
                UnityEngine.Debug.Log("Goal: Deliver order to location" + currEnd.name, player);
            }
        }
        if (Vector3.Distance(currEnd.position, player.transform.position) < goalRange && startReached)
        {
            endReached = true;
            UnityEngine.Debug.Log("Goal: Complete!", player);
            printedEnd = false;
            printedStart = false;
        }
    }

    void ExpoDeliveryComplete()
    {
        currStart = starts[0];
        currEnd = ends[0];

        player.transform.position = currStart.position;
    }

    void GetDelivery()
    {
        int min = 0;
        int max = starts.Count;
        int index = UnityEngine.Random.Range(min, max);

        currStart = starts[index];

        max = ends.Count;
        index = UnityEngine.Random.Range(min, max);

        currEnd = ends[index];
    }

    void CreateWaypoint(Transform newWaypoint)
    {
        waypoint.transform.position = newWaypoint.position;
    }
}
