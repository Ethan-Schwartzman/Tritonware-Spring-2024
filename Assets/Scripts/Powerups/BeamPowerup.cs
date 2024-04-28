using UnityEngine;

public class BeamPowerup : Powerup
{
    
    


    protected override void Awake()
    {
        base.Awake();

    }

    public override string GetName()
    {
        return "Beam";
    }
    public override void Activate()
    {
        base.Activate();
        PlayerShip.Instance.ToggleBoost(true);
    }

    public override void Finish()
    {
        PlayerShip.Instance.ToggleBoost(false);
        base.Finish();
    }

    public override float GetDuration()
    {
        return 3f;
    }

    public override PowerupState GetPowerupState()
    {
        return PowerupState.Boost;
    }
}