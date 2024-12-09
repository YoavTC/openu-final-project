using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TriggerEventOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnHoverEnterUnityEvent;
    public UnityEvent OnHoverExitUnityEvent;
    
    public void OnPointerEnter(PointerEventData eventData) => OnHoverEnterUnityEvent?.Invoke();
    public void OnPointerExit(PointerEventData eventData) => OnHoverExitUnityEvent?.Invoke();
}