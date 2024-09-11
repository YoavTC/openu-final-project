using UnityEngine;
using CodeMonkey.Utils;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerSettings towerSettings;
    private float elapsedTime;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer rangeRenderer;

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject targetPrefab;

    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = EnemyManager.Instance;
        VisualizeRange();
    }

    void Update()
    {
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

    private void Shoot(Enemy target)
    {
        Projectile newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        newArrow.Init(target.transform, towerSettings);
    }

    private void VisualizeRange()
    {
        float spriteDiameter = rangeRenderer.sprite.bounds.size.x;

        // Set the scale of the sprite to match the radius
        float scale = towerSettings.maxRange * 2 / spriteDiameter;

        // Apply the scale to the GameObject
        rangeRenderer.transform.localScale = new Vector3(scale, scale, 1f);
    }

    public float GetTowerRange() => towerSettings.maxRange;
}