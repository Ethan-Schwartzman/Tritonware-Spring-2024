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

        if (Input.GetKey(KeyCode.W))
        {
            
            ShipMovement.Instance.Rotate(1);
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            
            ShipMovement.Instance.Rotate(-1);
        }
        
        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            ShipMovement.Instance.Rotate(0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            ShipMovement.Instance.Boost(true);
        }


        if (Input.GetButtonDown("Fire")) Fire();
        if(Input.GetButtonDown("Puzzle1")) Puzzle1();
        if(Input.GetButtonDown("Puzzle1")) Puzzle2();

    }

    void Fire() {

    }

    void Puzzle1() {

    }

    void Puzzle2() {

    }
}
