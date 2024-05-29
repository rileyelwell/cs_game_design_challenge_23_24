using System;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private WheelCollider frwheel, flwheel, mrwheel, mlwheel, brwheel, blwheel;// Wheel colliders
    [SerializeField] private Transform frtrans, fltrans, mrtrans, mltrans, brtrans, bltrans;    // Wheel transforms
    [SerializeField] private float acceleration = 15.0f, breakingForce = 15.0f;                 // Max speed variables
    [SerializeField] private float roboBoost = 1.0f, roboBoostUseRate = 0.3f;                   // Sprint (RoboBoost) variables
    private float currentRightAcceleration, currentLeftAcceleration, currentBreakForce;         // Current speed variabels
    private bool frontWarning, backWarning, rightWarning, leftWarning;                          // ADAS triggered variables
    private float leftModifier, rightModifier;                                                  // ADAS effect variables

    /*
     * Name: Awake (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Sets the initial values for player variables
     */
    void Awake ()
    {
        currentBreakForce = 0.0f;
        currentRightAcceleration = 0.0f;
        currentLeftAcceleration = 0.0f;
        frontWarning = false;
        backWarning = false;
        rightWarning = false;
        leftWarning = false;
        leftModifier = -1.0f;
        rightModifier = -1.0f;
    }

    /*
     * Name: FixedUpdate (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Steadily handles the changes that occur each frame
     */
    void FixedUpdate ()
    {
        // Apply any ADAS intervention
        ADAS();

        // Update ADAS UI using warning booleans
        UIManager.instance.UpdateSensorDisplay(frontWarning, backWarning, leftWarning, rightWarning);

        // Handle breaking with space
        if (Input.GetKey(KeyCode.Space))
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0.0f;

        // Handle the roboboost (sprint) with left shift
        if (Input.GetKey(KeyCode.LeftShift) && !UIManager.instance.isBatteryOnCooldown)
        {
            roboBoost = 1.5f;
            UIManager.instance.UpdateBatteryDisplay(roboBoostUseRate);
        }
        else
        {
            roboBoost = 1f;
            UIManager.instance.UpdateBatteryDisplay(-roboBoostUseRate / 3);
        }

        // Update right and left acceleration to be a product of everything effecting the robots movement
        currentRightAcceleration = rightModifier * acceleration * Input.GetAxis("RightVertical") * roboBoost;
        currentLeftAcceleration = leftModifier * acceleration * Input.GetAxis("LeftVertical") * roboBoost;

        // Update each of the wheels
        UpdateWheel(frwheel, frtrans, currentRightAcceleration);
        UpdateWheel(flwheel, fltrans, currentLeftAcceleration);
        UpdateWheel(mrwheel, mrtrans, currentRightAcceleration);
        UpdateWheel(mlwheel, mltrans, currentLeftAcceleration);
        UpdateWheel(brwheel, brtrans, currentRightAcceleration);
        UpdateWheel(blwheel, bltrans, currentLeftAcceleration);

    }


    /*
     * Name: UpdateWheel
     * Inputs: wheel collider, wheel transform, wheel acceleration
     * Outputs: none
     * Description: Applies a given acceleration to a given wheel
     */
    void UpdateWheel (WheelCollider wheel, Transform trans, float acc)
    {
        // Apply movement (acceleration and breaking) to the wheel collider
        wheel.motorTorque = acc;
        wheel.brakeTorque = currentBreakForce;

        // Get the position of the wheel collider
        Vector3 position;
        Quaternion rotation;
        wheel.GetWorldPose(out position, out rotation);

        // Apply the position of the wheel collider to the wheel model's transform
        trans.position = position;
        trans.rotation = rotation;
    }

    /*
     * Name: ADAS
     * Inputs: none
     * Outputs: none
     * Description: Updates the acceleration modifiers based off of sensor warnings to alter the robot movement, mimicking ADAS
     */
    public void ADAS()
    {
        // Check for front warning (won't slow down if something is behind it) to slow down
        if (frontWarning && !backWarning)
        {
            leftModifier = -0.5f;
            rightModifier = -0.5f;
        }

        // Check for back warning to speed up
        else if (backWarning)
        {
            leftModifier = -1.25f;
            rightModifier = -1.25f;
        }

        // Check for no front or back warnings to reset modifiers
        else
        {
            leftModifier = -1.0f;
            rightModifier = -1.0f;
        }

        // Check for left warning to swerve right
        if (leftWarning)
            rightModifier *= 0.5f;

        // Check for right warning to swerve left
        else if (rightWarning)
            leftModifier *= 0.5f;

        /* might need to add warning for curbs, gradual speed changes, or other specific scenarios */
    }

    /*
     * Name: SetWarnings
     * Inputs: sensor side, sesnor value
     * Outputs: none
     * Description: Sets sensor warning information (Used in Ray.cs)
     */
    public void SetWarning(char side, bool value)
    {
        switch(side)
        {
            case 'f':   // Set forward warning
                frontWarning = value;   break;
            case 'b':   // Set backward warning
                backWarning = value;    break;
            case 'l':   // Set left warning
                leftWarning = value;    break;
            case 'r':   // Set right warning
                rightWarning = value;   break;
            default:
                return;
        }
    }
}
