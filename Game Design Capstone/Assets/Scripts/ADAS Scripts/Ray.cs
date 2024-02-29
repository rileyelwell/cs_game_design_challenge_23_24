using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
     public float raycastDistance = 100f; // The distance the ray will travel
     public LayerMask layerMask; // Layers to consider for the raycast
     public Vector3 rayDirection;
     public float drawDistance = 5.0f;
     public float interpolationRatio = 1.0f;
     public float stepCountSortOf = 1.0f;
     public float lifeSpan = 1.0f;
     public float stepDensity = 0.05f;
     public bool myCode = false;

     void Update()
     {
          // Define the origin of the ray. This should be a point on the exterior of the Robot.
          // For simplicity, let's assume the Robot has a Collider and we'll use its bounds.
          Vector3 rayOrigin = transform.position;

          //Debug.Log(transform.position);

          // Define the direction of the ray. This example sends the ray upwards.
          Vector3 rayDirection = transform.TransformDirection(Vector3.forward);

          
          // intended for interpolation ratio of 1.0
          if (myCode){
               for (float x = 0.0f; x < interpolationRatio; x += stepDensity)
               {
                    for (float y = 0.0f; y < interpolationRatio; y += stepDensity)
                    {
                         Vector3 horizontalVec = new Vector3(
                                                       (transform.right.x - (transform.right.x) * 2 * x ), 
                                                       (transform.up.y - (transform.up.y) * 2 * y ), 
                                                       (transform.right.z - (transform.right.z) * 2 * x )
                         ); //.normalized
                         Debug.DrawLine(transform.position, transform.position + (transform.forward + horizontalVec) * drawDistance, Color.red, lifeSpan, false);
                         //Debug.DrawLine(transform.position, transform.position + (transform.forward + Vector3.Lerp(-transform.up, transform.up, y) + Vector3.Lerp(-transform.right, transform.right, x)) * drawDistance, Color.blue, lifeSpan, false);
                         //Debug.DrawLine(transform.position, transform.position + (transform.forward + Vector3.Lerp(-transform.up, transform.up, y) + Vector3.Lerp(-transform.right, transform.right, x)) * drawDistance, Color.blue, lifeSpan, false);
                    }
               }
          }
          else
          {
               for (float x = 0.0f; x < stepCountSortOf; x += stepDensity)
               {
                    for (float y = 0.0f; y < stepCountSortOf; y += stepDensity)
                    {
                    
                         Debug.DrawLine(transform.position, transform.position + (transform.forward + Vector3.Lerp(-transform.up, transform.up, y) + Vector3.Lerp(-transform.right, transform.right, x)) * drawDistance, Color.blue, lifeSpan, false);
                         //Debug.DrawLine(transform.position, transform.position + (transform.forward + Vector3.Lerp(-transform.up, transform.up, y) + Vector3.Lerp(-transform.right, transform.right, x)) * drawDistance, Color.blue, lifeSpan, false);
                    }
               }
          }



          //Debug.Log(transform.position);

          //Debug.DrawLine(transform.position, transform.position + transform.forward * drawDistance, Color.blue, lifeSpan, false);


          //Debug.DrawLine(transform.position, transform.position + Vector3.Lerp(transform.forward, transform.up, interpolationRatio) * drawDistance, Color.blue, lifeSpan, false);
         //Debug.DrawLine(transform.position, transform.position + Vector3.Lerp(transform.forward, (transform.up+transform.right), interpolationRatio) * drawDistance, Color.blue, lifeSpan, false);
          //Debug.DrawLine(transform.position, transform.position + Vector3.Lerp(transform.forward, transform.right, interpolationRatio) * drawDistance, Color.blue, lifeSpan, false);
          // up
          //Debug.DrawLine(transform.position, transform.position + (Vector3(transform.forward.x, transform.forward.y + 0.25f, transform.forward.z) * drawDistance), Color.blue, 10.0f, false);

          //Debug.DrawLine(transform.position, transform.position + (Vector3(transform.forward.x, transform.forward.y + 0.25f, transform.forward.z) * drawDistance), Color.blue, 10.0f, false);
         // Debug.DrawLine(transform.position, transform.position + (Vector3(transform.forward.x, transform.forward.y + 0.25f, transform.forward.z + 0.25f) * drawDistance), Color.blue, 10.0f, false);
          //Debug.DrawLine(transform.position, transform.position + (Vector3(transform.forward.x + 0.25f, transform.forward.y + 0.25f, transform.forward.z) * drawDistance), Color.blue, 10.0f, false);

          //Debug.Log(transform.forward);

          // Perform the raycast
          RaycastHit hit;
          if (Physics.Raycast(rayOrigin, rayDirection, out hit, raycastDistance))
          {
               // The ray hit something
               Debug.Log("Raycast hit: " + hit.collider.name);
          }
          else
          {
               // The ray did not hit anything
               Debug.Log("Raycast did not hit anything.");
          }
     }
}
