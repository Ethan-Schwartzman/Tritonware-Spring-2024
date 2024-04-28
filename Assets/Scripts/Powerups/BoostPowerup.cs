using UnityEngine;

public class BoostPowerup : Powerup
{
    public override string GetName()
    {
        return "Boost";
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