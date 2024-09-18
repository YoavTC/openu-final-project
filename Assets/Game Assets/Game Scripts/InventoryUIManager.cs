using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Drawer Components")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform showHideButton;

    [Header("Drawer Settings")]
    [SerializeField] private float buttonCooldown;
    [SerializeField] private Ease easeType;
    [SerializeField] private float inTime;
    [SerializeField] private float outTime;

    [SerializeField] private float inY;
    [SerializeField] private float outY;

    private bool isIn;
    private float elapsedTime;

    private void Start()
    {
        isIn = true;
        inY = rectTransform.anchoredPosition.y;

        mainCamera = Camera.main;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime; 
        
        Debug.DrawLine(beginDragPoint, currentDragPoint, Color.green);
    }

    #region Inventory Drawer
    public void OnShowHideButtonPress()
    {
        if (elapsedTime >= buttonCooldown)
        {
            elapsedTime = 0f;
            
            rectTransform.DOKill();
            if (isIn) HideItems();
            else ShowItems();

            isIn = !isIn;
            showHideButton.localScale *= -1;
        }
    }

    private void ShowItems() => rectTransform.DOAnchorPosY(inY, inTime).SetEase(easeType);
    private void HideItems() => rectTransform.DOAnchorPosY(outY, outTime).SetEase(easeType);
    #endregion

    [Header("Card Dragging Components")]
    [SerializeField] private GameObject draggedCardPrefab;
    [SerializeField] private GameObject towerPrefab;
    private GameObject draggedCard;
    
    private Camera mainCamera;
    private Vector3 beginDragPoint;
    private Vector3 currentDragPoint;
    
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
        draggedCard = Instantiate(draggedCardPrefab, transform);
        Debug.Log("Found: " + card);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentDragPoint = eventData.position;
        if (draggedCard != null) draggedCard.transform.position = currentDragPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 placementPosition = mainCamera.ScreenToWorldPoint(eventData.position);
        if (IsValidPlacementPosition(placementPosition))
        {
            Instantiate(towerPrefab, placementPosition, quaternion.identity);
        }
        
        if (draggedCard != null) Destroy(draggedCard);
    }

    private bool IsValidPlacementPosition(Vector2 pos)
    {
        return true;
    }
}
