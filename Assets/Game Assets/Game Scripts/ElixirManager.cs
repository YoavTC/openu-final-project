using System;
using System.Collections;
using System.Collections.Generic;
using External_Packages;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ElixirManager : Singleton<ElixirManager>
{
    private int currentElixir;

    [Header("Components")] 
    [SerializeField] private Slider elixirBarSlider;
    [SerializeField] private TMP_Text elixirBarAmountDisplay;

    [Header("Settings")]
    [SerializeField] private int defaultIncreaseAmount;
    [SerializeField] private float increaseCooldown;
    [SerializeField] [ReadOnly] private float timeElapsed;

    [Header("Events")] 
    public UnityEvent<int> ElixirCountChangeEvent;
    
    #region Inspector Tools
    [Button]
    public void DecreaseTest()
    {
        DecreaseElixir(10);
    }
    
    [Button]
    public void IncreaseTest()
    {
        IncreaseElixir(10);
    }
    #endregion
    
    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= increaseCooldown)
        {
            timeElapsed = 0f;
            IncreaseElixir(defaultIncreaseAmount);
        }
    }
    
    //Dynamic Unity event listeners
    public void DecreaseElixir(Enemy enemy) => DecreaseElixir(enemy.enemySettings.damage);
    public void IncreaseElixir(Enemy enemy) => DecreaseElixir(enemy.enemySettings.reward);

    //Regular Unity event listeners
    private void IncreaseElixir(int amount) => UpdateElixirCount(Mathf.Clamp(currentElixir + amount, 0, 100));
    private void DecreaseElixir(int amount) => UpdateElixirCount(Mathf.Clamp(currentElixir - amount, 0, 100));
    
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

    private void UpdateElixirCount(int newCount)
    {
        currentElixir = newCount;
        ElixirCountChangeEvent?.Invoke(currentElixir);
        UpdateElixirBarUI();
    }
    
    private void UpdateElixirBarUI()
    {
        elixirBarSlider.value = currentElixir;
        elixirBarAmountDisplay.text = currentElixir.ToString();
    }
}
