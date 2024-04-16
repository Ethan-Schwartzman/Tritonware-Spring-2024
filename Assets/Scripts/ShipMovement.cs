using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum RotateDirection {
    Up, Down
}

public class ShipMovement : MonoBehaviour {

    // * = feel free to tweak, otherwise don't touch the value
    [SerializeField] private float torqueMultiplier = 2000; // * how fast you rotate
    [SerializeField] private float initialMaxTorqueMultiplier = 2;
    [SerializeField] private float initialTorqueBoostAngSpeedThreshold = 80;
    [SerializeField] private float maxAngSpeed = 250; // * the speed limit of rotation
    [SerializeField] private float angularDamping = 500; // * how quickly rotation stops when not actively applying torque
    [SerializeField] private float angularDampingVelocityThreshold = 120;
    [SerializeField] private float holdRotateMultiplier = 2;
    [SerializeField] private float initialRotatePower = 0.2f;
    [SerializeField] private float liftMultiplier = 100; // * how much lift (force that corrects your velocity to align to your facing direction) applies
    [SerializeField] private float topSpeed = 30; // * top speed through thrust only
    [SerializeField] private float excessSpeedDrag = 100f; // * how quickly you slow down if you go past the top speed
    [SerializeField] private float aoaDrag = 50; // * how much you slow down when you turn

    public float accumulatedThrust = 0;
    public float defaultThrust = 200;
    private float currentThrust = 200;

    private Rigidbody2D rb;
    public static ShipMovement Instance;
    private float currentTorque;
    private float upDuration, downDuration;

    public GameObject sm;

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

    public float GetSpeed() 
    {
        return rb.velocity.magnitude;   
    }

    public Vector2 GetVelocity() 
    {
        return rb.velocity;
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
        rb.AddForce((GetThrust() - GetSpeedDamping()) * Time.deltaTime * rb.mass);
        rb.AddForce(GetExcessSpeedDrag()  * Time.deltaTime * rb.mass);
        rb.AddForce(GetAOADrag() * Time.deltaTime * rb.mass);
        rb.AddForce(GetLift() * Time.deltaTime * rb.mass);
        DebugRenderer.lineRenderer1.SetPosition(0, transform.position);
        DebugRenderer.lineRenderer1.SetPosition(1, transform.position + (Vector3)GetLift() * 0.01f);
        DebugRenderer.lineRenderer2.SetPosition(0, transform.position);
        DebugRenderer.lineRenderer2.SetPosition(1, transform.position + (Vector3)rb.velocity * 0.5f);
        //DebugRenderer.lineRenderer3.SetPosition(0, transform.position);
        //DebugRenderer.lineRenderer3.SetPosition(1, transform.position - (Vector3)GetSpeedDamping() * 0.01f);

        // Debug.Log($"Speed: {GetSpeed()} Thrust Damping: {GetSpeedDamping()}, Drag Damping: {GetExcessSpeedDrag()}, AOA Drag: {GetAOADrag()}");
    }

    private Vector2 GetSpeedDamping()
    {
        // function between 1 and 0 that adjusts thrust based on current speed.
        if (Mathf.Abs(AngleOfAttack()) > 90) return Vector2.zero;
        float speed = GetSpeed();
        Vector2 vhat = GetMovingDirection();
        if (speed < topSpeed)
        {
            
            return (1f - Mathf.Pow(1f - Mathf.Pow(speed, 2) /
        Mathf.Pow(topSpeed, 2), 1f / 3f)) * (Vector2.Dot(vhat,GetThrust()) * vhat);
        }
        return (Vector2.Dot(vhat, GetThrust()) * vhat);

    }

    private Vector2 GetExcessSpeedDrag()
    {
        if (GetSpeed() > topSpeed)
        return -GetMovingDirection() * Mathf.Clamp01(Mathf.Pow((GetSpeed() - topSpeed) / topSpeed, 2)) 
               * excessSpeedDrag;
        return Vector2.zero;
    }

    private Vector2 GetAOADrag()
    {
        return -GetMovingDirection() * Mathf.Sin(Mathf.Deg2Rad * Mathf.Abs(AngleOfAttack())) * aoaDrag;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        StartCoroutine(OutageControl());
    }

    public IEnumerator OutageControl(){
        sm.GetComponent<ScreenManager>().Outage();
        yield return new WaitForSeconds(0.4f);
        sm.GetComponent<ScreenManager>().Recovery();
    }

}
