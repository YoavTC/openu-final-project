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
        tilemap.enabled = false;
    }

    [Header("Building Animation Settings")]
    [SerializeField] private float animationDuration;
    [SerializeField] private Ease animationEase;
    [SerializeField] private float animationYOffset = 1f;
    [SerializeField] private float animationScaleDuration;
    [SerializeField] private float animationScaleMultiplier = 1f;
    private float totalDelay;

    public void RevealBridge(int bridgeIndex, float delay, Action<int, bool> LastBridgeTileBuiltCallback = null)
    {
        if (bridgeIndex > bridgesTiles.Keys.Count - 1) return;
        
        if (delay == 0f)
        {
            Debug.Log(bridgeIndex);
            InstantlyRevealBridge(bridgeIndex, LastBridgeTileBuiltCallback);
            HideTiles(bridgeIndex + 1);
        }
        else
        {
            HideTiles(bridgeIndex);
            StartCoroutine(IterativelyRevealBridge(bridgeIndex, delay, LastBridgeTileBuiltCallback));
        }
    }
    
    private void HideTiles(int bridgeIndex)
    {
        if (bridgeIndex > bridgesTiles.Keys.Count - 1) return;
        foreach (var bridgeTile in bridgesTiles[bridgeIndex])
        {
            bridgeTile.GetComponent<SpriteRenderer>().enabled = false;
        }
        
        HideTiles(bridgeIndex + 1);
    }

    private IEnumerator IterativelyRevealBridge(int bridgeIndex, float delay, Action<int, bool> LastBridgeTileBuiltCallback)
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
            
            bridgeTile.DOMoveY(yPosition, animationDuration)
                .SetDelay(totalDelay)
                .SetEase(animationEase);
            
            bridgeTileRenderer.DOColor(Color.white, animationDuration)
                .SetDelay(totalDelay)
                .OnComplete(() => TileBuilt(bridgeTile, bridgeIndex, LastBridgeTileBuiltCallback));
        }
    }
    
    private void InstantlyRevealBridge(int bridgeIndex, Action<int, bool> LastBridgeTileBuiltCallback)
    {
        foreach (Transform bridgeTile in bridgesTiles[bridgeIndex])
        {
            SpriteRenderer bridgeTileRenderer = bridgeTile.GetComponent<SpriteRenderer>();
            bridgeTileRenderer.color = new Color(1, 1, 1, 1f);
            bridgeTileRenderer.enabled = true;
        }
        
        // Last bridge tile callback for IslandManager class
        LastBridgeTileBuiltCallback?.Invoke(bridgeIndex, false);
    }
    
    [Header("Effects")]
    [SerializeField] private float pitchOffset;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject placeParticle;

    private void TileBuilt(Transform tileTransform, int bridgeIndex, Action<int, bool> LastBridgeTileBuiltCallback)
    {
        // VFX
        Instantiate(placeParticle, tileTransform.position, Quaternion.identity, tilemapParent);
        tileTransform.DOPunchScale(tileTransform.localScale * animationScaleMultiplier, animationScaleDuration);
        
        // SFX
        audioSource.Stop();
        audioSource.pitch += pitchOffset;
        audioSource.Play();

        if (tileTransform == bridgesTiles[bridgeIndex][bridgesTiles[bridgeIndex].Count - 1])
        {
            // Last bridge tile callback for IslandManager class
            LastBridgeTileBuiltCallback?.Invoke(bridgeIndex, true);
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