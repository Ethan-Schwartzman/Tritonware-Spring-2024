using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotateDirection
{
    Up, Down
}

public class ShipMovement : MonoBehaviour
{
    public float TorqueMultiplier;
    private Rigidbody2D rb;
    public static ShipMovement Instance;
    private float currentTorque;

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

    


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddTorque(currentTorque * TorqueMultiplier * Time.deltaTime);
    }

    public void Rotate(float torque)
    {
        currentTorque = torque;
    }

}
