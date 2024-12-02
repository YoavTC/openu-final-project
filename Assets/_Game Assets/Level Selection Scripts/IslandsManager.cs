using System.Collections;
using System.Linq;
using External_Packages;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class IslandsManager : MonoBehaviour
{
    private (int, bool) currentLevel;
    
    private void Start()
    {
        currentLevel =  LevelManager.GetLevel();
        
        InitializeIslandPinsArray();
        StartCoroutine(PopIslandPins());
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