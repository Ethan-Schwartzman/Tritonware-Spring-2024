using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ShieldPowerup : Powerup
{
    [SerializeField] SpriteRenderer shieldSpriteRenderer;

    protected override void Awake()
    {
        base.Awake();
    }

    public override string GetName()
    {
        return "Shield";

    }

    public override float GetDuration()
    {
        return 5f;
    }


    public override void Activate()
    {
        base.Activate();
        
        transform.position = PlayerShip.Instance.transform.position;
        shieldSpriteRenderer.enabled = true;
    }

    public override void Finish()
    {

        base.Finish();
    }

    public override PowerupState GetPowerupState()
    {
        return PowerupState.Shield;
    }
}