using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    private void Awake()
    {
        Instance = this;
    }

    //public int PlayerMaxHealth = 100;
    public int AsteroidCollisionDamage = 4;
    public int ShipCollisionDamage = 4;


    public float powerupDropChance = 0.1f;
    public float sectorDistance = 300;

    public bool EnableAsteroids = true;
    public bool EnableEnemies = true;
    public bool EnablePuzzles = true;
    public bool EnableDeath = true;
    public bool EnableMissiles = true;
    public bool SpawnBossAtStart = false;
    public bool OverideStageSettings = false;
}
