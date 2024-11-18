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
    private GameObject draggedCard;

    private TowerSettings draggedCardTowerSettings;
    private TowerBase draggedTowerDefault;
    
    private Camera mainCamera;
    private Vector3 beginDragPoint;
    private Vector3 currentDragPoint;

    private InGameInventoryCard[] cards;
    private bool isLooping;
    // private bool invalidCardSelected;

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
    }

    [SerializeField] private int currentDraggerID;

    #region Card Dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentDraggerID == 0) currentDraggerID = eventData.pointerId;
        if (currentDraggerID != eventData.pointerId) return;
        
        beginDragPoint = eventData.position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var VARIABLE in results)
        {
            Debug.Log(VARIABLE.gameObject.name);
        }

        GameObject card = results.FirstOrDefault(a => a.gameObject.CompareTag("InventoryUICard")).gameObject;

        if (card != null)
        {
            draggedCard = Instantiate(draggedCardPrefab, transform);
            draggedCard.GetComponent<Image>().sprite = card.GetComponent<InGameInventoryCard>().towerSettings.sprite;
            draggedCardTowerSettings = card.GetComponent<InGameInventoryCard>().towerSettings;

            Vector2 placementPosition = ScreenToWorldPoint(eventData.position);
            draggedTowerDefault = Instantiate(towerPrefab,
                placementPosition
                , quaternion.identity,
                InSceneParentProvider.GetParent(SceneParentProviderType.TOWERS))
                .GetComponent<TowerBase>();
            
            draggedTowerDefault.towerSettings = draggedCardTowerSettings;
           
        } else
        {
            currentDraggerID = 0;
            // invalidCardSelected = true;
        }
        
        UpdatePlacementValidationUI(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentDraggerID != eventData.pointerId) return;
        
        currentDragPoint = eventData.position;
        if (draggedCard != null)
        {
            draggedTowerDefault.transform.position = ScreenToWorldPoint(currentDragPoint);
            draggedCard.transform.position = currentDragPoint;
        }
        
        UpdatePlacementValidationUI(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentDraggerID != eventData.pointerId) return;
        
        bool isValidPosition = IsValidPlacementPosition(eventData.position);
        
        if (isValidPosition && ElixirManager.Instance.TryAffordOperation(draggedCardTowerSettings.cost))
        {
            Vector2 placementPosition = ScreenToWorldPoint(eventData.position);
            TowerBase newTowerDefault = Instantiate(towerPrefab,
                placementPosition,
                quaternion.identity,
                InSceneParentProvider.GetParent(SceneParentProviderType.TOWERS))
                .GetComponent<TowerBase>();
            
            newTowerDefault.TowerPlaced(draggedCardTowerSettings);
        }
        
        Destroy(draggedTowerDefault.gameObject);
        
        if (draggedCard != null) Destroy(draggedCard);
        
        UpdatePlacementValidationUI(Color.clear);
        currentDraggerID = 0;
    }
    #endregion

    private void UpdatePlacementValidationUI(Vector2 pos)
    {
        UpdatePlacementValidationUI(IsValidPlacementPosition(pos) ? validColour : invalidColour);
    }
    
    private void UpdatePlacementValidationUI(Color colour)
    {
        if (lastColour == colour) return;
        
        lastColour = colour;
        validationImage.color = colour;

        if (draggedCard)
        {
            draggedCard.transform.GetChild(0).GetComponent<Image>().color = colour;
        }
    }

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

    //Returns the % of how much elixir there is out of the card's cost
    //1 = can afford
    //<1 can't afford
    private float GetAffordability(float currentElixir, TowerSettings towerSettings)
    {
        return (float) currentElixir / towerSettings.cost;
    }
    #endregion
}
