using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : DynamicEntity, IDamagable
{
    private const float MAX_DISTANCE = 100;

    public Transform PlayerTransform;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public HealthTracker healthTracker;

    public override Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    public override Vector2 GetFacingDirection()
    {
        return transform.up;
    }


    public void SetVelocity(Vector2 v) {
        rb.velocity = Vector2.zero;
        rb.AddForce(v * 40, ForceMode2D.Impulse);
    }

    public void SetMass(float m) {
        rb.mass = m;
    }

    public void SetSpin(float t) {
        rb.AddTorque(t, ForceMode2D.Impulse);
    }

    public void SetSprite(Sprite s) {
        sr.sprite = s;
    }

    public void DealDamage(int damage)
    {
        healthTracker.TakeDamage(damage);
    }

    public void TriggerDeath()
    {
        AsteroidGenerator.Instance.AsteroidPool.Release(this);
    }



    void Awake()
    {
        PlayerTransform = PlayerShip.Instance.transform;
        if(PlayerTransform == null) {
            Debug.LogError("null reference to PlayerTransform");
        }

        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(PlayerTransform.position, this.transform.position) > MAX_DISTANCE) {
            AsteroidGenerator.Instance.AsteroidPool.Release(this);
        }
    }

    public int GetHealth()
    {
        throw new System.NotImplementedException();
    }


}
