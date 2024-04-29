using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ThreatController : MonoBehaviour
{
    public static int EnemyHealth = 10;
    public static int BeamShipHealth = 6;
    public static int BossHealth = 200;
    public static int EliteHealth = 15;
    public static float AsteroidHealth = 1.5f;
    public Slider BossHealthbar;

    public static ThreatController Instance;
    public int activeEnemyCount;

    // Parameters
    const float MAX_ANGLE = 30f;
    const float MIN_DISTANCE = 40f;
    const float MAX_DISTANCE = 80f;

    const float SPAWN_COOLDOWN_RANGE = 3f;
    [HideInInspector] public float spawnCooldownAverage = 7f;

    const int MAX_GROUP_SIZE = 4;
    const int MAX_ACTIVE_ENEMIES = 10;

    public const float ENEMY_MAX_IDLE_TIME = 10;


    Transform PlayerTransform;
    float lastSpawnTime;
    float spawnCooldown;

    [SerializeField] float eliteGroupSpawnInterval = 15f;
    float eliteGroupSpawnTime = 0;

    float pursuitProgress;
    [HideInInspector] public float pursuitProgressSpeed = 2.2f;
    public float pursuitStartTimer = 10f;
    bool pursuitStarted = false;
    float stageStartTime = 0f;

    public EnemyShip eliteShipTemplate;

    public EnemyShip[] enemyShipPool;
    public EnemyShip[] addedShipPerStage;

    public BossEnemy BossShipTemplate;

    public float missileCooldown = 10f;

    private void Awake()
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
        PlayerTransform = PlayerShip.Instance.transform;
        eliteGroupSpawnTime = eliteGroupSpawnInterval - 0.1f;

    }
    void Start()
    {

        ResetSpawnTimer();
        if(Settings.Instance.SpawnBossAtStart) SpawnBoss();
        stageStartTime = Time.time;
    }

    // Create the ship
    public EnemyShip SpawnEnemyShip()
    {
        activeEnemyCount++;
        int type = Random.Range(0,enemyShipPool.Length);
        EnemyShip newShip = Instantiate(enemyShipPool[type]);
        if (newShip.enemyType == EnemyType.normal)
        {
            newShip.SetHealth(EnemyHealth);
        }
        else if (newShip.enemyType == EnemyType.beam)
        {
            newShip.SetHealth(BeamShipHealth);
        }
        else if (newShip.enemyType == EnemyType.elite)
        {
            newShip.SetHealth(EliteHealth);
        }
        
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
        spawnCooldown = Random.Range(spawnCooldownAverage-SPAWN_COOLDOWN_RANGE, spawnCooldownAverage+SPAWN_COOLDOWN_RANGE);
    }

    // Update is called once per frame
    void Update()
    {
        if (Settings.Instance.EnableEnemies)
        {
            if (Time.time - lastSpawnTime >= spawnCooldown && activeEnemyCount < MAX_ACTIVE_ENEMIES)
            {
                int spawnCount = Random.Range(1, MAX_GROUP_SIZE);
                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnEnemyShip();

                }
                ResetSpawnTimer();
            }
        }

        if (!pursuitStarted && Time.time - stageStartTime > pursuitStartTimer) pursuitStarted = true;
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

    public void AddShipToPool(int stage)
    {
        if (stage < addedShipPerStage.Length)
        {
            if (addedShipPerStage[stage] != null)
            {
                List<EnemyShip> ships = enemyShipPool.ToList();
                ships.Add(addedShipPerStage[stage]);
                enemyShipPool = ships.ToArray();
                
            }
        }
    }

    public void StopPursuit()
    {
        pursuitStarted = false;
        stageStartTime = float.MaxValue;
    }
    public void ResetPursuit()
    {
        pursuitProgress = 0;
        pursuitStarted = false;
        stageStartTime = Time.time;
    }
}

