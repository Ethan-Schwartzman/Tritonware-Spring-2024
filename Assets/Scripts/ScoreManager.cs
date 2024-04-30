using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    [SerializeField] TMP_Text scoreText;

    public int score = 0;
    private int stageModifier = 0;
    public int combatScore = 0;

    public static float difficultyMultiplier = 1;

    public static ScoreManager Instance;

    void Start() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogWarning("Tried to create more than one instance of ScoreManager");
            Destroy(this);
        }        
    }

    // Update is called once per frame
    void Update() {
        if (!PlayerShip.Instance.isAlive) return;
        int tmpScore = (int)ThreatController.Instance.GetPlayerProgress();
        if(tmpScore > Settings.Instance.sectorDistance) {
            tmpScore = Mathf.RoundToInt(Settings.Instance.sectorDistance);
            if(!StageManager.Instance.ActiveBossFight) {
                StageManager.Instance.ActivateBossFight();
            }
        }
        tmpScore += stageModifier;
        if (tmpScore > score)
        {
            score = tmpScore;
            
        }
        scoreText.text = GetTotalScore().ToString();
    }

    public int GetTotalScore()
    {
        return Mathf.RoundToInt((score + combatScore) * difficultyMultiplier);
    }

    public void NextStage() {
        stageModifier += (int)Settings.Instance.sectorDistance;
    }
}
