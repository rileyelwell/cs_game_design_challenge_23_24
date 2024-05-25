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

    // Define a delegate for the event within the class
    public delegate void RaycastResultsUpdatedEventHandler(object sender, RaycastResultsUpdatedEventArgs e);

    // Declare a public event of the delegate type within the class
    public event RaycastResultsUpdatedEventHandler OnRaycastResultsUpdated;

    void Update()
    {
        if (activateRaycast)
        {
            RaycastHit[,] results = new RaycastHit[(int)sStep + 1, (int)tStep + 1];
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

                    direction = Player.transform.rotation * direction;

                    if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, drawDistance, layerMask))
                    {
                        results[sIter, tIter] = hit;
                    }

                    Debug.DrawLine(transform.position, transform.position + direction.normalized * drawDistance, Color.blue, lifeSpan, true);
                    tIter++;
                }
                sIter++;
            }
            OnRaycastResultsUpdated?.Invoke(this, new RaycastResultsUpdatedEventArgs(results));
            
        }
    }
}