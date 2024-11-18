public class TowerIdle : TowerBase
{
    private int damage;
    
    public override void InitializeComponents(TowerSettings towerSettings)
    {
        damage = (int) towerSettings.damage;
        base.InitializeComponents(towerSettings);
    }

    protected override void CooldownAction()
    {
        ElixirManager.Instance.IncreaseElixir(damage);
        ParticlesManager.Instance.PlayElixirReward(transform.position, damage);
    }
}
