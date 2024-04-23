using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : DynamicEntity, IDamagable, IWeaponContainer
{
    public static PlayerShip Instance;
    public static PlayerShipMovement shipMovement;
    public static HealthTracker healthTracker;

    BulletSpawner bulletSpawner;
    SpriteRenderer spriteRenderer;
    TrailRenderer trailRenderer;


    float currentWeaponCooldown;
    public float weaponCooldown;

    bool controlLoss = false;
    float controlLossTimer = 0f;
    [SerializeField] float controlLossTime = 3f;

    public bool isAlive = true;

    public Color trailColor, driftTrailColor;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (Instance == null)
        {
            Instance = this;
        }
        shipMovement = GetComponent<PlayerShipMovement>();
        healthTracker = new HealthTracker(this, Settings.PlayerMaxHealth);
        bulletSpawner = GetComponent<BulletSpawner>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        trailRenderer.startColor = trailColor;
    }

    private void Start()
    {
        PlayerUI.Instance.UpdateUI();
    }

    private void Update()
    {
        if (currentWeaponCooldown >= 0) currentWeaponCooldown -= Time.deltaTime;
        if (controlLoss) controlLossTimer -= Time.deltaTime;
        if (controlLossTimer <= 0) ToggleDrift(false);
    }


    public void DealDamage(int damage)
    {
        healthTracker.TakeDamage(damage);
        PuzzleManager.Instance.RollForPuzzle(damage);
        StartCoroutine(EffectController.DamageEffect(spriteRenderer));
        PlayerUI.Instance.UpdateUI();
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

    public int GetMaxHealth()
    {
        return healthTracker.maxHealth;
    }

    public static Vector3 GetPosition()
    {
        return Instance.transform.position;
    }


    public void TriggerDeath()
    {
        return;
        isAlive = false;
        throw new System.NotImplementedException();
        
    }

    public void TriggerCollision()
    {
        ToggleDrift(true);
    }

    public void ToggleDrift(bool toggle)
    {

        PlayerShipMovement.Instance.ToggleDrift(toggle);
        if (toggle)
        {
            controlLoss = true;
            controlLossTimer = controlLossTime;
            trailRenderer.startColor = driftTrailColor;
        }
        else
        {
            controlLoss = false;
            trailRenderer.startColor = trailColor;
        }

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
