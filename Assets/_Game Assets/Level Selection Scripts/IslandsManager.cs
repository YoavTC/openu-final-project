using System.Collections;
using System.Linq;
using DG.Tweening;
using External_Packages;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IslandsManager : MonoBehaviour
{
    private (int, bool) currentLevel;

    [Header("Components")]
    [SerializeField] private TileBridgeBuilder tileBridgeBuilder;
    [SerializeField] private Transform locksParent;
    [SerializeField] private Animation[] locks;
    [SerializeField] private GameObject sceneTransitionManagerPrefab;

    [Header("New Level Settings")] 
    [SerializeField] private float buildBridgeDelay;

    [Header("Music")] 
    [SerializeField] private AudioSource mainMusicSource;
    
    private void Start()
    {
        currentLevel =  LevelManager.GetLevel();

        if (currentLevel.Item1 == 1)
        {
            LevelManager.LevelUp();
            currentLevel.Item2 = true;
        }
        
        InitializeIslandPinsArray();
        StartCoroutine(PopIslandPins());

        locks = new Animation[locksParent.childCount];
        
        // Remove old locks
        for (int i = 0; i < locksParent.childCount; i++)
        {
            locks[i] = locksParent.GetChild(i).GetComponent<Animation>();
            locksParent.GetChild(i).gameObject.SetActive(i + 2 > currentLevel.Item1);
        }
        
        if (currentLevel.Item2)
        {
            // Build new bridge
            tileBridgeBuilder.RevealBridge(currentLevel.Item1 - 1, buildBridgeDelay, UnlockNewIsland);
        } else tileBridgeBuilder.RevealBridge(currentLevel.Item1 - 2, 0f, UnlockNewIsland);
        
        if (!SceneTransitionManager.Instance) Instantiate(sceneTransitionManagerPrefab);
    }

    // Called from TileBridgeBuilder class when last bridge tile has been placed
    private void UnlockNewIsland(int bridgeIndex, bool levelUp)
    {
        if (levelUp)
        {
            locks[bridgeIndex].Play();
            LevelManager.ResetLevelUp();
        }
        CameraManager.Instance.UpdateButtonStates();
    }

    public void OnPressMainMenuButton()
    {
        mainMusicSource.DOFade(0, 0.5f);
        SceneTransitionManager.Instance.LoadScene(0);
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