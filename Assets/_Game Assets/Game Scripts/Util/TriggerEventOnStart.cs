using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnStart : MonoBehaviour
{
    public UnityEvent unityEvent;
    
    private void Start()
    {
        unityEvent?.Invoke();
    }
}