using TMPro;
using UnityEngine;
using UnityEngine.UI;

//For MVP playtesting, delete later
public class InGameInventoryCard : MonoBehaviour
{
    public TowerSettings towerSettings;
    public Slider affordabilitySlider;
    [SerializeField] private TMP_Text damageDisplay;
    [SerializeField] private TMP_Text costDisplay;
    [SerializeField] private Image image;
    
    void Start()
    {
        image.sprite = towerSettings.sprite;
        float dps = 0f;
        if (towerSettings.damage > 0)
        {
            dps = towerSettings.damage / towerSettings.attackCooldown;
        }
        
        damageDisplay.text = dps.ToString("0.##");
        costDisplay.text = towerSettings.cost.ToString();
    }
}
