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
        ApplyVisual();
    }

    public void OnClickButton()
    {
        EnumerateSpeed();
    }

    private void EnumerateSpeed(float newSpeed = 0)
    {
        currentSpeedIndex++;
        if (currentSpeedIndex + 1 > speeds.Length) currentSpeedIndex = 0;
        
        Time.timeScale = newSpeed == 0 ? speeds[currentSpeedIndex] : newSpeed;

        ApplyVisual();
    }

    private void ApplyVisual()
    {
        clockImage?.DoEffect();
        displayText.text = Time.timeScale.ToString("F0") + "x";
    }

    public void ToggleInteractable(bool state, float newSpeed = -1f)
    {
        button.interactable = state;
        if (newSpeed != -1) EnumerateSpeed(newSpeed);
    }
}
