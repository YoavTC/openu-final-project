using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Card Dragging Components")] 
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private GameObject draggedCardPrefab;
    [SerializeField] private GameObject towerPrefab;
    // private GameObject draggedCard;

    // private TowerSettings draggedCardTowerSettings;
    private TowerBase draggedTower;
    
    private Camera mainCamera;
    private Vector3 beginDragPoint;
    private Vector3 currentDragPoint;

    private InGameInventoryCard[] cards;
    private bool isLooping;
    private int currentDraggerID;

    [Header("Position Validator")] 
    [SerializeField] private Color invalidColour;
    [SerializeField] private Color validColour;
    [SerializeField] private float colourTransitionDuration;
    private Image validationImage;
    private Color lastColour;
    private RectTransform rectTransform;
    
    private void Start()
    {
        isLooping = false;

        validationImage = GetComponent<Image>();
        UpdatePlacementValidationUI(validColour);

        rectTransform = GetComponent<RectTransform>();
        
        mainCamera = Camera.main;
        cards = new InGameInventoryCard[cardsContainer.childCount];
        for (int i = 0; i < cardsContainer.childCount; i++)
        {
            if (cardsContainer.GetChild(i).TryGetComponent(out InGameInventoryCard card))
            {
                cards[i] = card;
            }
        }
    }
    
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
            return;
        }
        
        draggedTower = Instantiate(towerPrefab,
                ScreenToWorldPoint(eventData.position),
                quaternion.identity,
                InSceneParentProvider.GetParent(SceneParentProviderType.TOWERS))
            .GetComponent<TowerBase>();
            
        
        //draggedTower.towerSettings = card.GetComponent<InGameInventoryCard>().towerSettings;
        draggedTower.InitializeComponents(card.GetComponent<InGameInventoryCard>().towerSettings);
        
        // UpdatePlacementValidationUI(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentDraggerID != eventData.pointerId) return;
        
        currentDragPoint = eventData.position;
        if (draggedTower != null)
        {
            draggedTower.transform.position = ScreenToWorldPoint(currentDragPoint);
        }
        
        // UpdatePlacementValidationUI(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentDraggerID != eventData.pointerId) return;
        
        if (IsValidPlacementPosition(eventData.position) && ElixirManager.Instance.TryAffordOperation(draggedTower.towerSettings.cost))
        {
            Vector2 placementPosition = ScreenToWorldPoint(eventData.position);
            TowerBase newTower = Instantiate(towerPrefab,
                placementPosition,
                quaternion.identity,
                InSceneParentProvider.GetParent(SceneParentProviderType.TOWERS))
                .GetComponent<TowerBase>();
            
            newTower.InitializeComponents(draggedTower.towerSettings);
            newTower.OnTowerPlaced();
        }
        
        Destroy(draggedTower.gameObject);
        
        UpdatePlacementValidationUI(Color.clear);
        currentDraggerID = 0;
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
        validationImage.color = colour;

        if (draggedTower)
        {
            draggedTower.transform.GetChild(2).GetComponent<SpriteRenderer>().color = colour;
        }
    }
    #endregion

    #region Elixir Affordability
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
}
