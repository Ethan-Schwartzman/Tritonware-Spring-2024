using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    Rigidbody2D rb;
    public float trackingForce = 100;
    public float missileSpeed = 60;
    public float maxTime = 4f;

    public float activeTime = 0;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        activeTime = 0;
    }

    private void OnEnable()
    {
        activeTime = 0;
    }

    

    protected override void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 targetPos = GetTarget().transform.position;
        float dist = Mathf.Clamp(Vector3.Distance(targetPos, transform.position), 1, 100);

        Vector3 r = targetPos - transform.position;
        Vector3 vRel = rb.velocity - GetTarget().GetVelocity();
        float deviationAngle = Vector2.SignedAngle(r, vRel);

        rb.velocity += -Vector2.Perpendicular(vRel) * trackingForce * Mathf.Deg2Rad * Mathf.Sin(Mathf.Deg2Rad * deviationAngle);
        //if (rb.velocity.magnitude < missileSpeed) rb.velocity += rb.velocity.normalized * 10 * dt;

        //Debug.Log($"Angle: {deviationAngle}");

        /*
        transform.position += (rhat * pursuitSpeed + (Vector3)player.GetVelocity()) * dt;

        rb.AddForce(dt * trackingForce * (targetPos - transform.position));
        */
    }

    protected override void Update()
    {
        base.Update();
        activeTime += Time.deltaTime;
        if (activeTime > maxTime) Despawn();
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