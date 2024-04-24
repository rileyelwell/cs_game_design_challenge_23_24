
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class PedestrianPathEditor : MonoBehaviour
{
    public Color rayColor = Color.white;
    public List<Transform> path_objs = new List<Transform> ();
    Transform[] theArray;
    
    public GameObject pedestrian;
    public float minDensity;
    public float maxDensity;
    // Randomized Spawning
    /*
    void Start()
    {
        theArray = GetComponentsInChildren<Transform>();
        float pathLength = 0;
        for(int i = 0; i < theArray.Length - 1; i++)
        {
            pathLength += Vector3.Distance(theArray[i].position, theArray[i+1].position);
        }
        pathLength += Vector3.Distance(theArray[theArray.Length - 2].position, theArray[theArray.Length - 1].position);
        UnityEngine.Debug.Log("Path Length:" + pathLength);
        float currentLength = 0;
        float incrementLength;
        float distanceFromNext;

        int currentIndex = 0;
        int nextIndex = 1;

        Transform currentPosition = theArray[currentIndex];
        float incrementX;
        float incrementZ;
        while(currentLength < pathLength && currentIndex < theArray.Length)
        {
            UnityEngine.Debug.Log("Spawned!");
            Instantiate(pedestrian, currentPosition);
            incrementLength = UnityEngine.Random.Range(minDensity, maxDensity);
            distanceFromNext = Vector3.Distance(theArray[currentIndex].position, theArray[nextIndex].position);
            while ( incrementLength > distanceFromNext && currentIndex < theArray.Length)
            {
                incrementLength -= distanceFromNext;
                currentIndex++;
                if (currentIndex < theArray.Length - 1)
                {
                    nextIndex++;
                }
                else if (currentIndex == theArray.Length - 1)
                {
                    nextIndex = 0;
                }
                else
                {
                    break;
                }
                distanceFromNext = Vector3.Distance(theArray[currentIndex].position, theArray[nextIndex].position);
            }
            if(currentIndex == theArray.Length)
            {
                break;
            }
            currentPosition = theArray[currentIndex];
            incrementX = incrementLength / distanceFromNext * (theArray[nextIndex].position.x - currentPosition.position.x);
            incrementZ = incrementLength / distanceFromNext * (theArray[nextIndex].position.z - currentPosition.position.z);
            currentPosition.position += new Vector3(incrementX, 0, incrementZ);
        }


    }*/

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        theArray = GetComponentsInChildren<Transform> ();
        path_objs.Clear ();

        foreach (Transform path_obj in theArray)
        {
            if(path_obj != this.transform)
            {
                path_objs.Add(path_obj);
            }
        }
        for(int i = 0; i < path_objs.Count; i++)
        {
            Vector3 position = path_objs[i].position;
            Gizmos.DrawWireSphere(position, 0.3f);
            if (i > 0)
            {
                Vector3 previous = path_objs[i - 1].position;
                Gizmos.DrawLine(previous, position);
            }
            if(i == path_objs.Count - 1)
            {
                Gizmos.DrawLine(path_objs[0].position, position);
            }


        }
    }
}
