using UnityEngine;

public abstract class Powerup: MonoBehaviour
{
    bool collected;
    public bool isActive;
    public const float COLLECT_DURATION = 5f;
    public float activatedDuration = 0f;

    float spawnTime;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnTime = Time.time;
    }

    protected virtual void Update()
    {
        if (Time.time - spawnTime > COLLECT_DURATION)
        {
            Destroy(this);
            return;
        }
        if (isActive) activatedDuration += Time.deltaTime;
        if (activatedDuration > GetDuration())
        {
            Finish();
        }
    }

    public virtual void Init(Vector3 pos, Vector3 vel)
    {
        transform.position = pos;
        rb.velocity = vel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");
        if (collision.gameObject == PlayerShip.Instance.gameObject)
        {
            Powerup pow = PlayerShip.Instance.currentPowerup;
            if (pow == null || (pow != null && !pow.isActive))
            {
                Collect();
            }
               
        }
    }

    

    public void Collect()
    {
        spriteRenderer.enabled = false;
        transform.SetParent(PlayerShip.Instance.transform);
        PlayerShip.Instance.SetPowerup(this);
        collected = true;
    }

    protected virtual void Finish()
    {
        PlayerShip.Instance.SetPowerup(null);
        Destroy(gameObject);
    }

    public abstract string GetName();

    public virtual void Activate()
    {
        isActive = true;
    }



    public abstract float GetDuration();

}