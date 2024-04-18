using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyShipMovement : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerShipMovement player;

    Vector2 targetRelativePos = new Vector2(20f, 0);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerShipMovement.Instance;
    }

    public bool inertialDrift = true;
    public bool compensation = true;

    public float compensationFactor = 5f;
    public float dampingFactor = 1.0f;

    private void Update()
    {
        rb.AddForce(GetCompensation() * Time.deltaTime);
        rb.AddForce(GetDamping() * Time.deltaTime);
    }

    Vector2 GetRelativePosition()
    {
        return transform.position - player.transform.position;
    }

    Vector2 GetRelativeVelocity()
    {
        return rb.velocity - player.GetVelocity();
    }

    Vector2 GetInertialDrift()
    {
        if (!inertialDrift) return Vector2.zero;
        return -player.GetDv() * 5000;
    }

    Vector2 GetCompensation()
    {

        if (!compensation) return Vector2.zero;
        Vector2 dr = targetRelativePos - GetRelativePosition();
        return compensationFactor * dr;
    }

    Vector2 GetDamping()
    {
        return -GetRelativeVelocity() * dampingFactor;
    }

}
