using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : EnemyShip
{
    public ProjectileSpawner normalBullets;
    public ProjectileSpawner rapidBullets;
    public ProjectileSpawner Missiles;
    private float bossWeaponCooldown = 5f;
    private bool halfHealth = false;

    private Slider healthbar;
    private EnemyShip[] minions = new EnemyShip[3];

    public static int BossBulletBarrageCount = 10;
    public static int BossMissileBarrageCount = 3;

    override protected void Awake() {
        healthbar = ThreatController.Instance.BossHealthbar;
        base.Awake();
        shipMovement.CombatDistance = new Vector2(35f, 0);
    }

    override public void SetHealth(int hp) {
        healthbar.value = 1;
        healthbar.gameObject.SetActive(true);
        healthTracker = new HealthTracker(this, hp);
    }

    protected override void Combat() {
        if (normalBullets.CanFire()) normalBullets.Fire();
        if (currentWeaponCooldown >= 0) currentWeaponCooldown -= Time.deltaTime;
        healthbar.value = (float)healthTracker.health/healthTracker.maxHealth;
        if (currentWeaponCooldown <= 0)
        {
            // call minions
            if(healthTracker.health < healthTracker.maxHealth/2 && !halfHealth) {
                halfHealth = true;

                minions[0] = ThreatController.Instance.SpawnEnemyShip();
                minions[1] = ThreatController.Instance.SpawnEnemyShip();
                minions[2] = ThreatController.Instance.SpawnEnemyShip();
            }

            int weapon = Random.Range(0, 2);
            // bullets
            if(weapon == 0) {
                StartCoroutine(RapidFire(rapidBullets, BossBulletBarrageCount, 0.07f));
            }
            // missiles
            else if (weapon == 1) {
                StartCoroutine(RapidFire(Missiles, BossMissileBarrageCount, 0.25f));
            }
            currentWeaponCooldown = bossWeaponCooldown;
        }
        
    }

    private IEnumerator RapidFire(ProjectileSpawner spawner, int count, float time) {
        for(int i = 0; i < count; i++) {
            spawner.Fire();
            yield return new WaitForSeconds(time);
        }
    }

    public override void TriggerDeath()
    {
        foreach(EnemyShip minion in minions) {
            if(minion != null) minion.TriggerDeath();
        }
        PlayerShip.Instance.Heal(9999);
        StageManager.Instance.AdvanceStage();
        healthbar.gameObject.SetActive(false);
        PuzzleManager.Instance.ClearPuzzles();
        base.TriggerDeath();
    }

    public override void DealDamage(int damage)
    {
        ScoreManager.Instance.combatScore += damage;
        if (damage > healthTracker.health) ScoreManager.Instance.combatScore -= (damage - healthTracker.health);
        base.DealDamage(damage);
        
    }
}