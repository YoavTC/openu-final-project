using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using External_Packages;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Card Dragging Components")] 
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private InGameInventoryCard inGameCardPrefab;
    
    [Header("Position Validator")] 
    [SerializeField] private Color invalidColour;
    [SerializeField] private Color validColour;
    [SerializeField] private float colourTransitionDuration;
    private Color lastColour;
    private RectTransform rectTransform;

    [SerializeField] [Layer] int towersLayerMask; 
    [SerializeField] private float placementValidationRadius;

    [Header("In Transition Settings")] 
    [SerializeField] private Vector2 inOutPositions;
    [SerializeField] private float transitionDuration;
    
    private TowerBase draggedTower;
    private int currentDraggerID;
    
    private Vector3 beginDragPoint;
    private Vector3 currentDragPoint;
    
    private Camera mainCamera;
    private InGameInventoryCard[] cards;
    
    private void Start()
    {
        RetrieveCardsFromInventory();
        TransitionOut();
    }

    #region Initialization
    public void InitializeInventory(LevelBuildSO levelBuild)
    {
        HelperFunctions.DestroyChildren(cardsContainer);
        foreach (TowerSettings towerSettings in levelBuild.towerBases)
        {
            Instantiate(inGameCardPrefab, cardsContainer)
                .towerSettings = towerSettings;
        }
        
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;

        isLooping = false;
        
        UpdatePlacementValidationUI(validColour);
    }
    
    private void RetrieveCardsFromInventory()
    {
        cards = new InGameInventoryCard[cardsContainer.childCount];
        for (int i = 0; i < cardsContainer.childCount; i++)
        {
            if (cardsContainer.GetChild(i).TryGetComponent(out InGameInventoryCard card))
            {
                cards[i] = card;
            }
        }
    }
    #endregion
    
    private void Update()
    {
        DebugPainter.DrawArrow(beginDragPoint, currentDragPoint, Color.green);
        if (draggedTower != null) UpdatePlacementValidationUI(WorldToScreenPoint(draggedTower.transform.position));
    }

    #region Card Dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentDraggerID == 0) currentDraggerID = eventData.pointerId;
        if (currentDraggerID != eventData.pointerId) return;
        
        beginDragPoint = eventData.position;
        
        // Raycast to find selected card
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        GameObject card = results.FirstOrDefault(a => a.gameObject.CompareTag("InventoryUICard")).gameObject;

        if (card == null)
        {
            currentDraggerID = 0;
        } 
        else
        {
            InstantiateDraggedTower(card.GetComponent<InGameInventoryCard>().towerSettings, eventData.position);
        }
    }

    private void InstantiateDraggedTower(TowerSettings towerSettings, Vector2 pos)
    {
        draggedTower = Instantiate(towerPrefab,
                ScreenToWorldPoint(pos),
                quaternion.identity,
                InSceneParentProvider.GetParent(SceneParentProviderType.TOWERS))
            .GetComponent<TowerBase>();
            
        
        draggedTower.InitializeComponents(towerSettings);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentDraggerID != eventData.pointerId) return;
        
        currentDragPoint = eventData.position;
        if (draggedTower != null)
        {
            draggedTower.transform.position = ScreenToWorldPoint(currentDragPoint);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentDraggerID != eventData.pointerId) return;
        
        if (CanPlaceTower(eventData.position, draggedTower.towerSettings.cost))
        {
            TowerBase newTower = Instantiate(towerPrefab, 
                ScreenToWorldPoint(eventData.position),
                quaternion.identity,
                InSceneParentProvider.GetParent(SceneParentProviderType.TOWERS))
            .GetComponent<TowerBase>();
            
            newTower.InitializeComponents(draggedTower.towerSettings);
            newTower.OnTowerPlaced();
        }
        
        ClearDragCache();
    }

    private void ClearDragCache()
    {
        Destroy(draggedTower.gameObject);
        
        UpdatePlacementValidationUI(Color.clear);
        currentDraggerID = 0;
    }

    private bool CanPlaceTower(Vector2 pos, int cost)
    {
        return IsValidPlacementPosition(pos) && ElixirManager.Instance.TryAffordOperation(cost);
    }
    #endregion

    #region UI
    private void UpdatePlacementValidationUI(Vector2 pos)
    {
        UpdatePlacementValidationUI(IsValidPlacementPosition(pos) ? validColour : invalidColour);
    }
    
    private void UpdatePlacementValidationUI(Color colour)
    {
        if (lastColour == colour) return;
        
        lastColour = colour;
        if (draggedTower)
        {
            draggedTower.transform.GetChild(2).GetComponent<SpriteRenderer>().color = colour;
        }
    }
    #endregion

    #region Elixir Affordability
    private bool isLooping;
    
    //Dynamic Unity event listener
    public void OnElixirCountChangeEventListener(float newCount)
    {
        StartCoroutine(UpdateAffordabilitySlidersCoroutine(newCount));
    }

    private IEnumerator UpdateAffordabilitySlidersCoroutine(float newCount)
    {
        yield return new WaitUntil(() => !isLooping);
        UpdateAffordabilitySliders(newCount);
    }

    private void UpdateAffordabilitySliders(float newCount)
    {
        isLooping = true;
        for (int i = 0; i < cards.Length; i++)
        {
            Slider cardAffordabilitySlider = cards[i].affordabilitySlider;
            cardAffordabilitySlider.value = 1f - GetAffordability(newCount, cards[i].GetComponent<InGameInventoryCard>().towerSettings);
        }
        isLooping = false;
    }
    #endregion
    
    #region Utility
    //TODO: Change after level generation algorithm is implemented
    private bool IsValidPlacementPosition(Vector2 pos)
    {
        Vector2 localPoint;
        
        // Convert screen position to local point in the target RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, 
            pos, 
            null, 
            out localPoint
        );

        // var a = Physics2D.OverlapCircleAll(ScreenToWorldPoint(pos), placementValidationRadius, LayerMask.GetMask("Towers"));
        var a = Physics2D.OverlapCircle(ScreenToWorldPoint(pos), placementValidationRadius, LayerMask.GetMask("Towers", "Spline"));
        // foreach (var VARIABLE in a)
        // {
        //     Debug.Log(VARIABLE);
        // }
        if (a) return false;
        
        return !rectTransform.rect.Contains(localPoint);
    }
    
    private Vector2 ScreenToWorldPoint(Vector2 point) => mainCamera.ScreenToWorldPoint(point);
    private Vector2 WorldToScreenPoint(Vector2 point) => mainCamera.WorldToScreenPoint(point);

    //Returns the % of how much elixir there is out of the card's cost
    //1 = can afford
    //<1 can't afford
    private float GetAffordability(float currentElixir, TowerSettings towerSettings)
    {
        return (float) currentElixir / towerSettings.cost;
    }
    #endregion

    #region Transition

    public void TransitionIn()
    {
        rectTransform.DOAnchorPosY(inOutPositions.x, transitionDuration).SetUpdate(true);
    }

    private void TransitionOut()
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, inOutPositions.y);
    }

    #endregion
}