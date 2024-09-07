using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HubCameraController : MonoBehaviour, IEndDragHandler, IDragHandler,  IBeginDragHandler
{
    [Header("Settings")]
    [SerializeField] private float swipeTransitionDuration;
    [SerializeField] private Ease swipeTransitionEaseType;
    [SerializeField] private int initialPanel;
    
    private List<RectTransform> hubPanels = new List<RectTransform>();
    private RectTransform panelsContainer;
    private float panelWidth;
    
    private void Start()
    {
        panelsContainer = transform.GetChild(0).GetComponent<RectTransform>();
        panelWidth = panelsContainer.GetChild(0).GetComponent<RectTransform>().rect.width;
        HubPanelsSetup();
     
        panelLocation = panelsContainer.position;
        panelLocation = new Vector3(0, 0, 0);
    }

    private void HubPanelsSetup()
    {
        //Add hub panels to list 
        hubPanels.Clear();
        for (int i = 0; i < panelsContainer.childCount; i++)
        {
            hubPanels.Add(panelsContainer.GetChild(i).GetComponent<RectTransform>());
            
            //Set text TODO: delete later
            TMP_Text textDisplay = panelsContainer.GetChild(i).GetChild(0).GetComponent<TMP_Text>();
            textDisplay.text = string.Format(textDisplay.text, i, panelWidth);
            
            if (i == 0)
            {
                hubPanels[i].anchoredPosition = Vector2.zero;
            } else {
                hubPanels[i].anchoredPosition = hubPanels[i - 1].anchoredPosition + new Vector2(hubPanels[i - 1].rect.width, 0);
            }
        }
    }

    [Button] private void SwipeLeft() {SwipePanel(-1);}
    [Button] private void SwipeRight() {SwipePanel(1);}
    
    private void SwipePanel(int direction)
    {
        panelsContainer.DOKill(true);
        panelsContainer.DOAnchorPosX(panelsContainer.anchoredPosition.x + (panelWidth * direction), swipeTransitionDuration).SetEase(swipeTransitionEaseType);
    }
    
    

    [SerializeField] private float minDragDifference;
    [SerializeField] private float swipeEasing;
    private Vector3 panelLocation;
    private Vector2 startDragLocation;
    
    public void OnEndDrag(PointerEventData eventData)
    {
        //With swipe easing (adjusting it so it doesn't have to be 50% of the way to the next panel)
        float offset = 0f;
        float swipePercentage = ((startDragLocation.x - eventData.position.x) / panelWidth) * 100;
        float absoluteSwipePercentage = Mathf.Abs(swipePercentage);
        if (absoluteSwipePercentage > swipeEasing && absoluteSwipePercentage < 50f)
        {
            offset = panelWidth * Mathf.Sign(swipePercentage);
        }
        //Without swipe easing
        float closestPositionX = Mathf.Round((panelsContainer.anchoredPosition.x + (offset) * Mathf.Sign(panelsContainer.anchoredPosition.x)) / panelWidth) * panelWidth;
        closestPositionX = Mathf.Clamp(closestPositionX,-1 * ((hubPanels.Count - 1) * panelWidth), 0);
        
        panelsContainer.DOAnchorPosX(closestPositionX, swipeTransitionDuration).SetEase(swipeTransitionEaseType);
        panelLocation = new Vector3(closestPositionX, 0, 0);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        float dragDifference = eventData.pressPosition.x - eventData.position.x;
        if (Mathf.Abs(dragDifference) > minDragDifference)
        {
            Debug.Log("Drag Direction:" + ((startDragLocation.x - eventData.position.x) / panelWidth) * 100);
            panelsContainer.anchoredPosition = panelLocation - new Vector3(dragDifference, 0, 0);
            Debug.DrawLine(eventData.pressPosition, eventData.position, Color.magenta);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragLocation = eventData.pressPosition;
    }
}
