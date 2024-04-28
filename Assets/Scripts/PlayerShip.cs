using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : DynamicEntity, IDamagable, IWeaponContainer
{
    public static PlayerShip Instance;
    public static PlayerShipMovement shipMovement;
    public static HealthTracker healthTracker;

    ProjectileSpawner[] bulletSpawner;
    SpriteRenderer spriteRenderer;
    TrailRenderer trailRenderer;

    public Powerup currentPowerup;

    float currentWeaponCooldown;
    public float weaponCooldown;

    bool controlLoss = false;
    float controlLossTimer = 0f;
    [SerializeField] float controlLossTime = 3f;

    public bool isAlive = true;

    public Color trailColor, driftTrailColor;

    protected void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (Instance == null)
        {
            Instance = this;
        }
        shipMovement = GetComponent<PlayerShipMovement>();
        healthTracker = new HealthTracker(this, Settings.Instance.PlayerMaxHealth);
        bulletSpawner = GetComponentsInChildren<ProjectileSpawner>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();

       // trailRenderer.startColor = trailColor;
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

        DebugRenderer.lineRenderer1.SetPosition(0, bulletSpawner[0].transform.position);
        DebugRenderer.lineRenderer1.SetPosition(1, bulletSpawner[0].transform.position + 30 * (Vector3)GetAimDirection());
    }


    public void DealDamage(int damage)
    {
        healthTracker.TakeDamage(damage);
        PuzzleManager.Instance.RollForPuzzleDamage(damage);
        StartCoroutine(EffectController.DamageEffect(spriteRenderer));
        PlayerUI.Instance.UpdateUI();
    }

    public void Heal(int hp)
    {
        healthTracker.Heal(hp);
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
        if (!Settings.Instance.EnableDeath) return;
        isAlive = false;
        PlayerShipMovement.Instance.Shutdown();
        PuzzleManager.Instance.gameObject.SetActive(false);
        //trailRenderer.startColor = driftTrailColor;

    }

    public void TriggerCollision()
    {
        ToggleDrift(true);
    }

    public void ToggleDrift(bool toggle)
    {
        if (!isAlive) return;
        PlayerShipMovement.Instance.ToggleDrift(toggle);
        if (toggle)
        {
            controlLoss = true;
            controlLossTimer = controlLossTime;
            //trailRenderer.startColor = driftTrailColor;
        }
        else
        {
            controlLoss = false;
            //trailRenderer.startColor = trailColor;
        }

    }


    public void Shoot()
    {
        if (isAlive && currentWeaponCooldown <= 0)
        {
            currentWeaponCooldown = weaponCooldown;
            foreach (ProjectileSpawner ps in bulletSpawner)
            {
                ps.SpawnProjectile();
            }
            
        }
    }

    public Team GetTeam()
    {
        return Team.player;
    }

    public void SetPowerup(Powerup powerup)
    {
        currentPowerup = powerup;
        PlayerUI.Instance.UpdateUI();
    }

    public void ActivatePowerup()
    {
        if (currentPowerup != null && !currentPowerup.isActive) currentPowerup.Activate();
    }
}
