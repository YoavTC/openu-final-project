using UnityEngine;
using UnityEngine.UI;

public class AudioIconSliderChanger : MonoBehaviour
{
    [SerializeField] private Sprite icon1, icon2;
    [SerializeField] private Image sliderIcon;

    private void Start()
    {
        OnSliderValueChanged(GetComponent<Slider>().value);
    }

    public void OnSliderValueChanged(float value)
    {
        // Adjust slider icon
        if (value > 0) sliderIcon.sprite = icon1;
        else sliderIcon.sprite = icon2;
        
        // TODO: Play SFX
        // TODO: Save value as PlayerPref
    }
}
