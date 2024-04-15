using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 shipRelativePos;

    void Start()
    {
        shipRelativePos = transform.position - ShipMovement.Instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ShipMovement.Instance.transform.position + shipRelativePos; 
        
    }
}
