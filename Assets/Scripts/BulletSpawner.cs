using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    const int POOL_DEFAULT = 100;
    const int POOL_MAX = 1000;

    public Bullet templateBullet;

    private ObjectPool<Bullet> bulletPool;

    // Start is called before the first frame update
    void Start()
    {
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
        bullet.SetDirection(ShipMovement.Instance.GetFacingDirection());
        bullet.SetRelativeVelocity(ShipMovement.Instance.GetSpeed() + 50f);
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
    }

    public void Release(Bullet bullet) {
        bulletPool.Release(bullet);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire")) {
            SpawnBullet();
        }
    }
}
