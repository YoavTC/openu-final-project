using UnityEngine;

public class TowerDefault : TowerBase
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
        target = closestEnemy?.transform;
    }

    protected override void CooldownAction()
    {
        FindNextTarget();
        base.CooldownAction();
    }

    protected override void Shoot()
    {
        base.Shoot();
        target.GetComponent<Enemy>().CalculateDamage(towerSettings.damage);
    }
}
