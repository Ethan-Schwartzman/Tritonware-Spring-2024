using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform PlayerTransform;

    private float speed = 100f;
    private Vector2 velocity, initialVelocity;
    private float damage = 5f;
    private float bulletRadius = 3f;
    private Vector2 direction;
    private List<Collider2D> collisions;
    private ContactFilter2D filter;
    private BulletSpawner spawner;

    void Awake()
    {
        //cc = GetComponent<CircleCollider2D>();
        collisions = new List<Collider2D>();
        filter = new ContactFilter2D();
        filter = filter.NoFilter();
    }

    public void SetDirection(Vector2 dir) {
        direction = dir;
    }

    public void SetRelativeVelocity(float s) {
        speed = s;
        velocity = ShipMovement.Instance.GetFacingDirection() * s + ShipMovement.Instance.GetVelocity();
    }

    public void SetSpawner(BulletSpawner s) {
        spawner = s;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(
            transform.position.x + velocity.x * Time.deltaTime,
            transform.position.y + velocity.y * Time.deltaTime
        );

        if(Vector2.Distance(transform.position, PlayerTransform.position) > 50f) {
            if(spawner!= null)spawner.Release(this); //TODO
        }

        if(Physics2D.OverlapCircle(transform.position, bulletRadius, filter, collisions) > 0) {
            foreach(Collider2D col in collisions) {
                Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
                if(asteroid != null) {
                    AsteroidGenerator.Instance.AsteroidPool.Release(asteroid);
                    if(spawner!= null)spawner.Release(this); //TODO
                }
            }
        }
    }
}
