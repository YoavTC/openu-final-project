using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthBase: MonoBehaviour, IModifierAffectable
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

    protected abstract void Die();

    #region Modifier Effects
    public ModifierEffect currentEffect { get; set; }
    
    public void StartEffect(ModifierEffect newModifierEffect)
    {
        currentEffect = newModifierEffect;
        if (currentEffect.type == ModifierEffectType.HEALTH)
        {
            Debug.Log($"Started effect {currentEffect.type} on {gameObject.name} for {currentEffect.duration}!");
            StartCoroutine(TickEffect());
        }
    }
    
    public IEnumerator TickEffect()
    {
        float durationProgress = 0f;
        float tickRate = currentEffect.tickRate;
        while (durationProgress <= currentEffect.duration)
        {
            ModifyHealth(currentEffect.strengthCurve.Evaluate(durationProgress));
            
            yield return new WaitForSeconds(tickRate);
            durationProgress += tickRate;
            Debug.Log($"{durationProgress}/{currentEffect.duration}");
        }
    }
    #endregion
}