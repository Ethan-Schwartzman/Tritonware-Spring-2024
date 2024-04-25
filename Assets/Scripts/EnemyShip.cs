using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;


public enum ActivationState
{
    idle, pursuit, combat
}
public class EnemyShip : MonoBehaviour, IDynamicEntity, IWeaponContainer, IDamagable
{

    public ActivationState state;
    Collider2D col;
    Rigidbody2D rb;
    EnemyShipMovement shipMovement;
    ProjectileSpawner bulletSpawner;
    HealthTracker healthTracker;

    float currentWeaponCooldown;
    public float weaponCooldown;

    public float bulletSpread = 1f;

    float spawnTime;

    SpriteRenderer spriteRenderer;
    [SerializeField] private Color defaultColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        shipMovement = GetComponent<EnemyShipMovement>();
        bulletSpawner = GetComponent<ProjectileSpawner>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        healthTracker = new HealthTracker(this, ThreatController.EnemyHealth);
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeaponCooldown >= 0) currentWeaponCooldown -= Time.deltaTime;
        switch (state)
        {
            case ActivationState.idle:
                if (Time.time - spawnTime > ThreatController.ENEMY_MAX_IDLE_TIME)
                {
                    TriggerDeath();
                }
                break;
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
        return ((Vector2)(PlayerShip.GetPosition() - transform.position) + Random.insideUnitCircle * bulletSpread).normalized;
    }

    public int GetHealth()
    {
        return healthTracker.health;
    }

    public void DealDamage(int damage)
    {
        healthTracker.TakeDamage(damage);
        StartCoroutine(EffectController.DamageEffect(spriteRenderer));
    }

    public void TriggerDeath()
    {
        ThreatController.Instance.DecreaseEnemyCount();
        StopAllCoroutines();
        Destroy(gameObject);
    }


    public Team GetTeam()
    {
        return Team.enemy;
    }
}
