using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AsteroidGenerator : MonoBehaviour
{
    // Parameters
    const float MAX_ANGLE = 30f;
    const float MIN_SCALE = 0.5f;
    const float MAX_SCALE = 3.0f;
    const float MIN_DISTANCE = 40f;
    const float MAX_DISTANCE = 80f;
    const float MAX_VELOCITY = 10f;
    const float MIN_SPAWN_COOLDOWN = 0.0f;
    const float MAX_SPAWN_COOLDOWN = 0.5f;
    const float MIN_MASS = 5f;
    const float MAX_MASS = 40f;
    const float MAX_SPIN = 200f;

    const int POOL_MAX = 200;
    const int POOL_DEFAUlT = 100;
    const int MAX_ACTIVE = 100;

    public static AsteroidGenerator Instance;

    public Transform PlayerTransform;
    public Sprite[] AsteroidSprites;
    public Asteroid AsteroidPrefab;
    public ObjectPool<Asteroid> AsteroidPool;

    private float spawnCooldown;
    private float lastSpawnTime;

    void Start() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogWarning("Tried to create more than one instance of AstroidGenerator");
            Destroy(this);
        }        

        AsteroidPool = new ObjectPool<Asteroid>
        (
            CreateAsteroid,
            OnTakeFromPool, 
            OnReleaseAsteroid, 
            OnDestroyAsteroid, 
            true, POOL_DEFAUlT, POOL_MAX
        );

        ResetSpawnTimer();
    }

    // Create the asteroid
    private Asteroid CreateAsteroid() {
        Asteroid asteroid = Instantiate(AsteroidPrefab, Vector3.zero, Quaternion.identity);
        asteroid.transform.SetParent(this.transform);
        asteroid.gameObject.SetActive(false);
        return asteroid;
    }

    // Spawn the asteroid
    private void OnTakeFromPool(Asteroid asteroid) {
        // Set sprite
        if(AsteroidSprites.Length != 0) {
            asteroid.SetSprite(AsteroidSprites[Random.Range(0, AsteroidSprites.Length)]);
        }

        // Set asteroid scale
        asteroid.transform.localScale = new Vector3(
            Random.Range(MIN_SCALE, MAX_SCALE), 
            Random.Range(MIN_SCALE, MAX_SCALE), 
            1f
        );

        // Spawn asteroid in the general direction player is facing
        float rotationAmount = Random.Range(-MAX_ANGLE, MAX_ANGLE);
        Vector3 spawnDirection = Quaternion.AngleAxis(rotationAmount, Vector3.forward) * PlayerTransform.up;
        float angleRad = Mathf.Deg2Rad * Vector3.SignedAngle(Vector3.right, spawnDirection, Vector3.forward);

        // Set asteroid position
        float spawnDistance = Random.Range(MIN_DISTANCE, MAX_DISTANCE);
        Vector3 spawnLocation = new Vector3(
            PlayerTransform.position.x + (spawnDistance * Mathf.Cos(angleRad)),
            PlayerTransform.position.y + (spawnDistance * Mathf.Sin(angleRad)),
            PlayerTransform.position.z
        );
        asteroid.transform.position = spawnLocation;

        // Set active before setting rigidbody properties
        asteroid.gameObject.SetActive(true);

        // Set asteroid rigidbody properties
        asteroid.SetVelocity(new Vector2(
            Random.Range(-MAX_VELOCITY, MAX_VELOCITY),
            Random.Range(-MAX_VELOCITY, MAX_VELOCITY)
        ));

        asteroid.SetMass(Random.Range(MIN_MASS, MAX_MASS));

        asteroid.SetSpin(Random.Range(-MAX_SPIN, MAX_SPIN));
    }

    // Return the asteroid to the pool
    private void OnReleaseAsteroid(Asteroid asteroid) {
        asteroid.gameObject.SetActive(false);
    }

    // Destroy asteroid
    private void OnDestroyAsteroid(Asteroid asteroid) {
        Destroy(asteroid);
    }

    private void ResetSpawnTimer() {
        lastSpawnTime = Time.time;
        spawnCooldown = Random.Range(MIN_SPAWN_COOLDOWN, MAX_SPAWN_COOLDOWN);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSpawnTime >= spawnCooldown) {
            if(AsteroidPool.CountActive <= MAX_ACTIVE) {
                Asteroid asteroid = AsteroidPool.Get();
            }
            ResetSpawnTimer();
        }
    }
}
