using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HubCameraController : MonoBehaviour
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
    }

    private void HubPanelsSetup()
    {
        //Add hub panels to list 
        hubPanels.Clear();
        for (int i = 0; i < panelsContainer.childCount; i++)
        {
            hubPanels.Add(panelsContainer.GetChild(i).GetComponent<RectTransform>());
            
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
}
