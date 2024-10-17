using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using External_Packages;
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
    private Tower draggedTower;
    
    private Camera mainCamera;
    private Vector3 beginDragPoint;
    private Vector3 currentDragPoint;

    private InGameInventoryCard[] cards;
    private bool isLooping;
    
    private void Start()
    {
        isLooping = false;
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
        Debug.DrawLine(beginDragPoint, currentDragPoint, Color.green);
    }

    #region Card Dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragPoint = eventData.position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var VARIABLE in results)
        {
            Debug.Log(VARIABLE.gameObject.name);
        }

        GameObject card = results.First(a => a.gameObject.CompareTag("InventoryUICard")).gameObject;
        Debug.Log("Found: " + card);
        
        draggedCard = Instantiate(draggedCardPrefab, transform);
        draggedCard.GetComponent<Image>().sprite = card.GetComponent<InGameInventoryCard>().towerSettings.sprite;
        draggedCardTowerSettings = card.GetComponent<InGameInventoryCard>().towerSettings;

        Vector2 placementPosition = ScreenToWorldPoint(eventData.position);
        draggedTower = Instantiate(towerPrefab, placementPosition, quaternion.identity).GetComponent<Tower>();
        draggedTower.towerSettings = draggedCardTowerSettings;
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentDragPoint = eventData.position;
        if (draggedCard != null)
        {
            draggedTower.transform.position = ScreenToWorldPoint(currentDragPoint);
            draggedCard.transform.position = currentDragPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ElixirManager.Instance.TryAffordOperation(draggedCardTowerSettings.cost))
        {
            Vector2 placementPosition = ScreenToWorldPoint(eventData.position);
            if (IsValidPlacementPosition(placementPosition))
            {
                Destroy(draggedTower.gameObject);
                Tower newTower = Instantiate(towerPrefab, placementPosition, quaternion.identity).GetComponent<Tower>();
                newTower.towerSettings = draggedCardTowerSettings;
                newTower.OnTowerPlacedEventListener();
                newTower.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = draggedCardTowerSettings.sprite;
            }
        }
        else
        {
            Destroy(draggedTower.gameObject);
            Debug.Log("Cant afford this tower!");
        }
        
        if (draggedCard != null) Destroy(draggedCard);
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
    private bool IsValidPlacementPosition(Vector2 pos) => true;
    
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
