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
            Destroy(this);
            Debug.LogWarning("Tried to create more than one instance of InputManager");
        }        
    }

    void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
    }
}
