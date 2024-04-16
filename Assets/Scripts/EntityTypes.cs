using UnityEngine;

public interface IDamagable
{
    public int GetHealth();
    public void DealDamage(int damage);
    public void TriggerDeath();

}

public interface IDynamicEntity
{
    public Vector2 GetVelocity();
    public Vector2 GetFacingDirection();

}


// mainly used for AI ships or special weapons that do not have to shoot where they are pointing
public interface IWeaponContainer: IDynamicEntity
{
    public Vector2 GetAimDirection();
}