using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnEnable : MonoBehaviour
{
    public UnityEvent unityEvent;
    
    private void OnEnable()
    {
        unityEvent?.Invoke();
    }
}