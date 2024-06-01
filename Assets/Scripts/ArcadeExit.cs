using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeExit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetButton("Exit 1") && Input.GetButton("Exit 2"))) {
            Application.Quit();
        }
    }
}