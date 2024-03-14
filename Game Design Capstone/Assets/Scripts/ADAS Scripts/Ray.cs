using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
     public GameObject Player;

     public float raycastDistance = 100f; // The distance the ray will travel
     public LayerMask layerMask; // Layers to consider for the raycast
     public Vector3 rayDirection;
     public float drawDistance = 5.0f;
     public float interpolationRatio = 1.0f;
     public float lifeSpan = 1.0f;
     public float stepDensity = 10.0f;
     public bool fuckme = false;


     /* funny new algo */
     public float sStep = 16f;
     public float tStep = 16f;

     public float sLeft = 2;
     public float sRight = 2;
     public float tTop = 2;
     public float tBot = 2;

     public float radius = 1f;
     public float angle = 90f;
     public int segments = 10;

     //[SerializeField] LineRenderer lineRend;


     private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float stepAngle = Mathf.PI * 2f / segments;
        for (int i = 0; i < segments; i++)
        {
            float currentAngle = stepAngle * i;
            Vector3 point = center + new Vector3(Mathf.Cos(currentAngle) * radius, 0, Mathf.Sin(currentAngle) * radius);
            if (i > 0)
            {
                Debug.DrawLine(point, center + new Vector3(Mathf.Cos(currentAngle - stepAngle) * radius, 0, Mathf.Sin(currentAngle - stepAngle) * radius), Color.white);
            }
        }
    }


void Start()
{
     
}

void Update()
{
     //if (Player != null)
     //{
     //     Transform playerTransform = Player.transform;
     //     transform.position = Player.transform.position;
     //     Debug.Log("transforming");
     //     // Optional: Set the semisphere's rotation to face the player's forward direction
     //     transform.rotation = Quaternion.LookRotation(playerTransform.forward);
     //}

     float sL = Mathf.PI/sLeft; float sR = Mathf.PI/sRight; float sS = (sL + sR) / sStep;
     float tB = Mathf.PI/tBot; float tT = Mathf.PI/tTop; float tS = (tT + tB) / tStep;

     if (fuckme)
     {
          //float sAdjust = Mathf.Atan(transform.forward.z/transform.forward.x);
          //Debug.Log(sAdjust);
          for (float s = (-sL); s <= sR; s += sS) // horizontal
          {
               for (float t = (-tB); t <= tT; t += tS) // vertical
               {
                    Vector3 direction = new Vector3(
                         Mathf.Sin(s) * Mathf.Cos(t),
                         Mathf.Sin(t) ,
                         Mathf.Cos(s)
                    );

                    RaycastHit hit;

                    direction = Player.transform.rotation * direction;

                    if (Physics.Raycast(transform.position, direction.normalized, out hit, drawDistance, layerMask))
                    {
                         Debug.Log("Raycast hit: " + hit.collider.name);
                         
                    }
                    Debug.DrawLine(transform.position, transform.position + direction.normalized * drawDistance, Color.blue, lifeSpan, true);
                    
               }
          }
     }

          
     

     // Define the origin of the ray. This should be a point on the exterior of the Robot.
     // For simplicity, let's assume the Robot has a Collider and we'll use its bounds.
     //Vector3 rayOrigin = transform.position;

     //Debug.Log(transform.position);

     // Define the direction of the ray. This example sends the ray forwards.
     //Vector3 rayDirection = transform.TransformDirection(Vector3.forward);
     /*
     // Perform the raycast
     RaycastHit hit;
     if (Physics.Raycast(rayOrigin, rayDirection, out hit, raycastDistance))
     {
          // The ray hit something
          Debug.Log("Raycast hit: " + hit.collider.name);
          //lineRend.enabled = true;
         // lineRend.SetPosition(0, transform.position);
          //lineRend.SetPosition(1, hit.point);
          //Destroy(hit.transform.gameObject);
     }
     else
     {
          // The ray did not hit anything
          Debug.Log("Raycast did not hit anything.");
          //lineRend.enabled = false;
     }*/
}    
}

