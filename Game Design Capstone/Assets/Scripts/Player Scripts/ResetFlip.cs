using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFlip : MonoBehaviour
{
    private Vector3 currentPosition;
    private Quaternion correctRotation;
    [SerializeField] private List<Transform> wheelTransforms;

    /*
     * Name: Start (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Sets initial values
     */
    private void Start() {
        correctRotation = transform.rotation;
    }

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Checks for player input to flip robot
     */
    private void Update() {
            if (Input.GetButtonDown("Reset"))
                PlayerResetFlip();
    }

    /*
     * Name: PlayerResetFlip
     * Inputs: none
     * Outputs: none
     * Description: Flips the player right side up
     */
    private void PlayerResetFlip()
    {
        // handle the new position (drop from slight height)
        currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, currentPosition.y + 0.5f, currentPosition.z);

        // handle the rotation (flip over, but keep pointing same direction of y axis)
        Quaternion currentRotation = transform.rotation;
        Quaternion newRotation = Quaternion.Euler(correctRotation.x, currentRotation.eulerAngles.y, correctRotation.z);
        transform.rotation = newRotation;
    }
}