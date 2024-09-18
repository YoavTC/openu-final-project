using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//For MVP playtesting, delete later
public class InGameInventoryCard : MonoBehaviour
{
    public TowerSettings towerSettings;
    [SerializeField] private TMP_Text damageDisplay;
    [SerializeField] private TMP_Text costDisplay;
    [SerializeField] private Image image;
    
    void Start()
    {
        image.sprite = towerSettings.sprite;
        damageDisplay.text = towerSettings.damage.ToString();
        costDisplay.text = towerSettings.cost.ToString();
    }
}
