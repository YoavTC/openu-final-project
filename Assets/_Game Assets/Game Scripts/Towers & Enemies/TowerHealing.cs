public class TowerHealing : TowerBase
{
    private HealthBaseListManager towerManager;
    
    protected override void Start()
    {
        towerManager = TowerManager.Instance;;
        base.Start();
    }

    //Todo: Make this tower find other towers that need healing instead of enemies!!
    protected override void FindNextTarget()
    {
        TowerBase closestEnemy = (TowerBase) towerManager.GetClosestEntity(transform, towerSettings.maxRange);
        if (closestEnemy != null) target = closestEnemy.transform;
    }
    
    protected override void CooldownAction()
    {
        FindNextTarget();
        base.CooldownAction();
    }
}
