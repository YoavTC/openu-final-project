using System;
using System.Reflection;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using External_Packages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : Singleton<SelectionManager>
{
    #region Selection Handling
    private TowerBase lastTowerClicked;
    
    public void OnSelectableItemClicked(TowerBase tower)
    {
        UpdatePanelVisibility(true);
                
        lastTowerClicked?.ToggleVisualRange(false);
        lastTowerClicked = tower;
        lastTowerClicked.ToggleVisualRange(true);
        
        RemoveSelectedInformation();
        SetSelectedInformation(lastTowerClicked.towerSettings);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDeselect();
        }
    }

    private void OnDeselect()
    {
        lastTowerClicked?.ToggleVisualRange(false);
        RemoveSelectedInformation();
        UpdatePanelVisibility(false);
    }
    #endregion
    
    #region Information Handling
    [Header("Components")] 
    [SerializeField] private TMP_Text selectedDisplay;
    [SerializeField] private Image selectedImage;
    [SerializeField] private TMP_Text selectedDescription;
    [SerializeField] private Transform statsContainer;
    [SerializeField] private SelectionStat selectionStatPrefab;

    [Header("Sprites")] [SerializeField]
    private SerializedDictionary<string, Sprite> statSpritesDictionary = new SerializedDictionary<string, Sprite>();

    private void SetSelectedInformation(TowerSettings towerSettings)
    {
        selectedDisplay.text = towerSettings.name;
        selectedDescription.text = towerSettings.description;
        selectedImage.sprite = towerSettings.sprite;
        
        DepopulateStats();
        PopulateStats(towerSettings);
    }

    private void RemoveSelectedInformation()
    {
        selectedDisplay.text = "";
        selectedDescription.text = "";
        selectedImage.sprite = null;
    }
    
    private void PopulateStats(TowerSettings towerSettings)
    {
        FieldInfo[] fields = typeof(TowerSettings).GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            System.Object fieldValue = field.GetValue(towerSettings);
            if (fieldValue != null && statSpritesDictionary.ContainsKey(field.Name))
            {
                Sprite sprite = statSpritesDictionary[field.Name];
                string fieldValueName = fieldValue.ToString();

                if (fieldValue is ModifierEffect modifierEffect)
                {
                    fieldValueName = modifierEffect.type.ToString();
                }

                Instantiate(selectionStatPrefab, transform.position, Quaternion.identity, statsContainer)
                    .InitializeComponents(sprite, fieldValueName);
            }
        }
    }

    private void DepopulateStats()
    {
        HelperFunctions.DestroyChildren(statsContainer);
    }
    #endregion

    [Header("Appearance Settings")] 
    [SerializeField] private float slideDuration;
    [SerializeField] private Vector2 inOutPanelPositions;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = transform.GetComponent<RectTransform>();
    }

    private void UpdatePanelVisibility(bool visible)
    {
        rectTransform.DOKill(true);
        rectTransform.DOAnchorPosX(visible ? inOutPanelPositions.x : inOutPanelPositions.y, slideDuration);
    }
}