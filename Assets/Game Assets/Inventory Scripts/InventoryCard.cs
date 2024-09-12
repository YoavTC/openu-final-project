using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardOutlineImage;
    [SerializeField] private TMP_Text cardNameDisplay;
    [SerializeField] private TMP_Text cardLevelDisplay;
    [SerializeField] private Slider cardLevelProgressSlider;
    
    public void Display(BaseItemScriptableObject item)
    {
        cardImage.sprite = item.cardSprite;
        cardImage.color = item.isUnlocked ? Color.white : Color.gray;
        cardOutlineImage.color = RarityColours.GetRarityColour(item.itemRarity);
        cardNameDisplay.text = item.itemName;
        cardLevelDisplay.text = item.itemLevel.ToString();
        cardLevelProgressSlider.value = item.itemLevelProgression;
    }
}