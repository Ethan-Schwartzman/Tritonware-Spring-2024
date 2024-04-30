using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    private List<KeyValuePair<int, string>> leaderboard;

    public TextMeshProUGUI ResetText;
    public GameObject HighscoreObject;
    public TextMeshProUGUI Highscore1Text;
    public TextMeshProUGUI Highscore2Text;
    public TextMeshProUGUI Highscore3Text;

    public static HighscoreManager Instance;

    void Start() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogWarning("Tried to create more than one instance of HighscoreManager");
            Destroy(this);
        }        
    }

    public void GameOver() {
        int highscore1 = PlayerPrefs.GetInt("Highscore1", 0);
        int highscore2 = PlayerPrefs.GetInt("Highscore2", 0);
        int highscore3 = PlayerPrefs.GetInt("Highscore3", 0);

        string initials1 = PlayerPrefs.GetString("Initials1", "AAA");
        string initials2 = PlayerPrefs.GetString("Initials2", "AAA");
        string initials3 = PlayerPrefs.GetString("Initials3", "AAA");

        KeyValuePair<int, string> leaderboard1 = new KeyValuePair<int, string>(highscore1, initials1);
        KeyValuePair<int, string> leaderboard2 = new KeyValuePair<int, string>(highscore2, initials2);
        KeyValuePair<int, string> leaderboard3 = new KeyValuePair<int, string>(highscore3, initials3);

        leaderboard = new List<KeyValuePair<int, string>> {leaderboard1, leaderboard2, leaderboard3};

        int score = ScoreManager.Instance.score;
        int leaderboardPos = 3;
        if(score > highscore1) {
            leaderboardPos = 0;
        }
        else if(score > highscore2) {
            leaderboardPos = 1;
        }
        else if(score > highscore3) {
            leaderboardPos = 2;
        }

        leaderboard.Insert(leaderboardPos, new KeyValuePair<int, string>(score, "AAA"));

        // Blink

        //A<color="red">A</color>A

        ResetText.text = "Score: " + ScoreManager.Instance.score + "\nPress Enter to Restart";
        HighscoreObject.SetActive(true);

        UpdateText(0);
        UpdateText(1);
        UpdateText(2);
    }

    public void SaveHighscore() {
        PlayerPrefs.SetInt("Highscore1", leaderboard[0].Key);
        PlayerPrefs.SetInt("Highscore2", leaderboard[1].Key);
        PlayerPrefs.SetInt("Highscore3", leaderboard[2].Key);

        PlayerPrefs.SetString("Highscore1", leaderboard[0].Value);
        PlayerPrefs.SetString("Highscore2", leaderboard[1].Value);
        PlayerPrefs.SetString("Highscore3", leaderboard[2].Value);
    }

    private void UpdateText(int spot) {
        TextMeshProUGUI scoreText;
        if(spot == 0) scoreText = Highscore1Text;
        else if(spot == 1) scoreText = Highscore2Text;
        else scoreText = Highscore3Text;

        scoreText.text = (spot+1) + ".\t" + leaderboard[spot].Value + " - " + leaderboard[spot].Key;
    }
}
