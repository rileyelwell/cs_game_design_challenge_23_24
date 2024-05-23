using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] WheelCollider frwheel;
    [SerializeField] WheelCollider flwheel;
    [SerializeField] WheelCollider mrwheel;
    [SerializeField] WheelCollider mlwheel;
    [SerializeField] WheelCollider brwheel;
    [SerializeField] WheelCollider blwheel;

    [SerializeField] Transform frtransform;
    [SerializeField] Transform fltransform;
    [SerializeField] Transform mrtransform;
    [SerializeField] Transform mltransform;
    [SerializeField] Transform brtransform;
    [SerializeField] Transform bltransform;

    public float rightAcceleration = 500.0f;
    public float leftAcceleration = 500.0f;
    public float breakingForce = 300.0f;

    private bool frontWarning = false;
    private bool backWarning = false;
    private bool rightWarning = false;
    private bool leftWarning = false;

    private float leftModifier = -1.0f;
    private float rightModifier = -1.0f;

    private float currentRightAcceleration = 0.0f;
    private float currentLeftAcceleration = 0.0f;
    private float currentBreakForce = 0.0f;

    void Start ()
    {
        frontWarning = false;
        backWarning = false;
        rightWarning = false;
        leftWarning = false;
        leftModifier = -1.0f;
        rightModifier = -1.0f;
    }

    void FixedUpdate ()
    {
        GetWarnings();
        CheckWarnings();

        if (Input.GetKey(KeyCode.Space))
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0.0f;

        currentRightAcceleration = rightModifier * rightAcceleration * Input.GetAxis("RightVertical");
        currentLeftAcceleration = leftModifier * leftAcceleration * Input.GetAxis("LeftVertical");

        frwheel.motorTorque = currentRightAcceleration;
        mrwheel.motorTorque = currentRightAcceleration;
        brwheel.motorTorque = currentRightAcceleration;

        flwheel.motorTorque = currentLeftAcceleration;
        mlwheel.motorTorque = currentLeftAcceleration;
        blwheel.motorTorque = currentLeftAcceleration;

        flwheel.brakeTorque = currentBreakForce;
        frwheel.brakeTorque = currentBreakForce;
        mrwheel.brakeTorque = currentBreakForce;
        mlwheel.brakeTorque = currentBreakForce;
        brwheel.brakeTorque = currentBreakForce;
        blwheel.brakeTorque = currentBreakForce;

        UpdateWheel(frwheel, frtransform);
        UpdateWheel(flwheel, fltransform);
        UpdateWheel(mrwheel, mrtransform);
        UpdateWheel(mlwheel, mltransform);
        UpdateWheel(brwheel, brtransform);
        UpdateWheel(blwheel, bltransform);

    }

    void UpdateWheel (WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);
        trans.position = position;
        trans.rotation = rotation;
    }

    void GetWarnings()
    {
        /* replace with gathering sensor data to determine if there should be a warning */
        frontWarning = false;
        backWarning = false;
        rightWarning = false;
        leftWarning = false;
    }

    void CheckWarnings()
    {
        // Check front warning (won't slow down if something is behind it)
        if (frontWarning && !backWarning)
        {
            leftModifier = -0.5f;
            rightModifier = -0.5f;
        }

        // Check back warning
        else if (backWarning)
        {
            leftModifier = -1.25f;
            rightModifier = -1.25f;
        }

        // Check no front or back warnings
        else
        {
            leftModifier = -1.0f;
            rightModifier = -1.0f;
        }

        // Check left warning
        if (leftWarning)
            rightModifier *= 0.5f;

        // Check right warning
        else if (rightWarning)
            leftModifier *= 0.5f;

        /* might need to add warning for curbs or other specific scenarios */
    }
}
