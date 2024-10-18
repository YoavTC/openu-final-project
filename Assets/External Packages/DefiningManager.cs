using System.Collections;
using System.Collections.Generic;
using External_Packages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefiningManager : MonoBehaviour
{

    [Header("Components")] 
    [SerializeField] private Image displayImage;
    [SerializeField] private TMP_Text displayName;
    
    [SerializeField] private int currentTowerSettingsItemIndex;
    [SerializeField] private List<TowerSettings> towerSettingsList;

    private List<Transform> inputFields = new List<Transform>();
    
    void Start()
    {
        OnSkipButton(0);
    }

    //UGLY CODE! DO NOT LOOK! (shut up this won't be a part of the build anyway, it's just a tool for me)
    #region Ugly Code Region
    private void ClearInputFields()
    {
        inputFields.Clear();
        inputFields = HelperFunctions.GetChildren(transform);
        
        TowerSettings currentTower = towerSettingsList[currentTowerSettingsItemIndex];
        inputFields[0].GetComponent<TMP_InputField>().text = currentTower.health.ToString();
        inputFields[1].GetComponent<TMP_InputField>().text = currentTower.damage.ToString();
        inputFields[2].GetComponent<TMP_InputField>().text = currentTower.attackCooldown.ToString();
        inputFields[3].GetComponent<TMP_InputField>().text = currentTower.areaOfEffect.ToString();
        inputFields[4].GetComponent<TMP_InputField>().text = currentTower.cost.ToString();
    }

    public void OnSaveButton()
    {
        TowerSettings currentTower = towerSettingsList[currentTowerSettingsItemIndex];
        currentTower.health = float.Parse(inputFields[0].GetComponent<TMP_InputField>().text);
        currentTower.damage = float.Parse(inputFields[1].GetComponent<TMP_InputField>().text);
        currentTower.attackCooldown = float.Parse(inputFields[2].GetComponent<TMP_InputField>().text);
        currentTower.areaOfEffect = float.Parse(inputFields[3].GetComponent<TMP_InputField>().text);
        currentTower.cost = int.Parse(inputFields[4].GetComponent<TMP_InputField>().text);
    }
    #endregion
    
    public void OnSkipButton(int forward)
    {
        int futureIndex = currentTowerSettingsItemIndex + forward;
        if (futureIndex >= 0 && futureIndex < towerSettingsList.Count)
        {
            currentTowerSettingsItemIndex = futureIndex;
            ChangeCurrentItem();
        }
    }

    private void ChangeCurrentItem()
    {
        displayImage.sprite = towerSettingsList[currentTowerSettingsItemIndex].sprite;
        displayName.text = towerSettingsList[currentTowerSettingsItemIndex].name + "\n(" +
                           (currentTowerSettingsItemIndex + 1) + "/" + towerSettingsList.Count + ")";
        ClearInputFields();
    }
}
