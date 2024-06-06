using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;          // Target to follow (the player)
    [SerializeField] private float smoothSpeed = 0.05f; // Smooth transistion into movement
    [SerializeField] private Vector3 offset;            // Offset behind the target

    /*
     * Name: FixedUpdate (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Follows the player with the camera
     */
    void FixedUpdate ()
    {
        Vector3 desiredPosition = target.position - target.forward + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
