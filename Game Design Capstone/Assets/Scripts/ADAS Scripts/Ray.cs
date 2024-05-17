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

            bool hitF = RaycastInDirection(Vector3.forward * 0.5f, Quaternion.identity, rayF, Color.blue);
            bool hitL = RaycastInDirection(Vector3.left * 0.5f, Quaternion.Euler(0, -90, 0), rayL, Color.green);
            bool hitR = RaycastInDirection(Vector3.right * 0.5f, Quaternion.Euler(0, 90, 0), rayR, Color.red);
            bool hitB = RaycastInDirection(Vector3.back * 0.5f, Quaternion.Euler(0, 180, 0), rayB, Color.yellow);

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
        }
    }
}
