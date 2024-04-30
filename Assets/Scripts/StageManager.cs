using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
 
    public Settings GameLogic;
    public ParticleSystem HyperspaceParticles;
    public FMODUnity.StudioEventEmitter HyperspaceSound; 
    [HideInInspector] public bool ActiveBossFight;

    private int stage;

    public DifficultyData difficultyData;
    /*
    public float[] enemySpawnCooldown;
    public float[] pursuitRates;
    public int[] bossHealths;
    public float[] AsteroidSpeedMultiplier;
    public float[] AsteroidHealths;
    public float[] PuzzleChances;
    public int[] BossBulletBarrageCount;
    public int[] BossMissileBarrageCount;
    public bool[] StageMissiles;
    */

    void Start() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogWarning("Tried to create more than one instance of StageManager");
            Destroy(this);
        }

        if(!GameLogic.OverideStageSettings) {
            GameLogic.EnablePuzzles = true;
            GameLogic.EnableDeath = true;
            GameLogic.EnableAsteroids = true;
            GameLogic.EnableEnemies = true;
            SetStageDifficulty(1);
        }

        ThreatController.EnemyHealth = difficultyData.EnemyHealth;
        ThreatController.BeamShipHealth = difficultyData.BeamEnemyHealth;
        ActiveBossFight = false;
        stage = 1;
    }

    public void ActivateBossFight() {
        ActiveBossFight = true;
        GameLogic.EnableEnemies = false;
        GameLogic.EnableAsteroids = false;
        //GameLogic.EnableMissiles = false;
        ThreatController.Instance.SpawnBoss();
        ThreatController.Instance.StopPursuit();
    }

    public void DeactivateBossFight() {
        ActiveBossFight = false;
    }

    public void AdvanceStage() {
        stage++;
        StartCoroutine(AdvanceStageCoroutine());
    }

    public int GetStage() {
        return stage;
    }

    private IEnumerator AdvanceStageCoroutine() {
        PlayerUI.Instance.PopupText("PREPARE FOR HYPERSPACE");
        yield return new WaitForSeconds(0.5f);
        HyperspaceSound.Play();
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(EffectController.Instance.Hyperspace(HyperspaceParticles));
        if(!GameLogic.OverideStageSettings) {
            switch(stage) {
                case 1:
                    GameLogic.EnableAsteroids = true;
                    break;
                case 2:
                    GameLogic.EnableAsteroids = true;
                    GameLogic.EnableEnemies = true;
                    break;
                case 3:
                    GameLogic.EnableAsteroids = true;
                    GameLogic.EnableEnemies = true;

                    break;
                default:
                    GameLogic.EnableAsteroids = true;
                    GameLogic.EnableEnemies = true;

                    break;
            }
            SetStageDifficulty(stage);
            ThreatController.Instance.AddShipToPool(stage);
        }
        ScoreManager.Instance.NextStage();
        DeactivateBossFight();
        

        /*
        Debug.Log("New stats: ");
        Debug.Log("ennemy spawn cd: " + ThreatController.Instance.spawnCooldownAverage);
        Debug.Log("pursuit speed: " + ThreatController.Instance.pursuitProgressSpeed);
        Debug.Log("boss health: " + ThreatController.BossHealth);
        */
    }

    private float StageStat(float[] arr, int stage)
    {
        stage -= 1;
        if (stage < arr.Length) return (arr[stage]);
        return arr[arr.Length - 1];
    }
    private int StageStat(int[] arr, int stage)
    {
        stage -= 1;
        if (stage < arr.Length) return (arr[stage]);
        return arr[arr.Length - 1];
    }

    private bool StageStat(bool[] arr, int stage)
    {
        stage -= 1;
        if (stage < arr.Length) return (arr[stage]);
        return arr[arr.Length - 1];
    }

    // 1-indexed
    public void SetStageDifficulty(int stage)
    {
        ThreatController.Instance.spawnCooldownAverage = StageStat(difficultyData.enemySpawnCooldown, stage);
        ThreatController.Instance.pursuitProgressSpeed = StageStat(difficultyData.pursuitRates, stage);
        ThreatController.BossHealth = StageStat(difficultyData.bossHealths, stage);
        PuzzleManager.Instance.puzzleChancePerDamage = StageStat(difficultyData.PuzzleChances, stage);
        AsteroidGenerator.Instance.stageVelocity = StageStat(difficultyData.AsteroidSpeedMultiplier, stage);
        ThreatController.AsteroidHealth = StageStat(difficultyData.AsteroidHealths, stage);
        BossEnemy.BossBulletBarrageCount = StageStat(difficultyData.BossBulletBarrageCount, stage);
        BossEnemy.BossMissileBarrageCount = StageStat(difficultyData.BossMissileBarrageCount,stage);
        GameLogic.EnableMissiles = StageStat(difficultyData.StageMissiles, stage);
    }
}
