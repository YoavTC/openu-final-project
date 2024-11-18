using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionStat : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text display;

    public void InitializeComponents(Sprite sprite, string text)
    {
        display.text = text;
        image.sprite = sprite;
    }
}
