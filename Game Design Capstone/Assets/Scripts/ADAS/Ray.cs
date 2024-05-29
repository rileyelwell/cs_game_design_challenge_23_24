using System;
using UnityEngine;

// EventArgs for passing raycast results
public class RaycastResultsUpdatedEventArgs : EventArgs
{
    public RaycastHit[,] Results { get; private set; }

    public RaycastResultsUpdatedEventArgs(RaycastHit[,] results)
    {
        Results = results;
    }
}

public class Ray : MonoBehaviour
{
    [SerializeField] private GameObject Player;             // The player GameObject to use as the origin of the raycasts
    [SerializeField] private LayerMask layerMask;           // Layer mask to specify which layers to raycast against
    [SerializeField] private Vector3 rayDirection;          // Direction of the raycast
    [SerializeField] private float drawDistance = 5.0f;     // Distance to draw debug lines for raycasts
    [SerializeField] private float lifeSpan = 1.0f;         // Lifespan of debug lines
    [SerializeField] private bool activateRaycast = false;  // Flag to activate raycasting
    private PlayerController playerController;              // Reference to the player controller for ADAS updates

    [SerializeField] private float sStep = 16f, tStep = 16f; // Number of steps in the s and t direction
    [SerializeField] private float sLeft = 2, sRight = 2;   // Left and right boundary for s
    [SerializeField] private float tTop = 2, tBot = 2;      // Top and bottom boundary for t

    // Delegates for the raycast results events
    public delegate void RaycastResultsUpdatedEventHandler(object sender, RaycastResultsUpdatedEventArgs e);

    // Events to notify when raycast results are updated
    public event RaycastResultsUpdatedEventHandler OnRayFUpdated;
    public event RaycastResultsUpdatedEventHandler OnRayLUpdated;
    public event RaycastResultsUpdatedEventHandler OnRayRUpdated;
    public event RaycastResultsUpdatedEventHandler OnRayBUpdated;

    /*
     * Name: Awake (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Gets the player controller
     */
    private void Awake() {
        playerController = gameObject.GetComponentInParent<PlayerController>();
    }

    /*
     * Name: RaycastInDirection
     * Inputs: direction offset, rotation offset, raycast results, raycast color
     * Outputs: boolean of if there was a hit or not
     * Description: Performs raycasting in a specific direction
     */
    private bool RaycastInDirection(Vector3 directionOffset, Quaternion rotationOffset, RaycastHit[,] results, Color debugColor)
    {
        bool hasHit = false;
        int sIter = 0;
        int tIter = 0;

        float sL = Mathf.PI / sLeft;
        float sR = Mathf.PI / sRight;
        float sS = (sL + sR) / sStep;
        float tB = Mathf.PI / tBot;
        float tT = Mathf.PI / tTop;
        float tS = (tT + tB) / tStep;

        // Loop through the steps in the s and t directions
        for (float s = -sL; s <= sR; s += sS)
        {
            tIter = 0;
            for (float t = -tB; t <= tT; t += tS)
            {
                Vector3 direction = new Vector3(
                    Mathf.Sin(s) * Mathf.Cos(t),
                    Mathf.Sin(t),
                    Mathf.Cos(s)
                );

                // Rotate the direction vector
                direction = rotationOffset * Player.transform.rotation * direction;

                // Calculate the offset position for the raycast
                Vector3 offsetPosition = Player.transform.position + (Player.transform.rotation * directionOffset);

                // Perform the raycast
                if (Physics.Raycast(offsetPosition, direction.normalized, out RaycastHit hit, drawDistance, layerMask))
                {
                    results[sIter, tIter] = hit;
                    hasHit = true;
                }

                // Draw debug lines for visualization
                Debug.DrawLine(offsetPosition, offsetPosition + direction.normalized * drawDistance, debugColor, lifeSpan, true);
                tIter++;
            }
            sIter++;
        }

        return hasHit;
    }


    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Checks for raycast hits and sends them to the player controller
     */
    void Update()
    {
        if (activateRaycast)
        {
            // Initialize raycast result arrays for each direction
            RaycastHit[,] rayF = new RaycastHit[(int)sStep + 1, (int)tStep + 1];
            RaycastHit[,] rayL = new RaycastHit[(int)sStep + 1, (int)tStep + 1];
            RaycastHit[,] rayR = new RaycastHit[(int)sStep + 1, (int)tStep + 1];
            RaycastHit[,] rayB = new RaycastHit[(int)sStep + 1, (int)tStep + 1];

            // Perform raycasts in each direction
            bool hitF = RaycastInDirection(Vector3.forward * 0.3f, Quaternion.identity, rayF, Color.blue);
            bool hitL = RaycastInDirection(Vector3.left * 0.25f, Quaternion.Euler(0, -90, 0), rayL, Color.green);
            bool hitR = RaycastInDirection(Vector3.right * 0.25f, Quaternion.Euler(0, 90, 0), rayR, Color.red);
            bool hitB = RaycastInDirection(Vector3.back * 0.3f, Quaternion.Euler(0, 180, 0), rayB, Color.yellow);

            // Trigger events based on raycast results
            if (hitF)
            {
                OnRayFUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(rayF));
            }
            if (hitL)
            {
                OnRayLUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(rayL));
            }
            if (hitR)
            {
                OnRayRUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(rayR));
            }
            if (hitB)
            {
                OnRayBUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(rayB));
            }

            // Update player controller
            playerController.SetWarning('f', hitF);
            playerController.SetWarning('b', hitB);
            playerController.SetWarning('l', hitL);
            playerController.SetWarning('r', hitR);
        }
    }
}
