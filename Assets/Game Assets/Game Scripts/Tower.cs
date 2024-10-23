using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : HealthBase, IPointerClickHandler
{
    private bool isPlaced = false;
    
    public TowerSettings towerSettings;
    private float elapsedTime;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer rangeRenderer;

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject targetPrefab;

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
    }

    private void Shoot(Enemy target)
    {
        Projectile newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        newArrow.Init(target.transform, towerSettings);
        target.CalculateDamage(towerSettings.damage);
    }

    protected override void Die()
    {
        throw new System.NotImplementedException();
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
