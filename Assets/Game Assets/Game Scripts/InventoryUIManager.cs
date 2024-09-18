using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform showHideButton;

    [Header("Settings")]
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
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime; 
    } 
    
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
    
}
