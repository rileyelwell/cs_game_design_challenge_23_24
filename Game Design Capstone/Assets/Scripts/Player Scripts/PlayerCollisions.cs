using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{

    private Dictionary<Collider, float> lastProcessedTimes = new Dictionary<Collider, float>();
    private float cooldownTime = 1f;

    private void OnCollisionEnter(Collision collision) {
        Collider collider = collision.collider;
        float currentTime = Time.time;

        // Check if this collider has been processed within the cooldown period
        if (lastProcessedTimes.ContainsKey(collider))
        {
            float lastProcessedTime = lastProcessedTimes[collider];
            if (currentTime - lastProcessedTime < cooldownTime)
            {
                return;
            }
        }

        // Update the last processed time for this collider
        lastProcessedTimes[collider] = currentTime;

        // player loses 10% health when hit by a pedestrain
        if (collider.gameObject.tag == TagManager.PEDESTRIAN_TAG)
        {
            ScoreHandler.instance.SetRobotHealth(0.1f);

            // handle small extra force hitting robot from human
        }

        // player loses 30% health when hit by a vehicle
        if (collider.gameObject.tag == TagManager.VEHICLE_TAG)
        {
            ScoreHandler.instance.SetRobotHealth(0.3f);

            // handle hitting large force onto robot from car
        }
        
        // player loses 100% health when hit by a train
        if (collider.gameObject.tag == TagManager.TRAIN_TAG)
        {
            ScoreHandler.instance.SetRobotHealth(1f);

            // handle hitting large force onto robot from train
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // remove the collider from the processed set when the collision ends
        //lastProcessedTimes.Remove(collision.collider);
    }


}
