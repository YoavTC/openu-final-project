using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] [Scene] private string targetScene;
    [SerializeField] private Color hoverColour;
    [SerializeField] private Vector2 defaultOutlineWidth;
    [SerializeField] private Vector2 hoverOutlineWidth;

    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(targetScene);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.effectColor = hoverColour;
        outline.effectDistance = hoverOutlineWidth;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.effectColor = Color.black;
        outline.effectDistance = defaultOutlineWidth;
    }
}
