using System.Collections;
using System.Collections.Generic;
using External_Packages;
using NaughtyAttributes;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private float stepDelay;
    [SerializeField] private bool generatorState;

    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private Transform generationParent;

    [SerializeField] private int gridWidth, gridHeight;
    private Cell[,] levelGrid;

    [Button]
    void Start()
    {
        InitializeGrid();
        
        DepopulateGrid();
        StartCoroutine(PopulateGrid());
        
        Cell startingCell = GetRandomCellOnWall();
        Cell endingCell = GetRandomCellOnWall();
    }

    #region Grid
    private void InitializeGrid()
    {
        levelGrid = new Cell[gridWidth, gridHeight];
    }
    
    private void DepopulateGrid()
    {
        HelperFunctions.DestroyChildren(generationParent);
    }
    
    private IEnumerator PopulateGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                yield return new WaitForSeconds(stepDelay);
                levelGrid[i, j] = Instantiate(gridCellPrefab, new Vector2(i, j), Quaternion.identity, generationParent)
                    .GetComponent<Cell>();
            }
        }
    }
    #endregion

    #region Utility
    private enum Wall { Top, Bottom, Left, Right }
    private Wall lastWall = Wall.Top; // Initialize with a default wall
    
    private Cell GetRandomCellOnWall()
    {
        Wall newWall;
        do
        {
            // Randomly select a wall, ensuring it's not the same as the last selected one
            newWall = (Wall)Random.Range(0, 4);
        } while (newWall == lastWall);

        lastWall = newWall; // Update the last wall used

        int x = 0, y = 0;

        // Determine coordinates based on the selected wall
        switch (newWall)
        {
            case Wall.Top:
                x = Random.Range(0, gridWidth);
                y = gridHeight - 1;
                break;

            case Wall.Bottom:
                x = Random.Range(0, gridWidth);
                y = 0;
                break;

            case Wall.Left:
                x = 0;
                y = Random.Range(0, gridHeight);
                break;

            case Wall.Right:
                x = gridWidth - 1;
                y = Random.Range(0, gridHeight);
                break;
        }
            
        levelGrid[x, y].ColourCell(Color.cyan);
        return levelGrid[x, y];
    }
    #endregion
}
