using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : EnemyShip
{
    public ProjectileSpawner Bullets;
    public ProjectileSpawner Missiles;
    private float bossWeaponCooldown = 5f;
    private bool halfHealth = false;

    private Slider healthbar;

    override protected void Awake() {
        healthbar = ThreatController.Instance.BossHealthbar;
        base.Awake();
        shipMovement.CombatDistance = new Vector3(30f, 0);
    }

    override public void SetHealth(int hp) {
        healthbar.value = 1;
        healthbar.gameObject.SetActive(true);
        healthTracker = new HealthTracker(this, hp);
    }

    protected override void Combat() {
        healthbar.value = (float)healthTracker.health/healthTracker.maxHealth;
        if (currentWeaponCooldown <= 0)
        {
            // call minions
            if(healthTracker.health < healthTracker.maxHealth/2 && !halfHealth) {
                halfHealth = true;

                ThreatController.Instance.SpawnEnemyShip();
                ThreatController.Instance.SpawnEnemyShip();
                ThreatController.Instance.SpawnEnemyShip();
            }

            int weapon = Random.Range(0, 2);
            // bullets
            if(weapon == 0) {
                StartCoroutine(RapidFire(Bullets, 5, 0.1f));
            }
            // missiles
            else if (weapon == 1) {
                StartCoroutine(RapidFire(Missiles, 3, 0.25f));
            }
            currentWeaponCooldown = bossWeaponCooldown;
        }
        
    }

    private IEnumerator RapidFire(ProjectileSpawner spawner, int count, float time) {
        for(int i = 0; i < count; i++) {
            spawner.SpawnProjectile();
            yield return new WaitForSeconds(time);
        }
    }

    public override void TriggerDeath()
    {
        healthbar.gameObject.SetActive(false);
        base.TriggerDeath();
    }
}