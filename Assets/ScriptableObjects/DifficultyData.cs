using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "ScriptableObjects/DifficultyData")]
public class DifficultyData : ScriptableObject
{
    public float[] enemySpawnCooldown;
    public float[] pursuitRates;
    public int[] bossHealths;
    public float[] AsteroidSpeedMultiplier;
    public float[] AsteroidHealths;
    public float[] PuzzleChances;
    public int[] BossBulletBarrageCount;
    public int[] BossMissileBarrageCount;
    public bool[] StageMissiles;

    public int EnemyHealth = 10;
    public int BeamEnemyHealth = 6;
}
