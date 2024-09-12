using UnityEngine;

[CreateAssetMenu(fileName = "Base Item List", menuName = "Base Items/Base Item List")]
public class BaseItemListScriptableObject : ScriptableObject
{
    public BaseItemScriptableObject[] collection;

    public BaseItemScriptableObject Get(int itemID)
    {
        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].itemID == itemID) return collection[i];
        }

        return null;
    }
}