using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.current;

        float maxHealth = transform.root.GetComponent<HealthBase>().maxHealth;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
        
        canvas.enabled = false;
    }

    public void OnHealthChangeEventListener(float newHealth)
    {
        if (!canvas.enabled) canvas.enabled = true;
        healthBarSlider.value = newHealth;
    }
}
