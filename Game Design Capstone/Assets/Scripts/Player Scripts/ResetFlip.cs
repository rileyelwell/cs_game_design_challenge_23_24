using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFlip : MonoBehaviour
{
    private Vector3 currentPosition;
    private Quaternion correctRotation;
    private bool canFlip;
    private float cooldownTime = 0.5f;
    [SerializeField] private float resetUpValue = 0.5f;

    /*
     * Name: Start (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Sets initial values
     */
    private void Start() {
        correctRotation = transform.rotation;
        canFlip = true;
    }

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Checks for player input to flip robot
     */
    private void Update() {
        if (Input.GetButtonDown("Reset") && canFlip && GameplayManager.instance.canPause && !GameplayManager.instance.isPaused)
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
        transform.position = new Vector3(currentPosition.x, currentPosition.y + resetUpValue, currentPosition.z);

        // handle the rotation (flip over, but keep pointing same direction of y axis)
        Quaternion currentRotation = transform.rotation;
        Quaternion newRotation = Quaternion.Euler(correctRotation.x, currentRotation.eulerAngles.y, correctRotation.z);
        transform.rotation = newRotation;

        StartCoroutine(FlipCooldown());
    }

    IEnumerator FlipCooldown()
    {
        canFlip = false;
        yield return new WaitForSeconds(cooldownTime);
        canFlip = true;
    }
}