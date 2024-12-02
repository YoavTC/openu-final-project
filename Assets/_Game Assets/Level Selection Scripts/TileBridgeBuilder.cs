using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using External_Packages;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileBridgeBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private SerializedDictionary<int, List<Transform>> bridgesTiles;

    private void Start()
    {
        HideTiles();
    }

    private void HideTiles()
    {
        tilemap.enabled = false;
        
        foreach (var bridgeTileSet in bridgesTiles)
        {
            foreach (Transform bridgeTile in bridgeTileSet.Value)
            {
                bridgeTile.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    [SerializeField] private float animationDuration;
    [SerializeField] private Ease animationEase;
    [SerializeField] private float animationYOffset = 1f;
    private float totalDelay;

    public IEnumerator RevealBridge(int bridgeIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (Transform bridgeTile in bridgesTiles[bridgeIndex])
        {
            totalDelay += animationDuration;
            
            float yPosition = bridgeTile.position.y;
            bridgeTile.position += Vector3.up * animationYOffset;
            
            SpriteRenderer bridgeTileRenderer = bridgeTile.GetComponent<SpriteRenderer>();
            bridgeTileRenderer.color = new Color(1, 1, 1, 0);
            bridgeTileRenderer.enabled = true;
            
            bridgeTile.DOMoveY(yPosition, animationDuration).SetDelay(totalDelay).SetEase(animationEase);
            bridgeTileRenderer.DOColor(Color.white, animationDuration).SetDelay(totalDelay).OnComplete(() => TileBuilt(bridgeTile, bridgeIndex));
        }
    }
    
    [Header("Effects")]
    [SerializeField] private float pitchOffset;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject placeParticle;

    [Header("Locks")] 
    [SerializeField] private Animation[] locks;

    private void TileBuilt(Transform tileTransform, int bridgeIndex)
    {
        audioSource.Stop();
        audioSource.pitch += pitchOffset;
        audioSource.Play();

        Instantiate(placeParticle, tileTransform.position, Quaternion.identity, tilemapParent);

        if (tileTransform == bridgesTiles[bridgeIndex][bridgesTiles[bridgeIndex].Count - 1])
        {
            locks[bridgeIndex].Play();
            LevelManager.ResetLevelUp();
            CameraManager.Instance.UpdateButtonStates();
        }
    }

    #region Generation
    [SerializeField] private Transform tilemapParent;
    [SerializeField] private bool allowGenerate;
    
    [Button]
    public void Generate()
    {
        if (!allowGenerate) return;
        
        allowGenerate = false;
        int j = 0;
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);

            if (tile != null)
            {
                j++;
                // Create temporary GameObject at tile's position
                GameObject tileObject = new GameObject($"Tile {j}");

                tileObject.transform.SetParent(tilemapParent);
                
                tileObject.transform.position = tilemap.CellToWorld(position) + Vector3.one / 2;

                // Add SpriteRenderer and set sprite to the tile's sprite
                SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = tilemap.GetSprite(position);
            }
        }
    }
    #endregion
}
