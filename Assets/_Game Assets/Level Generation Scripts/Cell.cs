using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = transform.position.x.ToString("F0") + "," + transform.position.y.ToString("F0");
        mainCam = Camera.main;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColour = spriteRenderer.color;
    }

    private Camera mainCam;
    private SpriteRenderer spriteRenderer;
    private Color originalColour;
    
    private void Update()
    {
        if (GetFloorV2(mainCam.ScreenToWorldPoint(Input.mousePosition)) == (Vector2)transform.position)
        {
            Debug.Log((Vector2) transform.position);
            GetComponent<TMP_Text>().color = Color.black;
        } else GetComponent<TMP_Text>().color = Color.white;

    }

    private Vector2 GetFloorV2(Vector2 vector2)
    { 
        return new Vector2(Mathf.Floor(vector2.x), Mathf.Floor(vector2.y));

    }

    public void ColourCell(Color colour)
    {
        spriteRenderer.color = colour;
    }
}
