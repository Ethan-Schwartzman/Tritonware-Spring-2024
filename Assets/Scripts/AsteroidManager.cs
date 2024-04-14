using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AsteroidGenerator : MonoBehaviour
{
    public static AsteroidGenerator Instance;

    public Transform PlayerTransform;
    public Texture2D[] AsteroidTextures;
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
            false, 100, 1000
        );

        ResetSpawnTimer();
    }

    // Create the asteroid
    private Asteroid CreateAsteroid() {
        Asteroid asteroid = Instantiate(AsteroidPrefab, Vector3.zero, Quaternion.identity);
        asteroid.gameObject.SetActive(false);
        return asteroid;
    }

    // Spawn the asteroid
    private void OnTakeFromPool(Asteroid asteroid) {
        // Set asteroid scale
        asteroid.transform.localScale = new Vector3(
            Random.Range(0.8f, 3.0f), 
            Random.Range(0.8f, 3.0f), 
            1f
        );

        // Spawn asteroid in the general direction player is facing
        float maxAngle = 30f;
        float rotationAmount = Random.Range(-maxAngle, maxAngle);
        Vector3 spawnDirection = Quaternion.AngleAxis(rotationAmount, Vector3.forward) * PlayerTransform.up;
        float angleRad = Mathf.Deg2Rad * Vector3.SignedAngle(Vector3.right, spawnDirection, Vector3.forward);

        // Set asteroid position
        float spawnDistance = Random.Range(20f, 80f);
        Vector3 spawnLocation = new Vector3(
            PlayerTransform.position.x + (spawnDistance * Mathf.Cos(angleRad)),
            PlayerTransform.position.y + (spawnDistance * Mathf.Sin(angleRad)),
            PlayerTransform.position.z
        );
        asteroid.transform.position = spawnLocation;

        asteroid.gameObject.SetActive(true);
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
        spawnCooldown = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSpawnTime >= spawnCooldown) {
            if(AsteroidPool.CountActive <= 100) {
                Asteroid asteroid = AsteroidPool.Get();
            }
            ResetSpawnTimer();
        }
    }
}
