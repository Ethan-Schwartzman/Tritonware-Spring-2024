using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum RotateDirection
{
    Up, Down
}

public class ShipMovement : MonoBehaviour
{
    public float TorqueMultiplier = 1000;
    public float InitialMaxTorqueMultiplier = 3;
    public float InitialTorqueBoostAngSpeedThreshold = 20;
    public float AngularDrag = 5;
    public float MaxAngSpeed = 120;
    public float AngularDamping = 1;
    public float AngularDampingVelocityThreshold = 10;
    public float HoldRotateMultiplier = 2;
    public float InitialRotatePower = 0.2f;

    private Rigidbody2D rb;
    public static ShipMovement Instance;
    private float currentTorque;
    private float upDuration, downDuration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            Debug.LogWarning("Tried to create more than one instance of InputManager");
        }
    }

    
    private float TurningMultiplier(float angVel, float inputTorque)
    {
        if (Mathf.Sign(angVel) != Mathf.Sign(inputTorque)) return InitialMaxTorqueMultiplier;
        if (Mathf.Abs(rb.angularVelocity) > MaxAngSpeed) return 0;
        return Mathf.Clamp(InitialMaxTorqueMultiplier -
            Mathf.Abs(angVel) * (InitialMaxTorqueMultiplier - 1 
            / InitialTorqueBoostAngSpeedThreshold),
            1, InitialMaxTorqueMultiplier);
    }

    private float Damping(float angVel)
    {

        return -Mathf.Clamp(angVel / AngularDampingVelocityThreshold, -1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTorque == 0)
        {
            rb.AddTorque(Damping(rb.angularVelocity) * AngularDamping * Time.deltaTime);
        }
        else
        rb.AddTorque(currentTorque * TorqueMultiplier * TurningMultiplier(rb.angularVelocity, currentTorque) * Time.deltaTime,
            ForceMode2D.Force);
        

    }


    public void Rotate(float torque)
    {

        
        if (torque > 0)
        {
            downDuration = InitialRotatePower;
            if (upDuration < 1) upDuration += HoldRotateMultiplier * Time.deltaTime;
            currentTorque = torque * upDuration;
        }
        else if (torque < 0)
        {
            upDuration = InitialRotatePower;
            if (upDuration < 1) downDuration += HoldRotateMultiplier * Time.deltaTime;
            currentTorque = torque * downDuration;
        }
        else
        {
            upDuration = InitialRotatePower;
            downDuration = InitialRotatePower;
            currentTorque = 0;
        }
        
        
    }

}
