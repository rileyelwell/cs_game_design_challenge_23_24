using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;          // Target to follow (the player)
    [SerializeField] private float smoothSpeed = 0.1f; // Smooth transistion into movement
    [SerializeField] private Vector3 offset;            // Offset behind the target
    [SerializeField] private float FOV = 1f, alternateFOV = 2f;
    [SerializeField] private float cameraTurn = 2.5f;

    private bool isAlternateFOV = false, isFrontView = false;                // Track if alternate FOV is active

    void Update()
    {
        // Check for FOV toggle input
        if (Input.GetButtonDown("FOVToggle"))
        {
            isAlternateFOV = !isAlternateFOV;
            FOV = isAlternateFOV ? alternateFOV : 1f;
        }
        // Check for swap view input
        if (Input.GetButtonDown("ReverseCam"))
            isFrontView = !isFrontView;
    }

    /*
     * Name: FixedUpdate (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Follows the player with the camera
     */
    void FixedUpdate ()
    {
        Vector3 desiredPosition;

        if (isFrontView)
            desiredPosition = target.position + target.forward * FOV + offset;
        else
            desiredPosition = target.position - target.forward * FOV + offset;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("CameraTurn") > 0.5f)
            desiredPosition -= target.right * cameraTurn;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("CameraTurn") < -0.5f)
            desiredPosition += target.right * cameraTurn;

        // Vector3 desiredPosition = target.position + target.right * offset.x + target.up * offset.y - target.forward * FOV;
        // Vector3 desiredPosition = target.position - target.forward * FOV + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
