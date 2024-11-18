using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public abstract class TowerBase : EntityBase, IPointerClickHandler
{
    public TowerSettings towerSettings;
    
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected SpriteRenderer rangeRenderer;
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected Transform currentTarget;

    private float attackCooldown;
    private float elapsedTime;
    private bool isPlaced = false;

    //Called before tower placed
    protected virtual void Start()
    {
        attackCooldown = towerSettings.attackCooldown;
        SetHealth(towerSettings.health);
        InitializeVisualRange();
    }
    
    public virtual void TowerPlaced(TowerSettings towerSettings)
    {
        this.towerSettings = towerSettings;
    }
    
    //Called after tower placed
    public void InitializeComponents(SpriteRenderer spriteRenderer, SpriteRenderer rangeRenderer, Projectile projectilePrefab)
    {
        this.spriteRenderer = spriteRenderer;
        this.rangeRenderer = rangeRenderer;
        this.projectilePrefab = projectilePrefab;

        TowerManager.Instance.AddEntity(this);
        isPlaced = true;
        
        spriteRenderer.sprite = towerSettings.sprite;
        transform.DOPunchScale(transform.localScale * 0.5f, 0.5f);
        ToggleVisualRange(false);
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
    
    protected virtual void CooldownAction()
    {
        if (currentTarget != null) Shoot();
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

        // yield return new WaitForSeconds(.5f);
        transform.DOKill();
        transform.DOComplete();
        Destroy(gameObject);
    }

    #region Shooting
    protected virtual void FindNextTarget() { }
    
    protected virtual void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity, InSceneParentProvider.GetParent(SceneParentProviderType.PROJECTILES))
                    .InitializeProjectile(currentTarget, transform, towerSettings);
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
        SelectionManager.Instance.OnSelectableItemClicked(this);
    }
    #endregion

    public override void TakeDamage(float amount)
    {
        if (!isPlaced) return;
        base.TakeDamage(amount);
    }
}