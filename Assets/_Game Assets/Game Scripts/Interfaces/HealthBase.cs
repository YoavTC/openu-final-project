using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class HealthBase: MonoBehaviour
{
    public float maxHealth { get; protected set; }
    public float health { get; protected set; }

    public bool isDead
    {
        get => _isDead;
        protected set
        {
             ensureDeathCoroutine = StartCoroutine(EnsureDeathCoroutine());
            _isDead = value;
        }
    }

    private bool _isDead;

    private Coroutine ensureDeathCoroutine;

    public UnityEvent<float> OnHealEvent;
    public UnityEvent<float> OnDamageEvent;
    public UnityEvent<float> OnDieEvent;
    public UnityEvent OnHealthInitializedEvent;
    
    private Slider healthBarSlider;
    private Canvas canvas;

    protected void SetHealth(float newHealth)
    {
        health = newHealth;
        maxHealth = health;
        
        InitializeHealthBarUI();
    }

    private void InitializeHealthBarUI()
    {
        canvas = GetComponentInChildren<Canvas>();
        healthBarSlider = GetComponentInChildren<Slider>();
        
        canvas.worldCamera = Camera.main;
        
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
        
        // if (GetComponent<Enemy>()) canvas.enabled = false;
        canvas.enabled = false;
    }

    private void UpdateHealthBarUI()
    {
        if (!canvas.enabled) canvas.enabled = true;
        healthBarSlider.value = health;
    }
    
    public virtual void TakeDamage(float amount) => ModifyHealth(-amount);
    public virtual void Heal(float amount) => ModifyHealth(+amount);
    
    private void ModifyHealth(float amount)
    {
        health = Mathf.Clamp((health + amount), 0f, maxHealth);
        
        if (amount > 0) OnHealEvent?.Invoke(health);
        else if (amount < 0) OnDamageEvent?.Invoke(health);
        
        UpdateHealthBarUI();
        
        if (health <= 0)
        {
            OnDieEvent?.Invoke(health);
            Die();
        }
    }

    private IEnumerator EnsureDeathCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Die();
    }

    protected virtual void Die()
    {
        StopCoroutine(ensureDeathCoroutine);
    }
    
    public void CalculateDamage(float damage)
    {
        float futureHealth = health - damage;
        if (futureHealth <= 0)
        {
            isDead = true;
        }
    }
}