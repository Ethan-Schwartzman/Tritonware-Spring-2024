using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    [SerializeField] TMP_Text scoreText;

    public int score = 0;
    private int stageModifier = 0;
    private const int STAGE_BONUS = 50;

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
        if(tmpScore > STAGE_BONUS) {
            tmpScore = STAGE_BONUS;
            if(!StageManager.Instance.ActiveBossFight) {
                StageManager.Instance.ActivateBossFight();
            }
        }
        tmpScore += stageModifier;
        if (tmpScore > score)
        {
            score = tmpScore;
            scoreText.text = score.ToString();
        }

    }

    public void NextStage() {
        stageModifier += STAGE_BONUS;
    }
}
