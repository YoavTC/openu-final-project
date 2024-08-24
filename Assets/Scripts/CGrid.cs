using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CGrid
{
    private float cellSize;
    private int width, height;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public CGrid(int width, int height, float cellSize)
    {
        Debug.Log(string.Format("New grid created: w: {0}, h:{1}", width, height));
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y, true), 20, Color.white, TextAnchor.MiddleCenter);
                
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.magenta, 10);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.magenta, 10);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.magenta, 10);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.magenta, 10);
    }

    private Vector3 GetWorldPosition(int x, int y, bool center = false)
    {
        if (center) return (new Vector3(x, y) * cellSize) + new Vector3(cellSize, cellSize) * 0.5f;
        return new Vector3(x, y) * cellSize;
    }

    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x / cellSize), Mathf.FloorToInt(worldPosition.y / cellSize));
    }

    public void SetValue(int x, int y, int value)
    {
        if (isValidXY(x, y))
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = value.ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        Vector2Int gridCell = GetGridPosition(worldPosition);
        SetValue(gridCell.x, gridCell.y, value);
    }

    public int GetValue(int x, int y)
    {
        if (isValidXY(x, y))
        {
            return gridArray[x, y];
        }

        return -1;
    }
    
    public int GetValue(Vector3 worldPosition)
    {
        Vector2Int gridCell = GetGridPosition(worldPosition);
        if (isValidXY(gridCell.x, gridCell.y))
        {
            return gridArray[gridCell.x, gridCell.y];
        }

        return -1;
    }

    private bool isValidXY(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < width && y < height);
    }
}
