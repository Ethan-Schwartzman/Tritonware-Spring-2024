using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private bool canReset;


    void Start() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogWarning("Tried to create more than one instance of InputManager");
            Destroy(this);
        }        

        canReset = false;
    }

    void Update() {
        // Depreciated
        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");

        if (Input.GetButton("Left")) PlayerShipMovement.Instance.Rotate(1);
        if (Input.GetButton("Right")) PlayerShipMovement.Instance.Rotate(-1);
        if (!Input.GetButton("Left") && !Input.GetButton("Right")) PlayerShipMovement.Instance.Rotate(0);

        if(Input.GetButtonDown("Puzzle1")) {
            Puzzle1();
        }
        if(Input.GetButtonDown("Puzzle2")) {
            Puzzle2();
        }
        if(Input.GetButton("Puzzle1") || Input.GetButton("Puzzle2")) {
            Fire();
        }
        if(Input.GetButtonDown("Powerup")) {
            PlayerShip.Instance.ActivatePowerup();
        }
        if (Input.GetButtonDown("Menu") && canReset) {
            HighscoreManager.Instance.SaveHighscore();

            // reload scene
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("GameSetupScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(canReset) HighscoreManager.Instance.SaveHighscore();
            Application.Quit();
        }

        // Debugging
        // if (Input.GetKeyDown(KeyCode.G)) PlayerShip.Instance.ToggleDrift(true);
        // if (Input.GetKeyDown(KeyCode.H)) PlayerShip.Instance.ToggleDrift(false);
        //if (Input.GetKeyDown(KeyCode.F)) PuzzleManager.Instance.SpawnPuzzle(true);

    }

    void Fire() {
        PlayerShip.Instance.Shoot();
    }

    void Puzzle1() {
        PuzzleManager.Instance.TriggerPuzzle1();
    }

    void Puzzle2() {
        PuzzleManager.Instance.TriggerPuzzle2();
    }

    public void EnableReset() {
        canReset = true;
    }
}
