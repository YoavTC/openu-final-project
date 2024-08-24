using System;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    [SerializeField] private bool paintTile;
    [SerializeField] private Transform debugObject;
    [SerializeField] private Sprite tileSprite;

    public bool showTargetCell;
    
    private Tilemap tilemap;
    private Grid grid;
    private Tile tile;

    private void Start()
    {
        grid = GetComponent<Grid>();
        tilemap = grid.transform.GetChild(0).GetComponent<Tilemap>();
        
        tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = tileSprite;
    }

    void Update()
    {
        Vector3 centeredCellAtMouse = GetCenteredCellAtMouse();
        
        if (showTargetCell)
        {
            debugObject.position = centeredCellAtMouse;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Do raycast check
            Paint(centeredCellAtMouse);
        }
    }

    private void Paint(Vector3 targetPosition)
    {
        //Place object
    }

    private Vector3 GetCenteredCellAtMouse()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        Vector3Int cellPos = grid.WorldToCell(mousePosition);
        
        return cellPos + new Vector3(0.5f, 0.5f, 0);
    }
}
