using System;
using UnityEngine;

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
    public GameObject Player;
    public float raycastDistance = 1f;
    public LayerMask layerMask;
    public Vector3 rayDirection;
    public float drawDistance = 5.0f;
    public float lifeSpan = 1.0f;
    public bool activateRaycast = false;

    public float sStep = 16f;
    public float tStep = 16f;

    public float sLeft = 2;
    public float sRight = 2;
    public float tTop = 2;
    public float tBot = 2;

    public delegate void RaycastResultsUpdatedEventHandler(object sender, RaycastResultsUpdatedEventArgs e);

    public event RaycastResultsUpdatedEventHandler OnRayFUpdated;
    public event RaycastResultsUpdatedEventHandler OnRayLUpdated;
    public event RaycastResultsUpdatedEventHandler OnRayRUpdated;
    public event RaycastResultsUpdatedEventHandler OnRayBUpdated;

    private PlayerController playerController;

    private void Awake() {
        playerController = gameObject.GetComponentInParent<PlayerController>();
    }

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

                direction = rotationOffset * Player.transform.rotation * direction;

                Vector3 offsetPosition = Player.transform.position + (Player.transform.rotation * directionOffset);

                if (Physics.Raycast(offsetPosition, direction.normalized, out RaycastHit hit, drawDistance, layerMask))
                {
                    results[sIter, tIter] = hit;
                    hasHit = true;
                }

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
            RaycastHit[,] rayF = new RaycastHit[(int)sStep + 1, (int)tStep + 1];
            RaycastHit[,] rayL = new RaycastHit[(int)sStep + 1, (int)tStep + 1];
            RaycastHit[,] rayR = new RaycastHit[(int)sStep + 1, (int)tStep + 1];
            RaycastHit[,] rayB = new RaycastHit[(int)sStep + 1, (int)tStep + 1];

            bool hitF = RaycastInDirection(Vector3.forward * 0.3f, Quaternion.identity, rayF, Color.blue);
            bool hitL = RaycastInDirection(Vector3.left * 0.25f, Quaternion.Euler(0, -90, 0), rayL, Color.green);
            bool hitR = RaycastInDirection(Vector3.right * 0.25f, Quaternion.Euler(0, 90, 0), rayR, Color.red);
            bool hitB = RaycastInDirection(Vector3.back * 0.3f, Quaternion.Euler(0, 180, 0), rayB, Color.yellow);

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


// using System;
// using UnityEngine;

// public class RaycastResultsUpdatedEventArgs : EventArgs
// {
//     public RaycastHit[,] Results { get; private set; }

//     public RaycastResultsUpdatedEventArgs(RaycastHit[,] results)
//     {
//         Results = results;
//     }
// }

// public class Ray : MonoBehaviour
// {
//     public GameObject Player;
//     public float raycastDistance = 1f;
//     public LayerMask layerMask;
//     public Vector3 rayDirection;
//     public float drawDistance = 5.0f;
//     public float lifeSpan = 1.0f;
//     public bool activateRaycast = false;

//     public float sStep = 16f;
//     public float tStep = 16f;

//     public float sLeft = 2;
//     public float sRight = 2;
//     public float tTop = 2;
//     public float tBot = 2;

//     // Define a delegate for the event within the class
//     public delegate void RaycastResultsUpdatedEventHandler(object sender, RaycastResultsUpdatedEventArgs e);

//     // Declare a public event of the delegate type within the class
//     public event RaycastResultsUpdatedEventHandler OnRaycastResultsUpdated;

//     void Update()
//     {
//         if (activateRaycast)
//         {
//             RaycastHit[,] results = new RaycastHit[(int)sStep + 1, (int)tStep + 1];
//             int sIter = 0;
//             int tIter = 0;

//             float sL = Mathf.PI / sLeft;
//             float sR = Mathf.PI / sRight;
//             float sS = (sL + sR) / sStep;
//             float tB = Mathf.PI / tBot;
//             float tT = Mathf.PI / tTop;
//             float tS = (tT + tB) / tStep;

//             for (float s = -sL; s <= sR; s += sS)
//             {
//                 tIter = 0;
//                 for (float t = -tB; t <= tT; t += tS)
//                 {
//                     Vector3 direction = new Vector3(
//                         Mathf.Sin(s) * Mathf.Cos(t),
//                         Mathf.Sin(t),
//                         Mathf.Cos(s)
//                     );

//                     direction = Player.transform.rotation * direction;

//                     if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, drawDistance, layerMask))
//                     {
//                         results[sIter, tIter] = hit;
//                     }

//                     Debug.DrawLine(transform.position, transform.position + direction.normalized * drawDistance, Color.blue, lifeSpan, true);
//                     tIter++;
//                 }
//                 sIter++;
//             }
//             OnRaycastResultsUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(results));
            
//         }
//     }
// }