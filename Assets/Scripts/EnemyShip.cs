using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public enum EnemyType
{
    normal, beam, elite, boss
}

public enum ActivationState
{
    idle, pursuit, combat
}
public class EnemyShip : MonoBehaviour, IDynamicEntity, IWeaponContainer, IDamagable
{
    public EnemyType enemyType = EnemyType.normal;
    public ActivationState state;
    public ParticleSystem Particles;
    Collider2D col;
    Rigidbody2D rb;
    protected EnemyShipMovement shipMovement;
    protected IWeapon[] bulletSpawner;
    protected HealthTracker healthTracker;

    protected float currentWeaponCooldown;
    public float weaponCooldown;

    public float bulletSpread = 1f;

    float spawnTime;

    public bool asteroidImmune = false;

    SpriteRenderer spriteRenderer;
    [SerializeField] private Color defaultColor;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        shipMovement = GetComponent<EnemyShipMovement>();
        bulletSpawner = GetComponentsInChildren<IWeapon>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentWeaponCooldown >= 0) currentWeaponCooldown -= Time.deltaTime;
        switch (state)
        {
            case ActivationState.idle:
                if (Time.time - spawnTime > ThreatController.ENEMY_MAX_IDLE_TIME)
                {
                    TriggerDeath();
                }
                break;
            case ActivationState.combat:
                Combat();
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
        Vector2 r = (Vector2)(PlayerShip.GetPosition() - transform.position);
        return (r + Vector2.Perpendicular(r) * Random.Range(-1f,1f) * bulletSpread).normalized;
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

    public virtual void TriggerDeath()
    {
        EffectController.Instance.SpawnParticles(Particles, transform);
        if (Random.value < Settings.Instance.powerupDropChance)
        {
            Powerup pow = PowerupManager.SpawnPowerup();
            pow.Init(transform.position, rb.velocity);
        }
        ThreatController.Instance.DecreaseEnemyCount();
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public Team GetTeam()
    {
        return Team.enemy;
    }

    virtual public void SetHealth(int hp) {
        healthTracker = new HealthTracker(this, hp);
    }

    virtual protected void Combat()
    {
        foreach (IWeapon ps in bulletSpawner)
        {
            if (ps.CanFire())
                ps.Fire();
        }
    }
}
