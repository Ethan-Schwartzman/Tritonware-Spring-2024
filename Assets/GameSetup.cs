using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public static GameSetup Instance;
    public static int selectedShip = 1;
    public static int selectedDifficulty = 1;

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
    }
    public void SetShip2()
    {
        selectedShip = 2;
    }

    public void SetNormalDifficulty()
    {
        selectedDifficulty = 1;
    }
    public void SetHardDifficulty()
    {
        selectedDifficulty = 2;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
