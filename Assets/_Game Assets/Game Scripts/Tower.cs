public class Tower : TowerBase
{
    private EnemyManager enemyManager;
    
    protected override void Start()
    {
        enemyManager = EnemyManager.Instance;
        base.Start();
    }

    protected override void FindNextTarget()
    {
        Enemy closestEnemy = enemyManager.GetClosestEnemy(transform.position, towerSettings.maxRange);
        if (closestEnemy != null) target = closestEnemy.transform;
    }

    protected override void PostShootAction()
    {
        target.GetComponent<Enemy>().CalculateDamage(towerSettings.damage);
    }
}
