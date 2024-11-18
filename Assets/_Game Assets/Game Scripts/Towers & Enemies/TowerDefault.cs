using UnityEngine;

public class TowerDefault : TowerBase
{
    private HealthBaseListManager enemyManager;

    // public override void TowerPlaced(TowerSettings towerSettings)
    // {
    //     enemyManager = EnemyManager.Instance;
    //     base.TowerPlaced(towerSettings);
    // }

    public override void InitializeComponents(TowerSettings towerSettings)
    {
        enemyManager = EnemyManager.Instance;
        base.InitializeComponents(towerSettings);
    }

    protected override void FindNextTarget()
    {
        Enemy closestEnemy = (Enemy) enemyManager.GetClosestEntity(transform, towerSettings.maxRange);
        currentTarget = closestEnemy?.transform;
    }

    protected override void CooldownAction()
    {
        FindNextTarget();
        base.CooldownAction();
    }

    protected override void Shoot()
    {
        base.Shoot();
        currentTarget.GetComponent<Enemy>().CalculateDamage(towerSettings.damage);
    }
}
