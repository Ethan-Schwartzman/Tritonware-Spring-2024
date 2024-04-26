using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    Transform PlayerTransform;

    public float speed = 50f;
    
    public int damage = 3;
    [SerializeField] protected float bulletRadius = 3f;
    protected List<Collider2D> collisions;
    protected ContactFilter2D filter;
    protected ProjectileSpawner spawner;
    protected Team team;

    protected virtual void Awake()
    {
        //cc = GetComponent<CircleCollider2D>();
        collisions = new List<Collider2D>();
        filter = new ContactFilter2D();
        filter = filter.NoFilter();

    }



    public void SetVelocityFromParent(IWeaponContainer weaponContainer) {
        SetVelocity(weaponContainer.GetAimDirection() * speed + weaponContainer.GetVelocity());
    }

    protected abstract void SetVelocity(Vector2 vel);

    // dictates how the projectile moves
    protected abstract void FixedUpdate();

    public void SetSpawner(ProjectileSpawner s) {
        spawner = s;
        team = spawner.weaponContainer.GetTeam();
    }



    // Update is called once per frame
    protected virtual void Update()
    {
        CheckDespawn();
        CheckCollision();
    }

    private void CheckDespawn()
    {
        if (spawner == null)
        {
            Destroy(gameObject, 2f);
        }
        else if (Vector2.Distance(transform.position, PlayerShip.GetPosition()) > 100f)
        {
            if (spawner != null) spawner.Release(this); //TODO
        }
    }

    private void CheckCollision()
    {
        if (Physics2D.OverlapCircle(transform.position, bulletRadius, filter, collisions) > 0)
        {
            foreach (Collider2D col in collisions)
            {
                IDamagable hit = col.gameObject.GetComponent<IDamagable>();
                if (hit != null && hit.GetTeam() != team)
                {
                    HitTarget(hit);
                }
            }
        }
    }

    private void HitTarget(IDamagable target)
    {
        target.DealDamage(damage);
        Despawn();
    }

    protected void Despawn()
    {
        if (spawner != null && isActiveAndEnabled) spawner.Release(this);
        else Destroy(gameObject);
    }

    
}
