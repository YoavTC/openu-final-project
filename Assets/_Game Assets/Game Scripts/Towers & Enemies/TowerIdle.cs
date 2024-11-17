public class TowerIdle : TowerBase
{
    private int damage;

    protected override void Start()
    {
        base.Start();
        damage = (int) towerSettings.damage;
    }

    protected override void CooldownAction()
    {
        ElixirManager.Instance.IncreaseElixir(damage);
        ParticlesManager.Instance.PlayElixirReward(transform.position, damage);
    }
}
