
using UnityEngine;

public class PowerupManager: MonoBehaviour
{
    public Powerup[] powerupPool;
    public static PowerupManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static Powerup SpawnPowerup()
    {
        Powerup powerup = Instance.powerupPool[Random.Range(0, Instance.powerupPool.Length)];
        return Instantiate(powerup);

    }
}