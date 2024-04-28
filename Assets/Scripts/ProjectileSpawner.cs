using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public interface IWeapon
{
    public void Fire();
}


public class ProjectileSpawner : MonoBehaviour, IWeapon
{
    const int POOL_DEFAULT = 100;
    const int POOL_MAX = 1000;

    public Projectile templateBullet;
    public FMODUnity.StudioEventEmitter SoundEffect;

    public IWeaponContainer weaponContainer;

    private ObjectPool<Projectile> bulletPool;

    // Start is called before the first frame update
    void Start()
    {
        /*
        DynamicEntity parentEntity = GetComponent<DynamicEntity>();
        if (parentEntity is IWeaponContainer wc)
        {
            weaponContainer = wc;
        }
        else
        {
            Debug.LogError("Bullet Spawner not attached to weaponContainer");
        }
        */

        weaponContainer = GetComponent<IWeaponContainer>();
        if (weaponContainer == null )
        {
            weaponContainer = GetComponentInParent<IWeaponContainer>();
        }
        if (weaponContainer == null )
        {
            Debug.LogError("Bullet Spawner not attached to weaponContainer");
        }
        bulletPool = new ObjectPool<Projectile>(
            CreateBullet,
            OnTakeFromPool,
            OnReleaseFromPool,
            DestroyBullet,
            true, POOL_DEFAULT, POOL_MAX
        );
    }

    private Projectile CreateBullet() {
        Projectile bullet = Instantiate(templateBullet);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void OnTakeFromPool(Projectile bullet) {
        if(bullet.trailRenderer != null) {
            bullet.trailRenderer.Clear();
        }
        bullet.gameObject.SetActive(true);
        bullet.SetVelocityFromParent(weaponContainer);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        
    }

    private void OnReleaseFromPool(Projectile bullet) {
        bullet.gameObject.SetActive(false);
    } 

    private void DestroyBullet(Projectile bullet) {
        Destroy(bullet);
    }

    public void Fire() {
        Projectile bullet = bulletPool.Get();
        bullet.SetSpawner(this);
        
        if (SoundEffect != null) SoundEffect.Play();
    }


    public void Release(Projectile bullet) {
        if (bullet.isActiveAndEnabled) 
        bulletPool.Release(bullet);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
