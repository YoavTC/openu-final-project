using UnityEngine;
using UnityEngine.Events;

public abstract class HealthBase: MonoBehaviour
{
    public float maxHealth { get; protected set; }
    public float health { get; protected set; }
    public bool isDead { get; protected set; }

    public UnityEvent OnHealEvent;
    public UnityEvent OnDamageEvent;
    public UnityEvent OnDieEvent;

    public virtual void TakeDamage(float amount)
    {
        OnDamageEvent?.Invoke();
        
        health -= amount;
        if (health <= 0)
        {
            OnDieEvent?.Invoke();
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        OnHealEvent?.Invoke();
        
        health = Mathf.Min(health + amount, maxHealth);
    }

    protected abstract void Die();
}