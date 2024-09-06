using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HubCameraController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    
    public void OnDrag(PointerEventData eventData)
    {
        float difference = eventData.pressPosition.x - eventData.position.x;
        transform.position = panelLocation - new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        panelLocation = transform.position;
    }
}
