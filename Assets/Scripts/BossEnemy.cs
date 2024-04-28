using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyShip
{
    public ProjectileSpawner Bullets;
    public ProjectileSpawner Missiles;
    private float bossWeaponCooldown = 5f;

    override protected void Awake() {
        base.Awake();
        shipMovement.CombatDistance = new Vector3(30f, 0);
    }

    override public HealthTracker SetHealth() {
        return new HealthTracker(this, ThreatController.BossHealth);
    }

    protected override void Combat() {
        if (currentWeaponCooldown <= 0)
        {
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
}