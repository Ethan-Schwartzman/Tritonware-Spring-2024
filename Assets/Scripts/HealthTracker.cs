using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthTracker
{
    public int health { get; private set; }
    public int maxHealth;
    IDamagable attachedEntity;

    public HealthTracker(IDamagable attachedEntity, int maxHealth)
    {
        this.attachedEntity = attachedEntity;
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            attachedEntity.TriggerDeath();
        }
            
    }

    

}
