using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TimescaleController : MonoBehaviour
{
    [SerializeField] private int[] speeds;
    [SerializeField] private int currentSpeedIndex;
    
    [Header("Components")]
    [SerializeField] private ScaleEffect clockImage;
    [SerializeField] private TMP_Text displayText;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        // ApplyVisual();
    }

    public void OnClickButton()
    {
        // EnumerateSpeed();
        if (Time.timeScale != 0f) EnumerateSpeed();
    }

    private void EnumerateSpeed()
    {
        currentSpeedIndex++;
        if (currentSpeedIndex + 1 > speeds.Length) currentSpeedIndex = 0;

        Time.timeScale = speeds[currentSpeedIndex];

        ApplyVisual();
    }

    private void ApplyVisual()
    {
        clockImage?.DoEffect();
        displayText.text = Time.timeScale.ToString("F0") + "x";
    }
}
