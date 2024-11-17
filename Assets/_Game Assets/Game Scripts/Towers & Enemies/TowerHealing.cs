public class TowerHealing : TowerBase
{
    private HealthBaseListManager towerManager;

    public override void TowerPlaced(TowerSettings towerSettings)
    {
        towerManager = TowerManager.Instance;;
        base.TowerPlaced(towerSettings);
    }

    //Todo: Make this tower find other towers that need healing instead of enemies!!
    protected override void FindNextTarget()
    {
        TowerBase closestEnemy = (TowerBase) towerManager.GetClosestHurtEntity(transform, towerSettings.maxRange);
        //if (closestEnemy != null) currentTarget = closestEnemy.transform;
        currentTarget = closestEnemy?.transform;
    }
    
    protected override void CooldownAction()
    {
        FindNextTarget();
        base.CooldownAction();
    }
}
