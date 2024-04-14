using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AsteroidGenerator : MonoBehaviour
{
    public static AsteroidGenerator Instance;

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
        spawnCooldown = Random.Range(0f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSpawnTime >= spawnCooldown) {
            if(AsteroidPool.CountActive <= 100) {
                Asteroid asteroid = AsteroidPool.Get();
                Debug.Log("Spawned Asteroid");
            }
            ResetSpawnTimer();
        }
    }
}
