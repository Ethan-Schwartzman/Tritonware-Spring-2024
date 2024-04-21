using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    const int POOL_DEFAULT = 100;
    const int POOL_MAX = 1000;

    public Bullet templateBullet;
    public FMODUnity.StudioEventEmitter SoundEffect;

    public IWeaponContainer weaponContainer;

    private ObjectPool<Bullet> bulletPool;

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
            Debug.LogError("Bullet Spawner not attached to weaponContainer");
        }
        bulletPool = new ObjectPool<Bullet>(
            CreateBullet,
            OnTakeFromPool,
            OnReleaseFromPool,
            DestroyBullet,
            true, POOL_DEFAULT, POOL_MAX
        );
    }

    private Bullet CreateBullet() {
        Bullet bullet = Instantiate(templateBullet);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void OnTakeFromPool(Bullet bullet) {
        bullet.SetVelocityFromParent(weaponContainer);
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseFromPool(Bullet bullet) {
        bullet.gameObject.SetActive(false);
    } 

    private void DestroyBullet(Bullet bullet) {
        Destroy(bullet);
    }

    public void SpawnBullet() {
        Bullet bullet = bulletPool.Get();
        bullet.SetSpawner(this);
        SoundEffect.Play();
    }

    public void Release(Bullet bullet) {
        bulletPool.Release(bullet);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
