using UnityEngine;

public abstract class Powerup: MonoBehaviour
{
    bool collected;
    public static float collectDuration = 5f;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector3 pos, Vector3 vel)
    {
        transform.position = pos;
        rb.velocity = vel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");
        if (collision.gameObject == PlayerShip.Instance.gameObject)
        {
            Collect();
        }
    }

    

    public void Collect()
    {
        spriteRenderer.enabled = false;
        transform.SetParent(PlayerShip.Instance.transform);
        PlayerShip.Instance.SetPowerup(this);
        collected = true;
    }

    public abstract string GetName();

    public abstract void Activate();

}