using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActivationState
{
    idle, pursuit, combat
}
public class EnemyShip : MonoBehaviour, IDynamicEntity, IWeaponContainer
{

    public ActivationState state;
    Collider2D col;
    Rigidbody2D rb;
    EnemyShipMovement shipMovement;
    BulletSpawner bulletSpawner;

    float currentWeaponCooldown;
    public float weaponCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        shipMovement = GetComponent<EnemyShipMovement>();
        bulletSpawner = GetComponent<BulletSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeaponCooldown >= 0) currentWeaponCooldown -= Time.deltaTime;
        switch (state)
        {
            case ActivationState.combat:
                if (currentWeaponCooldown <= 0)
                {
                    bulletSpawner.SpawnBullet();
                    currentWeaponCooldown = weaponCooldown;
                }
                break;
        }
    }

    public void Alert()
    {
        state = ActivationState.pursuit;
    }

    public void StartCombat()
    {
        state = ActivationState.combat;
        col.enabled = true;
    }

    public Vector2 GetVelocity()
    {
        if (state == ActivationState.pursuit) throw new System.NotImplementedException();
        return rb.velocity;
    }

    public Vector2 GetFacingDirection()
    {
        throw new System.NotImplementedException();
    }

    public Vector2 GetAimDirection()
    {
        return (PlayerShip.GetPosition() - transform.position).normalized;
    }

    
}
