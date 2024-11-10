public class TowerHealing : TowerBase
{
    private EnemyManager enemyManager;
    
    protected override void Start()
    {
        enemyManager = EnemyManager.Instance;
        base.Start();
    }

    //Todo: Make this tower find other towers that need healing instead of enemies!!
    protected override void FindNextTarget()
    {
        Enemy closestEnemy = enemyManager.GetClosestEnemy(transform.position, towerSettings.maxRange);
        if (closestEnemy != null) target = closestEnemy.transform;
    }
    
    protected override void CooldownAction()
    {
        FindNextTarget();
        base.CooldownAction();
    }
}
