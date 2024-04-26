using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour, IWeaponContainer
{

    ProjectileSpawner pSpawner;
    public static MissileSpawner Instance;
    float lastMissileSpawnTime;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        pSpawner = GetComponent<ProjectileSpawner>();
        // transform.SetParent(PlayerShip.Instance.transform, false);
    }

    private void Update()
    {
        if (!Settings.Instance.EnableMissiles) return;
        lastMissileSpawnTime += Time.deltaTime;
        if (lastMissileSpawnTime > ThreatController.Instance.GetMissileCooldown())
        {
            ShootMissile();
            lastMissileSpawnTime = 0;
        }
    }

    public Vector2 GetAimDirection()
    {
        return Vector2.right;
    }

    public Team GetTeam()
    {
        return Team.enemy;
    }

    public Vector2 GetVelocity()
    {
        return PlayerShip.Instance.GetVelocity();
    }

    public Vector2 GetFacingDirection()
    {
        return Vector2.right;
    }

    public void ShootMissile()
    {
        transform.position = (Vector2)PlayerShip.Instance.transform.position + new Vector2(-50f, Random.Range(-20f, 20f));
        pSpawner.SpawnProjectile();
        StartCoroutine(PlayerUI.Instance.MissileWarning());
    }
}
