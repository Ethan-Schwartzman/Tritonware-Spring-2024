using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;


    void Start() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogWarning("Tried to create more than one instance of InputManager");
            Destroy(this);
        }        
    }

    void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        // need to change to not hard coded inputs

        if (Input.GetKey(KeyCode.A))
        {
            
            PlayerShipMovement.Instance.Rotate(1);
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            
            PlayerShipMovement.Instance.Rotate(-1);
        }
        
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            PlayerShipMovement.Instance.Rotate(0);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayerShip.Instance.ActivatePowerup();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerShip.Instance.ToggleDrift(true);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerShip.Instance.ToggleDrift(false);
        }


        if (Input.GetButton("Fire")) Fire();
        if(Input.GetButtonDown("Puzzle1")) Puzzle1();
        if(Input.GetButtonDown("Puzzle2")) Puzzle2();

        if (Input.GetKeyDown(KeyCode.F)) PuzzleManager.Instance.SpawnPuzzle(true);

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
}
