using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    Rigidbody2D rb;
    public float trackingForce = 100;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        float dt = Time.deltaTime;
        Vector3 targetPos = GetTarget().transform.position;
        float dist = Mathf.Clamp(Vector3.Distance(targetPos, transform.position), 1, 100);

        rb.AddForce(dt * trackingForce * (targetPos - transform.position));
    }

    protected override void SetVelocity(Vector2 vel)
    {
        rb.velocity = vel;
    }

    DynamicEntity GetTarget()
    {
        return PlayerShip.Instance;
    }
}