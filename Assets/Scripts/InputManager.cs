using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private bool canReset;
    //private float flickThreshold = 0.6f;
    private float prevVertical;
    private float prevHorizontal;
    float horizontal;
    float vertical;
    //float horizontalSpeed;
    //float verticalSpeed;

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
        prevHorizontal = horizontal;
        prevVertical = vertical;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        //horizontalSpeed = horizontal - prevHorizontal;
        //verticalSpeed = vertical - prevVertical;

        if(Mathf.Abs(horizontal) > 0.01f) {
            PlayerShipMovement.Instance.Rotate(-horizontal);
        }
        else {
            PlayerShipMovement.Instance.Rotate(0);            
        }

        if(canReset) {                
            if(vertical == 1 && prevVertical == 0)
            {
                HighscoreManager.Instance.ModifyLetter(true);
            }
            else if(vertical == -1 && prevVertical == 0) {
                HighscoreManager.Instance.ModifyLetter(false);
            }

            if(horizontal == 1 && prevHorizontal == 0) {
                HighscoreManager.Instance.SelectLetter(true);
            }
            else if(horizontal == -1 && prevHorizontal == 0) {
                HighscoreManager.Instance.SelectLetter(false);                    
            }
        }

        // pc input code
        /* if (Input.GetButton("Left")) {
            PlayerShipMovement.Instance.Rotate(1);
        }
        if(Input.GetButtonDown("Left")) {
            if(canReset) HighscoreManager.Instance.SelectLetter(false);
        }
        if (Input.GetButton("Right")) {
            PlayerShipMovement.Instance.Rotate(-1);
        }
        if(Input.GetButtonDown("Right")) {
            if(canReset) HighscoreManager.Instance.SelectLetter(true);
        }
        if(Input.GetButtonDown("Up")) {
            if(canReset) HighscoreManager.Instance.ModifyLetter(true);
        }
        if(Input.GetButtonDown("Down")) {
            if(canReset) HighscoreManager.Instance.ModifyLetter(false);
        }
        if (!Input.GetButton("Left") && !Input.GetButton("Right")) PlayerShipMovement.Instance.Rotate(0);
        */

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
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetButton("Exit 1") && Input.GetButton("Exit 2"))) {
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
