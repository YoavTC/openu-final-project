﻿using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public abstract class TowerBase : EntityBase, IPointerClickHandler
{
    public TowerSettings towerSettings;
    
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected SpriteRenderer rangeRenderer;
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected Transform currentTarget;

    public UnityEvent OnTowerPlacedEvent;
    public UnityEvent OnProjectileFiredEvent;

    protected float attackCooldown;
    private float elapsedTime;
    protected bool isPlaced = false;
    
    public virtual void InitializeComponents(TowerSettings towerSettings)
    {
        this.towerSettings = towerSettings;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rangeRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        projectilePrefab = Utility.GetProjectilePrefab();

        spriteRenderer.sprite = towerSettings.sprite;
        attackCooldown = towerSettings.attackCooldown;
        
        SetHealth(towerSettings.health);
        InitializeVisualRange();
    }

    public void InitializeUnityEventListeners(
        UnityEvent OnTowerPlacedEvent,
        UnityEvent OnProjectileFiredEvent,
        UnityEvent<float> OnHealEvent,
        UnityEvent<float> OnDamageEvent,
        UnityEvent<float> OnDieEvent,
        UnityEvent OnHealthInitializedEvent
        )
    {
        this.OnTowerPlacedEvent = OnTowerPlacedEvent;
        this.OnProjectileFiredEvent = OnProjectileFiredEvent;

        this.OnHealEvent = OnHealEvent;
        this.OnDamageEvent = OnDamageEvent;
        this.OnDieEvent = OnDieEvent;
        this.OnHealthInitializedEvent = OnHealthInitializedEvent;
    }
    
    public virtual void OnTowerPlaced()
    {
        GetComponent<Collider2D>().enabled = true;
        isPlaced = true;
        TowerManager.Instance.AddEntity(this);
        
        transform.DOPunchScale(transform.localScale * 0.5f, 0.5f);
        ToggleVisualRange(false);
        
        OnTowerPlacedEvent?.Invoke();
    }
    
    protected virtual void Update()
    {
        if (!isPlaced) return;
        
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= attackCooldown)
        {
            elapsedTime = 0f;
            CooldownAction();
        }
    }
    
    #region Health
    public override void TakeDamage(float amount)
    {
        if (!isPlaced) return;
        base.TakeDamage(amount);
    }
    
    protected override void Die()
    {
        ParticlesManager.Instance.PlayTowerDeath(transform.position);
        StartCoroutine(DeathCoroutine());
    }
    
    private IEnumerator DeathCoroutine()
    {
        //Wait for the Enemy manager to safely remove enemy from list
        bool callback = false;
        TowerManager.Instance.RemoveEntity(this, () => callback = true);
        yield return new WaitUntil(() => callback);
        
        transform.DOKill();
        transform.DOComplete();
        Destroy(gameObject);
    }
    #endregion

    #region Shooting
    protected virtual void FindNextTarget() {}
    
    protected virtual void CooldownAction()
    {
        if (currentTarget != null) Shoot();
    }
    
    protected virtual void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity, InSceneParentProvider.GetParent(SceneParentProviderType.PROJECTILES))
                    .InitializeProjectile(currentTarget, transform, towerSettings);
        
        OnProjectileFiredEvent?.Invoke();
    }
    #endregion

    #region Effect Handling
    protected override void ApplyEffect(ModifierEffectType type, float amount)
    {
        switch (type)
        {
            case ModifierEffectType.HEALTH:
                TakeDamage(amount);
                break;
            case ModifierEffectType.SPEED:
                attackCooldown = amount * towerSettings.attackCooldown;
                break;
            default:
                return;
        }
    }

    public override void FinishEffect()
    {
        attackCooldown = towerSettings.attackCooldown;
        base.FinishEffect();
    }
    #endregion

    #region Inspecting
    protected bool interactable = true;
    
    private void InitializeVisualRange()
    {
        float scale = towerSettings.maxRange * 2 / rangeRenderer.sprite.bounds.size.x;
        rangeRenderer.transform.localScale = new Vector3(scale, scale, 1f);
    }
    
    public void ToggleVisualRange(bool state)
    {
        if (rangeRenderer != null)
        { 
            rangeRenderer.enabled = state;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable) SelectionManager.Instance.OnSelectableItemClicked(this);
    }
    #endregion
}