using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;



public class EnemyShipMovement : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerShipMovement player;


    Vector2 targetRelativePos = new Vector2(20f, 0);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerShipMovement.Instance;
        attachedShip = GetComponent<EnemyShip>();
    }
    public bool inertialDrift = true;
    public bool compensation = true;

    private EnemyShip attachedShip;

    [SerializeField] private float compensationFactor = 5f;
    [SerializeField] private float xComp = 1f;
    [SerializeField] private float yComp = 1f;
    [SerializeField] private float xDamp = 1.0f;
    [SerializeField] private float yDamp = 1.0f;

    [SerializeField] private float strafeSpeed = 1f;
    [SerializeField] private float xStrafeDestRange = 10f;
    [SerializeField] private float yStrafeDestRange = 10f;

    [SerializeField] private float activationRange = 30f;
    [SerializeField] private float combatRange = 5f;
    [SerializeField] private float pursuitSpeed = 20f;

    Vector2 strafeDest = new Vector2(20f, 0);
    

    const float MAX_STRAFE_RESET_ERROR = 3f;
    const float MAX_STRAFE_DEVIATION = 1f;
    const float MIN_STRAFE_PATH_DIST = 10f;

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        switch (attachedShip.state)
        {
            case ActivationState.idle:
                if (PlayerInRange(activationRange))
                {
                    attachedShip.Alert();
                }
                break;
            case ActivationState.pursuit:
                Vector3 rhat = (player.transform.position + (Vector3)targetRelativePos - transform.position).normalized;
                transform.position += (rhat * pursuitSpeed + (Vector3)player.GetVelocity()) * dt;
                if (PlayerInRange(combatRange,true))
                {
                    attachedShip.StartCombat();
                }
                break;
            case ActivationState.combat:
                float m = rb.mass;
                float devStrafeFactor = Mathf.Clamp(1 / GetDeviation().magnitude, 0.2f, 1f);

                rb.AddForce(GetCompensation() * dt * m);
                rb.AddForce(GetDamping() * dt * m);
                //Debug.Log(devStrafeFactor);

                targetRelativePos = Vector2.MoveTowards(targetRelativePos, strafeDest, dt * strafeSpeed * devStrafeFactor);
                //Debug.Log(targetRelativePos);

                if (Vector2.Distance(targetRelativePos, strafeDest) <= MAX_STRAFE_RESET_ERROR)
                {
                    while (Vector2.Distance(strafeDest, targetRelativePos) < MIN_STRAFE_PATH_DIST)
                    {
                        strafeDest = new Vector2(20f, 0f) + new Vector2(UnityEngine.Random.Range(-xStrafeDestRange, xStrafeDestRange),
                                                        UnityEngine.Random.Range(-yStrafeDestRange, yStrafeDestRange));
                    }

                    //Debug.Log(strafeDest);
                }
                break;
        }
    }



    bool PlayerInRange(float dist, bool useTargetRelativePos = false)
    {
        if (useTargetRelativePos) return Vector3.Distance(transform.position, 
            player.transform.position + (Vector3)targetRelativePos) <= dist;
        return Vector3.Distance(transform.position, player.transform.position) <= dist;
    }

    Vector2 GetDeviation()
    {
        return GetRelativePosition() - targetRelativePos;
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
        return compensationFactor * new Vector2(dr.x * xComp, dr.y * yComp) * player.GetVelocity().magnitude / 30f;
    }

    Vector2 GetDamping()
    {
        return -new Vector2(GetRelativeVelocity().x * xDamp, GetRelativeVelocity().y * yDamp);
    }

}
