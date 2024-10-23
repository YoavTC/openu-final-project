using External_Packages;

public class SelectionManager : Singleton<SelectionManager>
{
    private Tower lastTowerClicked;
    
    public void OnSelectableItemClicked(Tower tower)
    {
        lastTowerClicked?.ToggleVisualRange(false);
        
        lastTowerClicked = tower;
        
        lastTowerClicked.ToggleVisualRange(true);
    }
}