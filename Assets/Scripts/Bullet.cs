using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : Projectile
{
    Vector2 velocity;

    protected override void SetVelocity(Vector2 vel)
    {
        velocity = vel;
    }

    protected override void FixedUpdate()
    {
        transform.position += (Vector3)velocity * Time.fixedDeltaTime;
    }
}