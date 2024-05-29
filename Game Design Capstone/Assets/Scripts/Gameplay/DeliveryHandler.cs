using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;                             // Player
    [SerializeField] private GameObject startsObj, endsObj;                 // Objects holding all of the start and end positions
    [SerializeField] private GameObject waypoint;                           // Waypoint
    [SerializeField] private List<Transform> starts = new List<Transform>();// List of starting locations for deliveries
    [SerializeField] private List<Transform> ends = new List<Transform>();  // List of ending locations for deliveries
    [SerializeField] bool ExpoMode;                                         // Makes each game identical if on for expo
    [SerializeField] float goalRange = 3.0f;                                // Distance player needs to be from goal to succeed
    private Transform currStart, currEnd;                                   // Current start and end locations
    private bool startReached, endReached;                                  // Used to track what stages of the delivery are completed
    private bool printedStart, printedEnd;                                  // Used to track what stages of printing are completed
    private DeliveryTimer deliveryTimer;                                    // Displays a timer on the screen

    /*
     * Name: Awake (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Initializes the delivery timer UI
     */
    private void Awake() {
        deliveryTimer = UIManager.instance.GetComponent<DeliveryTimer>();
    }

    /*
     * Name: Start (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Initializes booleans for game state and fills the lists of start and end locations
     */
    void Start()
    {
        // Initialize game state booleans
        startReached = true;
        endReached = true;
        printedStart = false;
        printedEnd = false;

        // Fill start and end locations
        Transform[] theStartsArray = startsObj.GetComponentsInChildren<Transform>();
        Transform[] theEndsArray = endsObj.GetComponentsInChildren<Transform>();
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

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Checks and updates game state 
     */
    void Update()
    {
        // Checks if the player has yet to pick up a delivery
        if (startReached && endReached)
        {
            startReached = false;
            endReached = false;

            // Check for Expo mode
            if(ExpoMode == false)
                GetDelivery();
            else
                GetExpoDelivery();

            SetWaypoint(currStart);

            // Update UI once
            if (!printedStart)
            {
                printedStart = true;
                UIManager.instance.UpdateCurrentObjectiveDisplay("Pickup order from " + currStart.name);
            }
        }

        // Checks if the player is in the middle of a delivery
        if (Vector3.Distance(currStart.position, player.transform.position) < goalRange)
        {
            startReached = true;
            SetWaypoint(currEnd);

            // If the player has picked up their order and the timer is not running yet, start the timer
            if (!deliveryTimer.isRunning) 
                deliveryTimer.StartTimer();

            // Update UI once
            if (!printedEnd)
            {
                printedEnd = true;
                UIManager.instance.UpdateCurrentObjectiveDisplay("Deliver order to " + currEnd.name);
            }
        }

        // Checks if the player has made a successful delivery
        if (Vector3.Distance(currEnd.position, player.transform.position) < goalRange && startReached)
        {
            endReached = true;
            UIManager.instance.UpdateCurrentObjectiveDisplay("Order successfully delivered");
            printedEnd = false;
            printedStart = false;

            // Temporarily pause the game and show the player their delivery score
            GameplayManager.instance.DisplayScoreScreen();
        }
    }

    /*
     * Name: GetExpoDelivery
     * Inputs: none
     * Outputs: none
     * Description: Set the start location to dutch bros and the end location to the corner of buxton
     */
    void GetExpoDelivery()
    {
        currStart = starts[0];
        currEnd = ends[0];
        // Move player to the start
        player.transform.position = currStart.position;
    }

    /*
     * Name: GetDelivery
     * Inputs: none
     * Outputs: none
     * Description: Set a random start location and a random end location
     */
    void GetDelivery()
    {
        int min = 0;
        int max = starts.Count;

        // Gets random start
        int index = UnityEngine.Random.Range(min, max);
        currStart = starts[index];
        max = ends.Count;

        // Gets random end
        index = UnityEngine.Random.Range(min, max);
        currEnd = ends[index];
    }

    /*
     * Name: CreateWaypoint
     * Inputs: new waypoint position
     * Outputs: none
     * Description: Placse a waypoint at a specified position
     */
    void SetWaypoint(Transform newWaypoint)
    {
        waypoint.transform.position = newWaypoint.position;
    }

    /*
     * Name: GetCurrentWaypoint
     * Inputs: none
     * Outputs: transform of current waypoint
     * Description: Returns the current waypoint position
     */
    public Transform GetCurrentWaypoint()
    {
        return waypoint.transform;
    }
}
