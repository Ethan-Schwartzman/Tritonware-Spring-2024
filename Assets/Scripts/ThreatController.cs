using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreatController : MonoBehaviour
{
    public static int EnemyHealth = 10;
    public static int BossHealth = 200;
    public static int EliteHealth = 25;
    public static int AsteroidHealth = 6;
    public Slider BossHealthbar;

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

    [SerializeField] float eliteGroupSpawnInterval = 15f;
    float eliteGroupSpawnTime = 0;

    float pursuitProgress;
    public float pursuitProgressSpeed = 0.7f;
    public float pursuitStartTimer = 10f;
    bool pursuitStarted = false;

    public EnemyShip enemyShipTemplate, eliteShipTemplate;
    public BossEnemy BossShipTemplate;

    public float missileCooldown = 10f;

    private void Awake()
    {
        PlayerTransform = PlayerShip.Instance.transform;
        eliteGroupSpawnTime = eliteGroupSpawnInterval - 0.1f;

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
        if(Settings.Instance.SpawnBossAtStart) SpawnBoss();
    }

    // Create the ship
    public EnemyShip SpawnEnemyShip()
    {
        activeEnemyCount++;
        EnemyShip newShip = Instantiate(enemyShipTemplate);
        newShip.SetHealth(EnemyHealth);
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

        return newShip;
    }

    public void SpawnEliteGroup()
    {
        for (int i = 0; i < 3; i++)
        {
            activeEnemyCount++;
            EnemyShip newShip = Instantiate(eliteShipTemplate);
            newShip.SetHealth(EliteHealth);
            Vector2 spawnPosition = new Vector2(-80, 0) + Random.Range(-30f, 30f) * Vector2.up;
            newShip.transform.position = PlayerShip.Instance.transform.position + (Vector3)spawnPosition;
        }
    }

    public void SpawnBoss()
    {
        BossEnemy newBoss = Instantiate(BossShipTemplate);
        newBoss.SetHealth(BossHealth);
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
        newBoss.transform.position = spawnLocation;
        newBoss.Alert();
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
                
            }
            ResetSpawnTimer();
        }

        if (!pursuitStarted && Time.time > pursuitStartTimer) pursuitStarted = true;
        if (pursuitStarted)
        {
            pursuitProgress += pursuitProgressSpeed * Time.deltaTime;
        }

        if (pursuitStarted && GetPlayerProgress() < GetEnemyProgress())
        {
            eliteGroupSpawnTime += Time.deltaTime;
        }
        if (eliteGroupSpawnTime > eliteGroupSpawnInterval)
        {
            eliteGroupSpawnTime = 0;
            PlayerUI.Instance.PopupText("INTERCEPTION IMMINENT");
            SpawnEliteGroup();
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
        return missileCooldown - 5f * Mathf.Clamp01(1-((GetPlayerProgress() - GetEnemyProgress()) / 30)) + Random.Range(-2f,2f);
    }

}

