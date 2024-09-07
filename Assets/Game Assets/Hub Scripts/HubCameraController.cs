using System.Collections.Generic;
using DG.Tweening;
using External_Packages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HubCameraController : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    [Header("Swipe Settings")]
    [SerializeField] private float swipeTransitionDuration;
    [SerializeField] private Ease swipeTransitionEaseType;
    [SerializeField] private int initialPanel;
    [SerializeField] private float minDragDifference;
    [SerializeField] private float swipeEasing;

    private List<RectTransform> hubPanels = new List<RectTransform>();
    private RectTransform panelsContainer;

    private Vector3 panelLocation;
    private Vector2 startDragLocation;
    private float panelWidth;

    private void Start()
    {
        panelsContainer = GetRectTransformInFirstChild(transform);
        panelWidth = GetRectTransformInFirstChild(panelsContainer).rect.width;
        InitializeHubPanels();
        panelLocation = Vector3.zero;
    }

    private void InitializeHubPanels()
    {
        hubPanels.Clear();
        for (int i = 0; i < panelsContainer.childCount; i++)
        {
            RectTransform panel = panelsContainer.GetChild(i).GetComponent<RectTransform>();
            hubPanels.Add(panel);

            TMP_Text textDisplay = panel.GetChild(0).GetComponent<TMP_Text>();
            textDisplay.text = string.Format(textDisplay.text, i, panelWidth);

            panel.anchoredPosition = i == 0 ? Vector2.zero : hubPanels[i - 1].anchoredPosition + new Vector2(hubPanels[i - 1].rect.width, 0);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragLocation = eventData.pressPosition;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        float dragDifference = eventData.pressPosition.x - eventData.position.x;
        if (Mathf.Abs(dragDifference) > minDragDifference)
        {
            panelsContainer.anchoredPosition = panelLocation - new Vector3(dragDifference, 0, 0);
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        float swipePercentage = GetSwipePercentage(startDragLocation, eventData.position);
        float offset = (Mathf.Abs(swipePercentage) > swipeEasing && Mathf.Abs(swipePercentage) < 50f) ? panelWidth * Mathf.Sign(swipePercentage) : 0f;

        float closestPositionX = GetClosestPanelPosition(panelsContainer.anchoredPosition.x, offset);

        panelsContainer.DOAnchorPosX(closestPositionX, swipeTransitionDuration).SetEase(swipeTransitionEaseType);
        panelLocation = new Vector3(closestPositionX, 0, 0);
    }
    
    #region Helper Functions
    private RectTransform GetRectTransformInFirstChild(Transform parent)
    {
        return parent.GetChild(0).GetComponent<RectTransform>();
    }

    private float GetSwipePercentage(Vector2 start, Vector2 end)
    {
        return (start.x - end.x) / panelWidth * 100;
    }

    private float GetClosestPanelPosition(float currentPosition, float offset)
    {
        float closestPosition = Mathf.Round((currentPosition + offset) / panelWidth) * panelWidth;
        return Mathf.Clamp(closestPosition, -1 * (hubPanels.Count - 1) * panelWidth, 0);
    }
    #endregion
}
