using External_Packages;

public class SelectionManager : Singleton<SelectionManager>
{
    private Tower lastTowerClicked;
    private SelectableInformationObject lastSelectableInformation;
    
    public void OnSelectableItemClicked(Tower tower, SelectableInformationObject selectableInformation)
    {
        lastTowerClicked?.ToggleVisualRange(false);
        
        lastSelectableInformation = selectableInformation;
        lastTowerClicked = tower;
        
        lastTowerClicked.ToggleVisualRange(true);
    }
}