using UnityEngine;

public static class ItemUtilityClass
{
    public static BaseItemListScriptableObject baseItemList;

    static ItemUtilityClass()
    {
        Init();
    }

    //Init
    private static void Init()
    {
        baseItemList = Resources.Load<BaseItemListScriptableObject>("Item Data/Base Item List");
    } 
    
    //Retrieve
    public static BaseItemScriptableObject GetBaseItem(int itemID)
    {
        return baseItemList.Get(itemID);
    }
    
    //Modify
    public static void Upgrade(BaseItemScriptableObject item)
    {
        
    }
}