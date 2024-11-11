using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    private Canvas canvas;
    private HealthBase healthBase;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        
        healthBase = GetComponentInParent<HealthBase>();
        healthBase.OnHealthInitializedEvent.AddListener(InitializeHealthBar);
    }

    private void InitializeHealthBar()
    {
        float maxHealth = healthBase.maxHealth;
        
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
        
        // if (healthBase.GetComponent<Enemy>()) canvas.enabled = false;
    }

    public void OnHealthChangeEventListener(float newHealth)
    {
        if (!canvas.enabled) canvas.enabled = true;
        healthBarSlider.value = newHealth;
    }
}
