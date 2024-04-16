using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform PlayerTransform;

    public float speed = 50f;
    private Vector2 velocity;
    public int damage = 3;
    private float bulletRadius = 3f;
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



    public void SetVelocityFromParent(IWeaponContainer weaponContainer) {
        velocity = weaponContainer.GetAimDirection() * speed + weaponContainer.GetVelocity();
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

        if(Vector2.Distance(transform.position, PlayerShip.GetPosition()) > 50f) {
            if(spawner!= null)spawner.Release(this); //TODO
        }

        if(Physics2D.OverlapCircle(transform.position, bulletRadius, filter, collisions) > 0) {
            foreach(Collider2D col in collisions) {
                IDamagable hit = col.gameObject.GetComponent<IDamagable>();
                if(hit != null && col.gameObject != spawner.gameObject) {
                    HitTarget(hit);
                }
            }
        }
    }

    private void HitTarget(IDamagable target)
    {
        target.DealDamage(damage);
        if (spawner != null) spawner.Release(this); //TODO
    }

}
