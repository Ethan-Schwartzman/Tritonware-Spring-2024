using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Parallax ParallaxManager;
    private Vector3 shipRelativePos;
    private Vector3 lastPos;

    void Start()
    {
        shipRelativePos = transform.position - PlayerShipMovement.Instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerShipMovement.Instance.transform.position + shipRelativePos; 
        ParallaxManager.UpdateParallax(transform.position-lastPos, transform.position);
        lastPos = transform.position;
    }
}
