using External_Packages;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    private TowerBase lastTowerClicked;
    
    public void OnSelectableItemClicked(TowerBase tower)
    {
        lastTowerClicked?.ToggleVisualRange(false);
        lastTowerClicked = tower;
        lastTowerClicked.ToggleVisualRange(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastTowerClicked?.ToggleVisualRange(false);
        }
    }
}