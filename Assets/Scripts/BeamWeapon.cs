using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BeamWeapon : MonoBehaviour, IWeapon
{
    IWeaponContainer wc;



    public float beamDuration;

    public float beamInterval = 0.5f;
    public int beamDamage = 1;

    float overheatDuration = 0f;
    public float heatCapacity = 2f;
    public bool overheated = false;
    public float overheatPenalty = 0f;

    public float radius = 1f;

    bool beamEnabled = false;

    LineRenderer lineRenderer;


    public bool collideEnemies;
    int layerMask;

    [SerializeField] Color beamColor, overheatColor;

    

    private void Awake()
    {
        if (collideEnemies)
            layerMask = LayerMask.GetMask("Enemy Ship", "Asteroid");
        else
            layerMask = LayerMask.GetMask("Player Ship", "Asteroid");



        wc = GetComponent<IWeaponContainer>();
        if (wc == null)
        {
            wc = GetComponentInParent<IWeaponContainer>();
        }
        lineRenderer = GetComponent<LineRenderer>();
        
        
    }

    private void Update()
    {
        if (beamEnabled)
        {
            beamDuration -= Time.deltaTime;
            overheatDuration += Time.deltaTime;
        }
        else if (overheatDuration > 0f)
        {
            overheatDuration -= Time.deltaTime;
        }
        if (overheatDuration <= 0f)
        {
            overheated = false;
        }
        if (beamEnabled && beamDuration > -0.02)
        {
            RaycastHit2D beamHit = Physics2D.Raycast(transform.position, wc.GetAimDirection(), 100, layerMask);
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

        if (overheatDuration > heatCapacity)
        {
            if (!overheated)
            {
                overheated = true;
                overheatDuration += overheatPenalty;
            }
            
            
        }

        lineRenderer.startColor = Color.Lerp(beamColor, overheatColor, overheatDuration/heatCapacity);
        lineRenderer.endColor = Color.Lerp(beamColor, overheatColor, overheatDuration / heatCapacity);
    }



    public void Fire()
    {
        beamEnabled = true;
        lineRenderer.enabled = true;
        beamDuration = beamInterval;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, wc.GetAimDirection(), 100, layerMask);
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
        return (beamDuration <= 0 && !overheated);
    }

    public void ResetHeat()
    {
        beamEnabled = false;
        overheated = false;
        overheatDuration = 0;
        beamDuration = 0;
    }
}


