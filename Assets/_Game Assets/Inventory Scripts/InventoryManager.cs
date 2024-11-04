using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private BaseItemListScriptableObject baseItemsList = new BaseItemListScriptableObject();

    [SerializeField] private LayoutGroup itemsLayoutGroupContainer;
    [SerializeField] private InventoryCard itemListEntry;
    
    private void Start()
    {
        baseItemsList = FetchUnlockedItems();
        PopulateItemList();
    }

    private BaseItemListScriptableObject FetchUnlockedItems() => Resources.Load<BaseItemListScriptableObject>("Item Data/Unlocked Base Item List");

    private void PopulateItemList()
    {
        for (int i = 0; i < baseItemsList.collection.Length; i++)
        {
            if (baseItemsList.collection[i].isUnlocked)
            {
                InventoryCard newItemListEntry = Instantiate(itemListEntry, itemsLayoutGroupContainer.transform);
                newItemListEntry.Display(baseItemsList.collection[i]);
                //set card's colour (gray for locked, regular for unlocked)
            }
        }
    }
}
