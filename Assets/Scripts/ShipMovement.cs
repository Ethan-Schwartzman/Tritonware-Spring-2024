using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum RotateDirection {
    Up, Down
}

public class ShipMovement : MonoBehaviour {

    // * = feel free to tweak, otherwise don't touch the value
    [SerializeField] private float torqueMultiplier = 2000; // *
    [SerializeField] private float initialMaxTorqueMultiplier = 2;
    [SerializeField] private float initialTorqueBoostAngSpeedThreshold = 80;
    [SerializeField] private float maxAngSpeed = 250; // *
    [SerializeField] private float angularDamping = 500; // *
    [SerializeField] private float angularDampingVelocityThreshold = 120;
    [SerializeField] private float holdRotateMultiplier = 2;
    [SerializeField] private float initialRotatePower = 0.2f;
    [SerializeField] private float liftMultiplier = 100; // *

    public float accumulatedThrust = 0;
    public float defaultThrust = 200;
    private float currentThrust = 200;

    private Rigidbody2D rb;
    public static ShipMovement Instance;
    private float currentTorque;
    private float upDuration, downDuration;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();

        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
            Debug.LogWarning("Tried to create more than one instance of InputManager");
        }
    }

    public Vector2 GetFacingDirection() {
        return transform.up;
    }

    public Vector2 GetMovingDirection()
    {
        return rb.velocity.normalized;
    }

    private float AngleOfAttack()
    {
        // positive = facing down relative to velocity
        return Vector2.SignedAngle(GetFacingDirection(), rb.velocity);
    }

    private float TurningMultiplier(float angVel, float inputTorque) {
        // returns a multiplier to turning torque based on current angular velocity
        if (Mathf.Sign(angVel) != Mathf.Sign(inputTorque)) return initialMaxTorqueMultiplier;
        if (Mathf.Abs(rb.angularVelocity) > maxAngSpeed) return 0;
        return Mathf.Clamp(initialMaxTorqueMultiplier -
            Mathf.Abs(angVel) * (initialMaxTorqueMultiplier - 1
            / initialTorqueBoostAngSpeedThreshold),
            1, initialMaxTorqueMultiplier);
    }

    // Update is called once per frame
    void Update() {


        rb.AddTorque(GetTorque() * Time.deltaTime * rb.mass);
        rb.AddForce(GetThrust() * Time.deltaTime * rb.mass);
        rb.AddForce(GetLift() * Time.deltaTime * rb.mass);
        Debug.Log(AngleOfAttack());
        DebugRenderer.lineRenderer1.SetPosition(0, transform.position);
        DebugRenderer.lineRenderer1.SetPosition(1, transform.position + (Vector3)GetLift() * 0.01f);
        DebugRenderer.lineRenderer2.SetPosition(0, transform.position);
        DebugRenderer.lineRenderer2.SetPosition(1, transform.position + (Vector3)rb.velocity * 0.5f);
    }

    public void Boost(bool toggle)
    {
        if (toggle)
        {
            currentThrust = 0;
            if (accumulatedThrust < 300)
            {
                accumulatedThrust += Time.deltaTime * 100;
            }
            // Debug.Log(accumulatedThrust);
        }
        else
        {
            currentThrust = defaultThrust;
            if (accumulatedThrust > 0)
            {
                currentThrust += accumulatedThrust;
                accumulatedThrust -= Time.deltaTime * 100;
                // Debug.Log(accumulatedThrust);
            }
        }
    }

    private float RotateDamping(float angVel)
    {
        // returns a multiplier from -1 to 1 that dampens ship rotation
        return -Mathf.Clamp(angVel * angVel * Mathf.Sign(angVel) / angularDampingVelocityThreshold, -1, 1);
    }
    private float GetTorque() {
        if (currentTorque == 0) {
            return RotateDamping(rb.angularVelocity) * angularDamping;
        }
        else
            return currentTorque * torqueMultiplier * TurningMultiplier(rb.angularVelocity, currentTorque);
    }


    private Vector2 GetThrust() {
        return GetFacingDirection() * currentThrust;
        //rb.velocity = 5 * GetDirection();
    }

    private Vector2 GetLift()
    {
        // force perpendicular to ship movement that turns the ship towards its heading direction
        return liftMultiplier * Mathf.Sin(Mathf.Deg2Rad * AngleOfAttack()) * rb.velocity.magnitude * -Vector2.Perpendicular(GetMovingDirection());
    }


    public void Rotate(float torque) {
        // applies torque to the ship based on input

        if (torque > 0) {
            downDuration = initialRotatePower;
            if (upDuration < 1) upDuration += holdRotateMultiplier * Time.deltaTime;
            currentTorque = torque * upDuration;
        }
        else if (torque < 0) {
            upDuration = initialRotatePower;
            if (upDuration < 1) downDuration += holdRotateMultiplier * Time.deltaTime;
            currentTorque = torque * downDuration;
        }
        else {
            upDuration = initialRotatePower;
            downDuration = initialRotatePower;
            currentTorque = 0;
        }


    }

}
