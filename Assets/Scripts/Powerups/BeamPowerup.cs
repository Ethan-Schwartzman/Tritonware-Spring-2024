using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BeamPowerup : Powerup, IWeaponContainer
{

    BeamWeapon beam;

    protected override void Awake()
    {
        base.Awake();
        beam = GetComponentInChildren<BeamWeapon>();
        overrideFinish = true;
    }

    public override string GetName()
    {
        return "Beam";
    }
    public override void Activate()
    {
        base.Activate();
    }

    protected override void Update()
    {
        base.Update();
        if (isActive && beam.CanFire())
        {
            beam.Fire();
        }
        if (beam.overheated) Finish();
    }

    public override void Finish()
    {
        beam.ResetHeat();
        base.Finish();
    }

    public override float GetDuration()
    {
        return beam.heatCapacity;
    }

    public override PowerupState GetPowerupState()
    {
        return PowerupState.Boost;
    }

    public Vector2 GetAimDirection()
    {
        return PlayerShip.Instance.GetAimDirection();
    }

    public Team GetTeam()
    {
        return Team.player;
    }

    public Vector2 GetVelocity()
    {
        return PlayerShip.Instance.GetVelocity();
    }

    public Vector2 GetFacingDirection()
    {
        return PlayerShip.Instance.GetFacingDirection();
    }
}