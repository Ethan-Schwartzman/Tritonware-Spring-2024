using Unity.VisualScripting;
using UnityEngine;

public class ShieldPowerup : Powerup
{

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
        Debug.Log("shield activated");
    }
}