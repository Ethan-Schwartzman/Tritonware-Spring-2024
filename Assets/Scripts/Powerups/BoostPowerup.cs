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
        Debug.Log("boost activated");
    }

    public override float GetDuration()
    {
        return 5f;
    }
}