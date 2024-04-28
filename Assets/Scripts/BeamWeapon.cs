using Unity.Burst.CompilerServices;
using UnityEngine;

public class BeamWeapon : MonoBehaviour, IWeapon
{
    IWeaponContainer wc;



    public float beamDuration;

    public float beamInterval = 0.5f;
    public int beamDamage = 1;

    bool beamEnabled = false;

    LineRenderer lineRenderer;

    private void Awake()
    {
        wc = GetComponent<IWeaponContainer>();
        if (wc == null)
        {
            wc = GetComponentInParent<IWeaponContainer>();
        }
        lineRenderer = GetComponent<LineRenderer>();
        
    }

    private void Update()
    {


        if (beamEnabled) beamDuration -= Time.deltaTime;
        if (beamDuration > -0.05)
        {
            RaycastHit2D beamHit = Physics2D.Raycast(transform.position, wc.GetAimDirection());
            lineRenderer.SetPosition(0, transform.position);
            if (beamHit.collider != null)
            {
                lineRenderer.SetPosition(1, beamHit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, transform.position + 100 * (Vector3)wc.GetAimDirection());
            }
        }
        else
        {
            lineRenderer.enabled = false;
            beamEnabled = false;
        }

    }



    public void Fire()
    {
        beamEnabled = true;
        lineRenderer.enabled = true;
        beamDuration = beamInterval;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, wc.GetAimDirection());
        if (hit.collider != null)
        {
            IDamagable target = hit.collider.GetComponent<IDamagable>();
            if (target != null)
            {
                target.DealDamage(beamDamage);
            }
        }
        
    }

    public Vector3 GetFirePosition()
    {
        return transform.position;
    }

    public float GetFireInterval()
    {
        return beamInterval;
    }

    public bool CanFire()
    {
        return (beamDuration <= 0);
    }
}


