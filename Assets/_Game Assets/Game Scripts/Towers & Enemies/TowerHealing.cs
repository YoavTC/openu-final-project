using UnityEngine;

public class TowerHealing : TowerBase
{
    private HealthBaseListManager towerManager;
    
    public override void InitializeComponents(TowerSettings towerSettings)
    {
        towerManager = TowerManager.Instance;
        base.InitializeComponents(towerSettings);
    }
    
    protected override void FindNextTarget()
    {
        TowerBase closestEnemy = (TowerBase) towerManager.GetClosestHurtEntity(transform, towerSettings.maxRange);
        currentTarget = closestEnemy?.transform;
    }
    
    protected override void CooldownAction()
    {
        FindNextTarget();
        base.CooldownAction();
    }
}