/*
private void depricated ()
{
     // intended for interpolation ratio of 1.0
          
     if (false)
     {
          // Adjust the step size to ensure more uniform distribution within the specified range
          float xStep = 360f / xSteps; // Assuming xSteps is the number of steps you want within the 360-degree range
          float yStep = 90f / ySteps; // Assuming ySteps is the number of steps you want within the 90-degree range

          for (float x = 0; x <= 360; x += xStep)
          {
               for (float y = -45; y <= 45; y += yStep)
               {
                    // Convert spherical coordinates to Cartesian coordinates
                    float xRad = x * Mathf.Deg2Rad;
                    float yRad = y * Mathf.Deg2Rad;

                    // Calculate the direction vector for the ray
                    // Rotate around the forward vector to create circles around it
                    Vector3 direction = Quaternion.Euler(0, x, 0) * transform.forward;
                    direction += Quaternion.Euler(y, 0, 0) * transform.right;

                    // Normalize the direction vector
                    direction = direction.normalized;

                    // Draw the ray
                    Debug.DrawLine(transform.position, transform.position + direction * drawDistance, Color.blue, lifeSpan, false);
               }
          }
     }



     if (gemini)
     {
          float stepAngle = angle / (segments - 1);

          for (int i = 0; i < segments; i++)
          {
               float currentAngle = stepAngle * i;
               Vector3 dir = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

               // Draw circle at current distance relative to the robot
               DrawCircle(transform.position + dir * radius * (i / (float)(segments - 1)), radius * (i / (float)(segments - 1)), segments);
          }
     }

     


     float xWidth = xAngle/2;
     float yWidth = yAngle/2;

     if (phind)
     {
          for (float x = -xWidth; x <= xWidth; x += stepDensity)
          {
               for (float y = -yWidth; y <= yWidth; y += stepDensity)
               {
                    // Convert spherical coordinates to Cartesian coordinates
                    float xRad = x * Mathf.Deg2Rad;
                    float yRad = y * Mathf.Deg2Rad;

                    // Calculate the direction vector for the ray
                    Vector3 direction = new Vector3(
                         Mathf.Sin(yRad) * Mathf.Cos(xRad),
                         Mathf.Cos(yRad),
                         Mathf.Sin(yRad) * Mathf.Sin(xRad)
                    );

                    // Ensure the direction vector is pointing in the same hemisphere as the forward vector
                    if (Vector3.Dot(direction, transform.forward) < 0)
                    {
                         direction *= -1;
                    }

                    // Draw the ray
                    Debug.DrawLine(transform.position, transform.position + direction.normalized * drawDistance, Color.blue, lifeSpan, false);
               }
          }
     }

     else if (normal)
     {
          for (float x = 0.0f; x < xSteps; x += stepDensity)
          {
               for (float y = 0.0f; y < ySteps; y += stepDensity)
               {
                    Debug.DrawLine(transform.position, transform.position + ( (transform.forward + Vector3.Lerp(-transform.up, transform.up, y) + Vector3.Lerp(-transform.right, transform.right, x)) ).normalized * drawDistance, Color.blue, lifeSpan, false);
               }
          }
     }
     else if (quads)
     {
          {
          for (float x = 0.0f; x < xSteps; x += stepDensity)
          {
               for (float y = 0.0f; y < ySteps; y += stepDensity)
               {
                    //Debug.DrawLine(transform.position, transform.position + (Vector3.Lerp(transform.up, -transform.up + transform.forward, y)).normalized * drawDistance, Color.green, lifeSpan, false);
                    //Debug.DrawLine(transform.position, transform.position + (Vector3.Lerp(transform.up, transform.forward, y)).normalized * drawDistance, Color.green, lifeSpan, false);

                    Debug.DrawLine(transform.position, transform.position + ( 
                         (Vector3.Lerp(transform.forward, transform.up, y) + 
                         Vector3.Lerp(transform.forward, -transform.right, x)) 
                    ) * drawDistance, Color.blue, lifeSpan, false);

               }
          }
     }
     }
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
}*/