using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthBase: MonoBehaviour
{
    public float maxHealth { get; protected set; }
    public float health { get; protected set; }
    public bool isDead { get; protected set; }

    public UnityEvent<float> OnHealEvent;
    public UnityEvent<float> OnDamageEvent;
    public UnityEvent<float> OnDieEvent;

    protected void SetHealth(float health)
    {
        this.health = health;
        maxHealth = health;
    }
    
    public virtual void TakeDamage(float amount) => ModifyHealth(-amount);
    public virtual void Heal(float amount) => ModifyHealth(+amount);
    
    private void ModifyHealth(float amount)
    {
        health += amount;
        
        if (amount > 0) OnHealEvent?.Invoke(health);
        else if (amount < 0) OnDamageEvent?.Invoke(health);
        
        if (health <= 0)
        {
            OnDieEvent?.Invoke(health);
            Die();
        }
    }

    protected virtual void Die() { }
}