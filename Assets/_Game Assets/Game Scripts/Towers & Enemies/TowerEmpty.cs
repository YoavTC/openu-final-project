public class TowerEmpty : TowerBase
{
    private TowerSettings savedTowerSettings;
    
    public override void InitializeComponents(TowerSettings towerSettings)
    {
        savedTowerSettings = towerSettings;
        base.InitializeComponents(towerSettings);
    }

    public override void OnTowerPlaced()
    {
        TowerBase newTowerComponent = null;
        switch (savedTowerSettings.baseBehaviourType)
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
            case TowerBaseBehaviourType.BOOSTING:
                newTowerComponent = gameObject.AddComponent<TowerBoosting>();
                break;
            default:
                newTowerComponent = gameObject.AddComponent<TowerDefault>();
                break;
        }
        
        newTowerComponent.InitializeComponents(savedTowerSettings);
        newTowerComponent.OnTowerPlaced();
        
        Destroy(this);
    }
}