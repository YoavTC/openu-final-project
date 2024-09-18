using System;
using DG.Tweening;
using External_Packages;
using TMPro;
using UnityEngine;

public class TextPopupManager : Singleton<TextPopupManager>
{
    [SerializeField] private TMP_Text popupPrefab;
    
    public void DisplayPopup(string text, Vector2 position, float size, Color colour, float lifetime, Action callback = null)
    {
        TMP_Text newPopup = Instantiate(popupPrefab, transform);
        newPopup.transform.position = position;
        newPopup.fontSize = size;
        newPopup.text = text;
        newPopup.color = colour;

        newPopup.DOFade(1, 0);
        newPopup.DOFade(0, 0.2f).SetDelay(lifetime).OnComplete(() =>
        {
            callback?.Invoke();
            Destroy(newPopup.gameObject);
        });
    }
}
