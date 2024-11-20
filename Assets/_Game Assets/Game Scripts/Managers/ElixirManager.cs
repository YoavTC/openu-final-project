using External_Packages;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ElixirManager : Singleton<ElixirManager>
{
    private float currentElixir;

    [Header("Components")] 
    [SerializeField] private Slider elixirBarSlider;
    [SerializeField] private TMP_Text elixirBarAmountDisplay;

    [Header("Settings")]
    [SerializeField] private float defaultIncreaseAmount;
    [SerializeField] private bool isActive = true;

    [Header("Events")] 
    public UnityEvent<float> ElixirCountChangeEvent;
    
    #region Inspector Tools
    [Button] public void DecreaseElixir() => DecreaseElixir(10); [Button]
    public void IncreaseElixir() => IncreaseElixir(10);
    #endregion
    
    private void Update()
    {
        if (isActive) IncreaseElixir(defaultIncreaseAmount * Time.deltaTime);
    }
    
    //Dynamic Unity event listeners
    public void DecreaseElixir(Enemy enemy) => DecreaseElixir(enemy.enemySettings.damage);
    public void IncreaseElixir(Enemy enemy) => IncreaseElixir(enemy.enemySettings.reward);

    //Regular Unity event listeners
    public void IncreaseElixir(float amount) => UpdateElixirCount(Mathf.Clamp(currentElixir + amount, 0f, 100f));
    private void DecreaseElixir(float amount) => UpdateElixirCount(Mathf.Clamp(currentElixir - amount, 0f, 100f));
    
    private bool CanAffordOperation(int amount) => currentElixir - amount > 0;
    public bool TryAffordOperation(int amount)
    {
        bool result = CanAffordOperation(amount);
        if (result)
        {
            DecreaseElixir(amount);
            return true;
        }

        return false;
    }
    

    private void UpdateElixirCount(float newCount)
    {
        currentElixir = newCount;
        ElixirCountChangeEvent?.Invoke(currentElixir);
        UpdateElixirBarUI();
        
        if (newCount == 0)
        {
            // SceneManager.LoadScene(0);
            ded?.Invoke();
        }
    }

    public UnityEvent ded;
    
    private void UpdateElixirBarUI()
    {
        elixirBarSlider.value = currentElixir;
        elixirBarAmountDisplay.text = currentElixir.ToString("F0");
    }

    public void SetActiveState(bool state)
    {
        isActive = state;
    }
}
