using System.Collections;
using System.Linq;
using External_Packages;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class IslandsManager : MonoBehaviour
{
    private (int, bool) currentLevel;

    [Header("Components")]
    [SerializeField] private TileBridgeBuilder tileBridgeBuilder;
    [SerializeField] private Transform locksParent;

    [Header("New Level Settings")] 
    [SerializeField] private float buildBridgeDelay;
    
    private void Start()
    {
        currentLevel =  LevelManager.GetLevel();
        
        InitializeIslandPinsArray();
        StartCoroutine(PopIslandPins());
        
        // Remove old locks
        for (int i = 0; i < locksParent.childCount; i++)
        {
            bool doodoo = i + 2 > currentLevel.Item1;
            Debug.Log($"i: {i} is bigger than {currentLevel.Item1}? {doodoo}");
            locksParent.GetChild(i).gameObject.SetActive(doodoo);
        }

        if (currentLevel.Item2)
        {
            // Build bridge
            StartCoroutine(tileBridgeBuilder.RevealBridge(currentLevel.Item1 - 1, buildBridgeDelay));
        }
    }
    
    #region Island Pins
    [Header("Island Pins")]
    [SerializeField] private Transform islandPinParent;
    Animation[] islandPins;
    
    private void InitializeIslandPinsArray()
    {
        islandPins = HelperFunctions.GetChildren(islandPinParent).Select(a => a.GetComponent<Animation>()).ToArray();
    }
    
    private IEnumerator PopIslandPins()
    {
        for (int i = 0; i < currentLevel.Item1; i++)
        {
            if (i + 1 == currentLevel.Item1) yield return new WaitForSeconds(0.3f);
            PopIslandPin(i, i + 1 != currentLevel.Item1);
        }
    }
    
    private void PopIslandPin(int index, bool silently = false)
    {
        if (silently)
        {
            islandPins[index].GetComponentInChildren<SpriteRenderer>().enabled = true;
        } else islandPins[index].Play();
    }
    #endregion
}