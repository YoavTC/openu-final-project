using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : HealthBase, IPointerClickHandler
{
    private bool isPlaced = false;
    
    public TowerSettings towerSettings;
    private float elapsedTime;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer rangeRenderer;

    [SerializeField] private Projectile arrowPrefab;

    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = EnemyManager.Instance;
        SetHealth(towerSettings.health);
        InitializeVisualRange();
    }

    void Update()
    {
        if (!isPlaced) return;
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= towerSettings.attackCooldown)
        {
            elapsedTime = 0f;
            
            Enemy enemy = enemyManager.GetClosestEnemy(transform.position, towerSettings.maxRange);
            if (enemy != null)
            {
                Shoot(enemy);
            }
        }
    }

    public void OnTowerPlacedEventListener()
    {
        isPlaced = true;
        ToggleVisualRange(false);
        transform.DOPunchScale(transform.localScale * 0.5f, 0.5f);
    }

    private void Shoot(Enemy target)
    {
        Projectile newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        newArrow.Init(target.transform, towerSettings, transform);
        if (towerSettings.projectileModifierEffect)
        {
            newArrow.GetComponent<ProjectileModifierEffect>().modifierEffect = towerSettings.projectileModifierEffect;
        }
        target.CalculateDamage(towerSettings.damage);
    }
    
    private void InitializeVisualRange()
    {
        float spriteDiameter = rangeRenderer.sprite.bounds.size.x;

        // Set the scale of the sprite to match the radius
        float scale = towerSettings.maxRange * 2 / spriteDiameter;

        // Apply the scale to the GameObject
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
