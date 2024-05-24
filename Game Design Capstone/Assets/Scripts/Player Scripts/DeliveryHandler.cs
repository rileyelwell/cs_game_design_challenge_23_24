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

    public bool startReached;
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
        // if the player has not made a delivery yet or picked one up
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
                GameplayManager.instance.DisplayCurrentObjective("Pickup order from " + currStart.name);
            }
        }

        // if the player has picked up, but not delivered yet
        if (Vector3.Distance(currStart.position, player.transform.position) < goalRange)
        {
            startReached = true;
            CreateWaypoint(currEnd);

            // if the player has picked up their order and the timer is not running yet, start the timer
            if (!GameplayManager.instance.GetComponent<DeliveryTimer>().isRunning) 
            {
                GameplayManager.instance.GetComponent<DeliveryTimer>().StartTimer();
            }

            if (!printedEnd)
            {
                printedEnd = true;
                //UnityEngine.Debug.Log("Goal: Deliver order to location" + currEnd.name, player);
                GameplayManager.instance.DisplayCurrentObjective("Deliver order to " + currEnd.name);
            }
        }

        // if the player has successfully delivered an order
        if (Vector3.Distance(currEnd.position, player.transform.position) < goalRange && startReached)
        {
            endReached = true;
            //UnityEngine.Debug.Log("Goal: Complete!", player);
            GameplayManager.instance.DisplayCurrentObjective("Order successfully delivered");
            printedEnd = false;
            printedStart = false;

            // temporarily pause the game and show the player their delivery score
            ScoreHandler.instance.CalculateDeliveryScore();
            ScoreHandler.instance.DisplayScoreScreen();

            // stop and reset the temp timer
            GameplayManager.instance.GetComponent<DeliveryTimer>().StopTimer();
            GameplayManager.instance.GetComponent<DeliveryTimer>().ResetTimer();

            // reset the UI for health and temps
            //ScoreHandler.instance.ResetUI();
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

    public void UpdateForFailedDelivery()
    {
        endReached = true;
        printedEnd = false;
        printedStart = false;

        // temporarily pause the game and show the player their delivery score
        ScoreHandler.instance.DisplayScoreScreen();

        // stop and reset the temp timer
        GameplayManager.instance.GetComponent<DeliveryTimer>().StopTimer();
        GameplayManager.instance.GetComponent<DeliveryTimer>().ResetTimer();
    }
}
