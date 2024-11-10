public class TowerEmpty : TowerBase
{
    public override void OnTowerPlacedEventListener()
    {
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
        
        newTowerComponent.InitializeComponents(
            towerSettings,
            spriteRenderer,
            rangeRenderer,
            projectilePrefab);
        
        base.OnTowerPlacedEventListener();
        Destroy(this);
    }
}