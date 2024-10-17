using System;
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
    
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        OnDamageEvent?.Invoke(health);
        
        if (health <= 0)
        {
            OnDieEvent?.Invoke(health);
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        OnHealEvent?.Invoke(health);
    }

    protected abstract void Die();
}