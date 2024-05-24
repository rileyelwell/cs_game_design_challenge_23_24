using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFlip : MonoBehaviour
{
    
    private Vector3 currentPosition;
    private Quaternion correctRotation;

    [SerializeField] private List<Transform> wheelTransforms;

    private bool isFlipped;

    private void Start() {
        correctRotation = transform.rotation;
        isFlipped = false;
    }

    private void Update() {
        // currentPosition = transform.position;

        // check if robot has flipped
        // CheckForFlipped();

        // if (AreWheelsOffGround())
        // {
            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerResetFlip();
            }
        // }
    }

    private void PlayerResetFlip()
    {
        // handle the new position (drop from slight height)
        currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, currentPosition.y + 0.5f, currentPosition.z);

        // handle the rotation (flip over, but keep pointing same direction of y axis)
        Quaternion currentRotation = transform.rotation;
        Quaternion newRotation = Quaternion.Euler(correctRotation.x, currentRotation.eulerAngles.y, correctRotation.z);
        transform.rotation = newRotation;

        print("Flipping Robot!");
    }

    private bool AreWheelsOffGround()
    {
        // Check if the wheels are touching the ground using raycasts
        float rayLength = 0.3f; // Adjust based on your robot's wheel size and distance to ground
        bool wheelsOffGround = true;

        foreach (Transform wheel in wheelTransforms)
        {
            if (Physics.Raycast(wheel.position, Vector3.down, rayLength))
            {
                print("At least one wheel is on the ground");
                wheelsOffGround = false;
                break;
            }
        }

        return wheelsOffGround;
    }

    // private void OnCollisionStay(Collision collision)
    // {
    //     // Check if any collision is happening with the ground
    //     foreach (ContactPoint contact in collision.contacts)
    //     {
    //         if (contact.otherCollider.CompareTag("Ground"))
    //         {
    //             isFlipped = false;
    //             return;
    //         }
    //     }
    //     isFlipped = true;
    // }

    // private void OnCollisionExit(Collision collision)
    // {
    //     if (collision.collider.CompareTag("Ground"))
    //     {
    //         isFlipped = true;
    //     }
    // }









    public void SetBoolFlipped(bool flag) 
    {
        isFlipped = flag;
    }


    // private void CheckForFlipped()
    // {
    //     // print(transform.eulerAngles);

    //     if (transform.eulerAngles.z <= -70 || transform.eulerAngles.z >= 70)
    //     {
    //         print("Robot flipped on z axis");
    //         isFlipped = true;
    //     }

    //     else if (transform.eulerAngles.x < -70 || transform.eulerAngles.z >= 70)
    //     {
    //         print("Robot flipped on x axis");
    //         isFlipped = true;
    //     }

    //     else 
    //     {
    //         // print("Robot should not be able to flip");
    //         isFlipped = false;
    //     }
    // }

}
