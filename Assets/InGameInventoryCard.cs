using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//For MVP playtesting, delete later
public class InGameInventoryCard : MonoBehaviour
{
    public TowerSettings towerSettings;
    [SerializeField] private int cost;
    [SerializeField] private TMP_Text damageDisplay;
    [SerializeField] private TMP_Text costDisplay;
    
    void Start()
    {
        damageDisplay.text = towerSettings.damage.ToString();
        costDisplay.text = cost.ToString();
    }
}
