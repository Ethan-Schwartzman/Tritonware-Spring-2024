using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum RotateDirection {
    Up, Down
}

public class ShipMovement : MonoBehaviour {
    public float TorqueMultiplier = 1000;
    public float InitialMaxTorqueMultiplier = 3;
    public float InitialTorqueBoostAngSpeedThreshold = 20;
    public float MaxAngSpeed = 120;
    public float AngularDamping = 1;
    public float AngularDampingVelocityThreshold = 10;
    public float HoldRotateMultiplier = 2;
    public float InitialRotatePower = 0.2f;

    public float accumulatedThrust = 0;
    public float defaultThrust = 200;
    public float Thrust = 200;

    public float LiftMultiplier = 1000;

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
        return Vector2.SignedAngle(GetFacingDirection(), rb.velocity);
    }

    private float TurningMultiplier(float angVel, float inputTorque) {
        if (Mathf.Sign(angVel) != Mathf.Sign(inputTorque)) return InitialMaxTorqueMultiplier;
        if (Mathf.Abs(rb.angularVelocity) > MaxAngSpeed) return 0;
        return Mathf.Clamp(InitialMaxTorqueMultiplier -
            Mathf.Abs(angVel) * (InitialMaxTorqueMultiplier - 1
            / InitialTorqueBoostAngSpeedThreshold),
            1, InitialMaxTorqueMultiplier);
    }



    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.A)) {
            Thrust = 0;
            if (accumulatedThrust < 300) {
                accumulatedThrust += Time.deltaTime * 100;
            }
            // Debug.Log(accumulatedThrust);
        }
        else {
            Thrust = defaultThrust;
            if (accumulatedThrust > 0) {
                Thrust += accumulatedThrust;
                accumulatedThrust -= Time.deltaTime * 100;
                // Debug.Log(accumulatedThrust);
            }
        }
        rb.AddTorque(GetTorque() * Time.deltaTime * rb.mass);
        rb.AddForce(GetThrust() * Time.deltaTime * rb.mass);
        rb.AddForce(GetLift() * Time.deltaTime * rb.mass);
        Debug.Log(AngleOfAttack());
        DebugRenderer.lineRenderer1.SetPosition(0, transform.position);
        DebugRenderer.lineRenderer1.SetPosition(1, transform.position + (Vector3)GetLift() * 0.01f);
        DebugRenderer.lineRenderer2.SetPosition(0, transform.position);
        DebugRenderer.lineRenderer2.SetPosition(1, transform.position + (Vector3)rb.velocity * 0.5f);
    }

    private float RotateDamping(float angVel)
    {

        return -Mathf.Clamp(angVel * angVel * Mathf.Sign(angVel) / AngularDampingVelocityThreshold, -1, 1);
    }
    private float GetTorque() {
        if (currentTorque == 0) {
            return RotateDamping(rb.angularVelocity) * AngularDamping;
        }
        else
            return currentTorque * TorqueMultiplier * TurningMultiplier(rb.angularVelocity, currentTorque);
    }


    private Vector2 GetThrust() {
        return GetFacingDirection() * Thrust;
        //rb.velocity = 5 * GetDirection();
    }

    private Vector2 GetLift()
    {
        return LiftMultiplier * Mathf.Sin(Mathf.Deg2Rad * AngleOfAttack()) * rb.velocity.magnitude * -Vector2.Perpendicular(GetMovingDirection());
    }


    public void Rotate(float torque) {


        if (torque > 0) {
            downDuration = InitialRotatePower;
            if (upDuration < 1) upDuration += HoldRotateMultiplier * Time.deltaTime;
            currentTorque = torque * upDuration;
        }
        else if (torque < 0) {
            upDuration = InitialRotatePower;
            if (upDuration < 1) downDuration += HoldRotateMultiplier * Time.deltaTime;
            currentTorque = torque * downDuration;
        }
        else {
            upDuration = InitialRotatePower;
            downDuration = InitialRotatePower;
            currentTorque = 0;
        }


    }

}
