using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : DynamicEntity, IDamagable, IWeaponContainer
{
    public static PlayerShip Instance;
    public static PlayerShipMovement shipMovement;
    public static HealthTracker healthTracker;

    BulletSpawner bulletSpawner;

    [SerializeField] private int maxHealth = 20;

    float currentWeaponCooldown;
    public float weaponCooldown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        shipMovement = GetComponent<PlayerShipMovement>();
        healthTracker = new HealthTracker(this, maxHealth);
        bulletSpawner = GetComponent<BulletSpawner>();
    }

    private void Update()
    {
        if (currentWeaponCooldown >= 0) currentWeaponCooldown -= Time.deltaTime;
    }


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
        return;
        throw new System.NotImplementedException();
    }


    public void Shoot()
    {
        if (currentWeaponCooldown <= 0)
        {
            currentWeaponCooldown = weaponCooldown;
            bulletSpawner.SpawnBullet();
        }
    }

    public Team GetTeam()
    {
        return Team.player;
    }
}
