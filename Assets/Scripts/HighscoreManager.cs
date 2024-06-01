using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    private const string COLOR_START_TAG = "<color=\"red\">";
    private const string COLOR_END_TAG = "</color>";

    private List<KeyValuePair<int, string>> leaderboard;
    private List<string> colorText;
    private int leaderboardPos;
    private int charIndex = 0;
    private char[] initials = {'A', 'A', 'A'};

    public TextMeshProUGUI ResetText;
    public GameObject HighscoreObject;
    public TextMeshProUGUI Highscore1Text;
    public TextMeshProUGUI Highscore2Text;
    public TextMeshProUGUI Highscore3Text;

    public static HighscoreManager Instance;

    void Start() {
        //PlayerPrefs.DeleteAll();
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

        int score = ScoreManager.Instance.GetTotalScore();
        leaderboardPos = 3;
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

        colorText = new List<string> {leaderboard[0].Value, leaderboard[1].Value, leaderboard[2].Value};
        if(leaderboardPos != 3) colorText[leaderboardPos] = COLOR_START_TAG + "A" + COLOR_END_TAG + "AA";

        ResetText.text = "Score: " + ScoreManager.Instance.GetTotalScore() + "\nPress <color=\"green\">Green</color> to Restart";
        HighscoreObject.SetActive(true);

        UpdateText(0);
        UpdateText(1);
        UpdateText(2);
    }

    public void SaveHighscore() {
        PlayerPrefs.SetInt("Highscore1", leaderboard[0].Key);
        PlayerPrefs.SetInt("Highscore2", leaderboard[1].Key);
        PlayerPrefs.SetInt("Highscore3", leaderboard[2].Key);

        PlayerPrefs.SetString("Initials1", leaderboard[0].Value);
        PlayerPrefs.SetString("Initials2", leaderboard[1].Value);
        PlayerPrefs.SetString("Initials3", leaderboard[2].Value);
    }

    public void SelectLetter(bool facingRight) {
        if(leaderboardPos != 3) {
            if(facingRight) charIndex++;
            else charIndex--;

            if(charIndex < 0) charIndex = 2;
            else if(charIndex > 2) charIndex = 0;

            SetColorText();

            UpdateText(leaderboardPos);
        }
    }

    public void ModifyLetter(bool increment) {
        if(leaderboardPos != 3) {
            if(increment) initials[charIndex]++;
            else initials[charIndex]--;
        
            if(initials[charIndex] < 'A') initials[charIndex] = 'Z'; 
            else if(initials[charIndex] > 'Z') initials[charIndex] = 'A';

            string newInitials = initials[0].ToString() + initials[1].ToString() + initials[2].ToString();
            leaderboard[leaderboardPos] = new KeyValuePair<int, string>(leaderboard[leaderboardPos].Key, newInitials);
            SetColorText();
            UpdateText(leaderboardPos);
        }
    }

    private void UpdateText(int spot) {
        TextMeshProUGUI scoreText;
        if(spot == 0) scoreText = Highscore1Text;
        else if(spot == 1) scoreText = Highscore2Text;
        else scoreText = Highscore3Text;

        int num = spot+1;
        scoreText.text = num + ".\t" + colorText[spot] + " - " + leaderboard[spot].Key;
    }

    private void SetColorText() {
        if(charIndex == 0) {
            colorText[leaderboardPos] = COLOR_START_TAG + initials[0] + COLOR_END_TAG + initials[1] + initials[2];
        }
        else if(charIndex == 1) {
            colorText[leaderboardPos] = initials[0] + COLOR_START_TAG + initials[1] + COLOR_END_TAG + initials[2];
        }
        else if(charIndex == 2) {
            colorText[leaderboardPos] = initials[0] + (initials[1] + COLOR_START_TAG) + initials[2] + COLOR_END_TAG;
        }
    }
}
