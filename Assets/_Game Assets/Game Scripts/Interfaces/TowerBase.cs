using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public abstract class TowerBase : HealthBase, IPointerClickHandler
{
    public TowerSettings towerSettings;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer rangeRenderer;
    public Projectile projectilePrefab;
    
    protected Transform target;
    private float elapsedTime;
    private bool isPlaced = false;

    public void InitializeComponents(TowerSettings towerSettings, SpriteRenderer spriteRenderer, SpriteRenderer rangeRenderer, Projectile projectilePrefab)
    {
        this.towerSettings = towerSettings;
        this.spriteRenderer = spriteRenderer;
        this.rangeRenderer = rangeRenderer;
        this.projectilePrefab = projectilePrefab;

        isPlaced = true;
        
        TowerManager.Instance.AddEntity(this);
    }

    protected virtual void Start()
    {
        SetHealth(towerSettings.health);
        InitializeVisualRange();
    }

    protected virtual void Update()
    {
        if (!isPlaced) return;
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= towerSettings.attackCooldown)
        {
            elapsedTime = 0f;
            CooldownAction();
        }
    }

    public virtual void OnTowerPlacedEventListener()
    {
        isPlaced = true;
        ToggleVisualRange(false);
        transform.DOPunchScale(transform.localScale * 0.5f, 0.5f);
    }

    protected virtual void FindNextTarget()
    {
        
    }

    protected virtual void CooldownAction()
    {
        if (target != null) Shoot();
    }
    
    protected virtual void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity, InSceneParentProvider.GetParent(SceneParentProviderType.PROJECTILES))
                    .InitializeProjectile(target, transform, towerSettings);
    }
    
    private void InitializeVisualRange()
    {
        float scale = towerSettings.maxRange * 2 / rangeRenderer.sprite.bounds.size.x;
        rangeRenderer.transform.localScale = new Vector3(scale, scale, 1f);
    }

    public void ToggleVisualRange(bool show)
    {
        rangeRenderer.enabled = show;
    }

    public float GetTowerRange() => towerSettings.maxRange;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        SelectionManager.Instance.OnSelectableItemClicked(this);
    }
}