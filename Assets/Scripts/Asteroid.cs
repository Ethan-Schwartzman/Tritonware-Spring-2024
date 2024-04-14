using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const float MAX_DISTANCE = 100;

    public Transform PlayerTransform;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerTransform == null) {
            Debug.LogError("null reference to PlayerTransform");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(PlayerTransform.position, this.transform.position) > MAX_DISTANCE) {
            AsteroidGenerator.Instance.AsteroidPool.Release(this);
        }
    }
}
