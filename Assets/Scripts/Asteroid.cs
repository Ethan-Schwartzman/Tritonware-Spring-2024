using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const float MAX_DISTANCE = 100;

    public Transform PlayerTransform;
    private Rigidbody2D rb;

    public void SetVelocity(Vector2 v) {
        rb.velocity = Vector2.zero;
        rb.AddForce(v * 40, ForceMode2D.Impulse);
    }

    void Awake()
    {
        if(PlayerTransform == null) {
            Debug.LogError("null reference to PlayerTransform");
        }

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(PlayerTransform.position, this.transform.position) > MAX_DISTANCE) {
            AsteroidGenerator.Instance.AsteroidPool.Release(this);
        }
    }
}
