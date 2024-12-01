using UnityEngine;

public class EnemyShooter : TowerBase
{
    private HealthBaseListManager towersManager;
    
    public override void InitializeComponents(TowerSettings towerSettings)
    {
        this.towerSettings = towerSettings;
        interactable = false;
        
        attackCooldown = towerSettings.attackCooldown;
        isPlaced = true;
        
        towersManager = TowerManager.Instance;
        projectilePrefab = Utility.GetProjectilePrefab();
    }

    protected override void FindNextTarget()
    {
        TowerBase closestEntity = (TowerBase) towersManager.GetClosestEntity(transform, towerSettings.maxRange);
        currentTarget = closestEntity?.transform;
        Debug.Log($"Found next target {currentTarget}");
    }

    protected override void CooldownAction()
    {
        Debug.Log("Cooldown action");
        FindNextTarget();
        base.CooldownAction();
    }

    protected override void Shoot()
    {
        base.Shoot();
        currentTarget.GetComponent<TowerBase>().CalculateDamage(towerSettings.damage);
    }
}
