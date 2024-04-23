using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform PlayerTransform;

    public float speed = 50f;
    private Vector2 velocity;
    public int damage = 3;
    [SerializeField] private float bulletRadius = 3f;
    private List<Collider2D> collisions;
    private ContactFilter2D filter;
    private BulletSpawner spawner;
    Team team;

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
        team = spawner.weaponContainer.GetTeam();
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)velocity * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(Vector2.Distance(transform.position, PlayerShip.GetPosition()) > 50f) {
            if(spawner!= null)spawner.Release(this); //TODO
        }

        if(Physics2D.OverlapCircle(transform.position, bulletRadius, filter, collisions) > 0) {
            foreach(Collider2D col in collisions) {
                IDamagable hit = col.gameObject.GetComponent<IDamagable>();
                if(hit != null && hit.GetTeam() != team) {
                    HitTarget(hit);
                }
            }
        }

        if (spawner == null)
        {
            Destroy(gameObject, 2f);
        }
    }

    private void HitTarget(IDamagable target)
    {
        target.DealDamage(damage);
        if (spawner != null) spawner.Release(this);
        else Destroy(gameObject);
    }

    
}
