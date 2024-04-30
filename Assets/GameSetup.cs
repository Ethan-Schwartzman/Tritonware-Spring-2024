using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    public static GameSetup Instance;
    public static int selectedShip = 1;
    public static int selectedDifficulty = 1;

    public Image s1, s2, d1, d2;
    public Color enableColor, disableColor;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetShip1()
    {
        selectedShip = 1;
        s1.color = enableColor;
        s2.color = disableColor;
    }
    public void SetShip2()
    {
        selectedShip = 2;
        s2.color = enableColor;
        s1.color = disableColor;
    }

    public void SetNormalDifficulty()
    {
        selectedDifficulty = 1;
        d1.color = enableColor;
        d2.color = disableColor;
    }
    public void SetHardDifficulty()
    {
        selectedDifficulty = 2;
        d2.color = enableColor;
        d1.color = disableColor;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
