using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour, IWeaponContainer
{

    ProjectileSpawner pSpawner;
    public static MissileSpawner Instance;
    float lastMissileSpawnTime;
    float nextMissileSpawnTime;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        pSpawner = GetComponent<ProjectileSpawner>();

        // transform.SetParent(PlayerShip.Instance.transform, false);
        nextMissileSpawnTime = 20;

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!Settings.Instance.EnableMissiles) return;
        lastMissileSpawnTime += Time.deltaTime;
        if (lastMissileSpawnTime > nextMissileSpawnTime)
        {
            ShootMissile();
            lastMissileSpawnTime = 0;
            nextMissileSpawnTime = ThreatController.Instance.GetMissileCooldown();
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
        float height = Random.Range(-30f, 30f);
        transform.position = (Vector2)PlayerShip.Instance.transform.position + new Vector2(-80f, height);
        pSpawner.Fire();
        StartCoroutine(PlayerUI.Instance.MissileWarning(height));
    }
}
