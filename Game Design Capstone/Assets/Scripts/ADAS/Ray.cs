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
    public GameObject Player;  // The player GameObject to use as the origin of the raycasts
    public float raycastDistance = 1f;  // Distance of each raycast
    public LayerMask layerMask;  // Layer mask to specify which layers to raycast against
    public Vector3 rayDirection;  // Direction of the raycast
    public float drawDistance = 5.0f;  // Distance to draw debug lines for raycasts
    public float lifeSpan = 1.0f;  // Lifespan of debug lines
    public bool activateRaycast = false;  // Flag to activate raycasting

    public float sStep = 16f;  // Number of steps in the s direction
    public float tStep = 16f;  // Number of steps in the t direction

    public float sLeft = 2;  // Left boundary for s
    public float sRight = 2;  // Right boundary for s
    public float tTop = 2;  // Top boundary for t
    public float tBot = 2;  // Bottom boundary for t

    // Delegates for the raycast results events
    public delegate void RaycastResultsUpdatedEventHandler(object sender, RaycastResultsUpdatedEventArgs e);

    // Events to notify when raycast results are updated
    public event RaycastResultsUpdatedEventHandler OnRayFUpdated;
    public event RaycastResultsUpdatedEventHandler OnRayLUpdated;
    public event RaycastResultsUpdatedEventHandler OnRayRUpdated;
    public event RaycastResultsUpdatedEventHandler OnRayBUpdated;

    private PlayerController playerController;  // Reference to the player controller

    private void Awake() {
        // Initialize the player controller reference
        playerController = gameObject.GetComponentInParent<PlayerController>();
    }

    // Method to perform raycasting in a specific direction
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

            // Update player controller and trigger events based on raycast results
            if (hitF)
            {
                OnRayFUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(rayF));
                playerController.frontWarning = true;
            } else 
            {
                playerController.frontWarning = false;
            }

            if (hitL)
            {
                OnRayLUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(rayL));
                playerController.leftWarning = true;
            } else 
            {
                playerController.leftWarning = false;
            }

            if (hitR)
            {
                OnRayRUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(rayR));
                playerController.rightWarning = true;
            } else 
            {
                playerController.rightWarning = false;
            }

            if (hitB)
            {
                OnRayBUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(rayB));
                playerController.backWarning = true;
            } else 
            {
                playerController.backWarning = false;
            }
        }
    }
}
