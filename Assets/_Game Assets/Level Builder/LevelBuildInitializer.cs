using UnityEngine;

public class LevelBuildInitializer : MonoBehaviour
{
    public LevelBuildSO levelBuild;

    private void Start()
    {
        InitializeEnemySpawner();
        InitializeInventory();
    }

    private void InitializeEnemySpawner()
    {
        EnemySpawner enemySpawner = FindFirstObjectByType<EnemySpawner>();
        enemySpawner.InitializeSpawner(levelBuild);
    }

    private void InitializeInventory()
    {
        InventoryUIManager inventoryUIManager = FindFirstObjectByType<InventoryUIManager>();
        inventoryUIManager.InitializeInventory(levelBuild);        
    }
}
