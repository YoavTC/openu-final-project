public class TowerEmpty : TowerBase
{
    public override void TowerPlaced(TowerSettings towerSettings)
    {
        base.TowerPlaced(towerSettings);
        
        TowerBase newTowerComponent = null;
        switch (towerSettings.baseBehaviourType)
        {
            case TowerBaseBehaviourType.DEFAULT:
                newTowerComponent = gameObject.AddComponent<TowerDefault>();
                break;
            case TowerBaseBehaviourType.HEALING:
                newTowerComponent = gameObject.AddComponent<TowerHealing>();
                break;
            case TowerBaseBehaviourType.IDLE:
                newTowerComponent = gameObject.AddComponent<TowerIdle>();
                break;
            default:
                newTowerComponent = gameObject.AddComponent<TowerDefault>();
                break;
        }
        
        newTowerComponent.TowerPlaced(towerSettings);
        newTowerComponent.InitializeComponents(
            spriteRenderer,
            rangeRenderer,
            projectilePrefab);
        
        Destroy(this);
    }
}