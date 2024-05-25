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

    [SerializeField] private float roboBoost = 1.0f;

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
        //GetWarnings();
        CheckWarnings(frontWarning, backWarning, leftWarning, rightWarning);

        // send the bool flags to be displayed accordingly
        gameObject.GetComponent<DisplayADAS>().DisplaySensorActive(frontWarning, backWarning, leftWarning, rightWarning);

        if (Input.GetKey(KeyCode.Space))
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0.0f;

        if (Input.GetKey(KeyCode.LeftShift))
            roboBoost = 1.5f;
        else
            roboBoost = 1f;

        currentRightAcceleration = rightModifier * rightAcceleration * Input.GetAxis("RightVertical") * roboBoost;
        currentLeftAcceleration = leftModifier * leftAcceleration * Input.GetAxis("LeftVertical") * roboBoost;

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

    public void GetWarnings(int cordSurround, int cordHeight)
    {
        // front warning
        if (cordSurround >= 6 && cordSurround <= 10)
        {
            // print("Front Sensor Active!");
            frontWarning = true;
        }

        if (cordSurround >= 14 || cordSurround <= 2)
        {
            // print("Back Sensor Active!");
            backWarning = true;
        } 

        if (cordSurround >= 2 && cordSurround <= 6)
        {
            // print("Left Sensor Active!");
            leftWarning = true;
        }

        if (cordSurround >= 10 && cordSurround <= 14)
        {
            // print("Right Sensor Active!");
            rightWarning = true;
        }
        
        CheckWarnings(frontWarning, backWarning, leftWarning, rightWarning);

        // send the bool flags to be displayed accordingly
        gameObject.GetComponent<DisplayADAS>().DisplaySensorActive(frontWarning, backWarning, leftWarning, rightWarning);
    }

    public void CheckWarnings(bool frontWarning1, bool backWarning1, bool leftWarning1, bool rightWarning1)
    {
        // Check front warning (won't slow down if something is behind it)
        if (frontWarning1 && !backWarning1)
        {
            leftModifier = -0.5f;
            rightModifier = -0.5f;
        }

        // Check back warning
        else if (backWarning1)
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
        if (leftWarning1)
            rightModifier *= 0.5f;

        // Check right warning
        else if (rightWarning1)
            leftModifier *= 0.5f;

        /* might need to add warning for curbs or other specific scenarios */
    }

    public void ResetSensorWarnings() {
        frontWarning = false;
        backWarning = false;
        rightWarning = false;
        leftWarning = false;
    }
}
