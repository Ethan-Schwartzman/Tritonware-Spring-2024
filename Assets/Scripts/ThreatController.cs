using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatController : MonoBehaviour
{
    public static int EnemyHealth = 10;
    public static int AsteroidHealth = 10;

    public static ThreatController Instance;
    public int activeEnemyCount;

    // Parameters
    const float MAX_ANGLE = 30f;
    const float MIN_DISTANCE = 40f;
    const float MAX_DISTANCE = 80f;

    const float MIN_SPAWN_COOLDOWN = 5f;
    const float MAX_SPAWN_COOLDOWN = 10f;

    const int MAX_GROUP_SIZE = 4;
    const int MAX_ACTIVE_ENEMIES = 10;

    public const float ENEMY_MAX_IDLE_TIME = 10;


    Transform PlayerTransform;
    float lastSpawnTime;
    float spawnCooldown;

    float pursuitProgress;
    public float pursuitProgressSpeed = 0.7f;
    public float pursuitStartTimer = 10f;
    bool pursuitStarted = false;

    public EnemyShip enemyShipTemplate;

    public float missileCooldown = 10f;

    private void Awake()
    {
        PlayerTransform = PlayerShip.Instance.transform;
    }
    void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Tried to create more than one instance of ThreatController");
            Destroy(this);
        }

        ResetSpawnTimer();
    }

    // Create the ship
    private void SpawnEnemyShip()
    {
        EnemyShip newShip = Instantiate(enemyShipTemplate);
        // Spawn ship in the general direction player is facing
        float rotationAmount = Random.Range(-MAX_ANGLE, MAX_ANGLE);
        Vector3 spawnDirection = Quaternion.AngleAxis(rotationAmount, Vector3.forward) * PlayerTransform.up;
        float angleRad = Mathf.Deg2Rad * Vector3.SignedAngle(Vector3.right, spawnDirection, Vector3.forward);

        // Set ship position
        float spawnDistance = Random.Range(MIN_DISTANCE, MAX_DISTANCE);
        Vector3 spawnLocation = new Vector3(
            PlayerTransform.position.x + (spawnDistance * Mathf.Cos(angleRad)),
            PlayerTransform.position.y + (spawnDistance * Mathf.Sin(angleRad)),
            PlayerTransform.position.z
        );
        newShip.transform.position = spawnLocation;
    }


    private void ResetSpawnTimer()
    {
        lastSpawnTime = Time.time;
        spawnCooldown = Random.Range(MIN_SPAWN_COOLDOWN, MAX_SPAWN_COOLDOWN);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Settings.Instance.EnableEnemies) return;
        if (Time.time - lastSpawnTime >= spawnCooldown && activeEnemyCount < MAX_ACTIVE_ENEMIES)
        {
            int spawnCount = Random.Range(1, MAX_GROUP_SIZE);
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemyShip();
                activeEnemyCount++;
            }
            ResetSpawnTimer();
        }

        if (!pursuitStarted && Time.time > pursuitStartTimer) pursuitStarted = true;
        if (pursuitStarted)
        {
            pursuitProgress += pursuitProgressSpeed * Time.deltaTime;
        }
        
    }

    public void DecreaseEnemyCount()
    {
        activeEnemyCount--;
    }

    public float GetPlayerProgress()
    {
        return PlayerShip.Instance.transform.position.x / 10;
    }

    public float GetEnemyProgress()
    {
        return pursuitProgress;
    }

    public float GetMissileCooldown()
    {
        return missileCooldown;
    }

}

