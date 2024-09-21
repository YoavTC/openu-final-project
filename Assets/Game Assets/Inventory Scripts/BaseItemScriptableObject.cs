using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory/Base Item")]
public class BaseItemScriptableObject : ScriptableObject
{
    //Visual information
    [ShowAssetPreview] public Sprite cardSprite;
    
    //General information
    public int itemID;
    public string itemName;
    public bool isUnlocked;
    
    //Level information
    public Rarity itemRarity;
    public int itemLevel;
    public float itemLevelProgression;
    
    //Stats information
    public TowerSettings towerSettings;
}
