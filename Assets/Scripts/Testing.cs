using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private float cellSize;
    [SerializeField] private Vector2Int gridSize;

    private CGrid cGrid;
    
    void Start()
    {
        cGrid = new CGrid(gridSize.x, gridSize.y, cellSize);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
            cGrid.SetValue(mousePos, 1);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
            Debug.Log(cGrid.GetValue(mousePos));
        }
    }
}
