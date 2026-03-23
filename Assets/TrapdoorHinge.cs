using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class TrapdoorHinge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhysicalToggleButton button;

    [Header("Angles")]
    [SerializeField] private float closedAngle = 0f;    // resting position
    [SerializeField] private float openAngle = -90f;    // flipped down position

    [Header("Motor Settings")]
    [SerializeField] private float motorSpeed = 90f;    // degrees per second
    [SerializeField] private float motorForce = 150f;   // how much force the motor applies

    private HingeJoint _hinge;

    void Start()
    {
        _hinge = GetComponent<HingeJoint>();

        _hinge.useMotor = true;
        _hinge.useLimits = true;

        JointLimits limits = _hinge.limits;
        limits.min = Mathf.Min(closedAngle, openAngle);
        limits.max = Mathf.Max(closedAngle, openAngle);
        limits.bounciness = 0f;
        limits.bounceMinVelocity = 0f;
        _hinge.limits = limits;

        SetMotorTarget(closedAngle);
    }

    void Update()
    {
        if (button == null) return;

        bool isOn = button.IsToggled;

        if (isOn)
        {
            Debug.Log("[Trapdoor] Opening...");
            SetMotorTarget(openAngle);
        }
        else
        {
            Debug.Log("[Trapdoor] Closing...");
            SetMotorTarget(closedAngle);
        }

    }

    private void SetMotorTarget(float targetAngle)
    {
        float currentAngle = _hinge.angle;

        JointMotor motor = _hinge.motor;
        motor.targetVelocity = targetAngle > currentAngle ? motorSpeed : -motorSpeed;
        motor.force = motorForce;
        motor.freeSpin = false;
        _hinge.motor = motor;
    }
}