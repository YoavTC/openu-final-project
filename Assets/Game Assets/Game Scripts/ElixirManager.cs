using System;
using System.Collections;
using System.Collections.Generic;
using External_Packages;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElixirManager : Singleton<ElixirManager>
{
    private int _currentElixir;
    public int currentElixir 
    {
        set
        {
            UpdateElixirBarUI();
            _currentElixir = value;
        }
        get => _currentElixir;
    }

    [Header("Components")] 
    [SerializeField] private Slider elixirBarSlider;
    [SerializeField] private TMP_Text elixirBarAmountDisplay;

    [Header("Settings")]
    [SerializeField] private int defaultIncreaseAmount;
    [SerializeField] private float increaseCooldown;
    [SerializeField] [ReadOnly] private float timeElapsed;
    
    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= increaseCooldown)
        {
            timeElapsed = 0f;
            IncreaseElixir(defaultIncreaseAmount);
        }
    }

    private void UpdateElixirBarUI()
    {
        elixirBarSlider.value = currentElixir;
        elixirBarAmountDisplay.text = currentElixir.ToString();
    }

    [Button]
    public void DecreaseTest()
    {
        DecreaseElixir(10);
    }
    
    [Button]
    public void IncreaseText()
    {
        IncreaseElixir(10);
    }

    public void IncreaseElixir(int amount) => currentElixir = Mathf.Clamp(currentElixir + amount, 0, 100);
    public void DecreaseElixir(int amount) => currentElixir = Mathf.Clamp(currentElixir - amount, 0, 100);
    public bool CanAffordOperation(int amount) => currentElixir - amount > 0;

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
}
