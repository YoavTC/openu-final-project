using System.Collections;
using System.Collections.Generic;
using External_Packages;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenURL : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void Open(string url)
    {
        Application.OpenURL(url);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0f, 0.7f, 1f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.05f);
    }
}
