using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
 
    public Settings GameLogic;
    public ParticleSystem HyperspaceParticles;
    [HideInInspector] public bool ActiveBossFight;

    private int stage;

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
        }
        
        ActiveBossFight = false;
        stage = 1;
    }

    public void ActivateBossFight() {
        ActiveBossFight = true;
        GameLogic.EnableEnemies = false;
        GameLogic.EnableAsteroids = false;
        GameLogic.EnableMissiles = false;
        ThreatController.Instance.SpawnBoss();
    }

    public void AdvanceStage() {
        stage++;
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
                    GameLogic.EnableMissiles = true;
                    break;
                default:
                    GameLogic.EnableAsteroids = true;
                    GameLogic.EnableEnemies = true;
                    GameLogic.EnableMissiles = true;
                    break;
            }
        }
        StartCoroutine(AdvanceStageCoroutine());
    }

    private IEnumerator AdvanceStageCoroutine() {
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(EffectController.Instance.Hyperspace(HyperspaceParticles));
        //Debug.Log("done2");
        ScoreManager.Instance.NextStage();
        ActiveBossFight = false;
    }
}
