using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : DynamicEntity, IDamagable, IWeaponContainer
{
    public static PlayerShip Instance;
    public static PlayerShipMovement shipMovement;
    public static HealthTracker healthTracker;

    [SerializeField] private int maxHealth = 20;

    public void DealDamage(int damage)
    {
        healthTracker.TakeDamage(damage);
    }

    public override Vector2 GetVelocity()
    {
        return shipMovement.GetVelocity();
    }
    public override Vector2 GetFacingDirection()
    {
        return shipMovement.GetFacingDirection();
    }


    public Vector2 GetAimDirection()
    {
        return shipMovement.GetFacingDirection();
    }

    public int GetHealth()
    {
        return healthTracker.health;
    }

    public static Vector3 GetPosition()
    {
        return Instance.transform.position;
    }


    public void TriggerDeath()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        shipMovement = GetComponent<PlayerShipMovement>();
        healthTracker = new HealthTracker(this, maxHealth);
    }



}
