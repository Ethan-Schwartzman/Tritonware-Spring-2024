using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    [SerializeField] TMP_Text scoreText;

    public int score = 0;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (!PlayerShip.Instance.isAlive) return;
        int tmpScore = (int)ThreatController.Instance.GetPlayerProgress();
        if (tmpScore > score)
        {
            score = tmpScore;
            scoreText.text = score.ToString();
        }

    }
}
