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

    public float rightAcceleration = 1000.0f;
    public float leftAcceleration = 1000.0f;
    public float breakingForce = 300.0f;

    private float currentRightAcceleration = 0.0f;
    private float currentLeftAcceleration = 0.0f;
    private float currentBreakForce = 0.0f;

    void FixedUpdate ()
    {

        if (Input.GetKey(KeyCode.Space))
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0.0f;

        currentRightAcceleration = -1 * rightAcceleration * Input.GetAxis("RightVertical");
        currentLeftAcceleration = -1 * leftAcceleration * Input.GetAxis("LeftVertical");

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
}
