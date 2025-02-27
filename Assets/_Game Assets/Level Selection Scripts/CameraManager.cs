using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using External_Packages;
using NaughtyAttributes;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Transform islandsParent;
    private List<LevelIsland> islandsList = new List<LevelIsland>();

    private LevelIsland currentIsland;
    private int currentIslandIndex;

    [SerializeField] private Transform player;
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private float cameraMoveSpeed;

    private IEnumerator Start()
    {
        islandsList = (HelperFunctions.GetChildren(islandsParent)).Select(a => a.GetComponent<LevelIsland>()).ToList();

        currentIslandIndex = LevelManager.GetLevel().Item1 - 1;
        currentIsland = islandsList[currentIslandIndex];
        
        UpdateButtonStates();

        yield return new WaitForSeconds(0.1f);
        
        MoveToIslandInstant();
    }

    [Button]
    public void GoToNextIsland()
    {
        MoveToIsland(true);
    }
    
    [Button]
    public void GoToLastIsland()
    {
        MoveToIsland(false);
    }

    private void MoveToIslandInstant()
    {
        Vector3 nextIslandPos = currentIsland.centerPos;
        
        // Move player
        player.position = nextIslandPos;
        
        // Move camera
        nextIslandPos.z = transform.position.z;
        transform.position = nextIslandPos;
        
        UpdateButtonStates();
    }

    private void MoveToIsland(bool forward)
    {
        StartCoroutine(MovePlayerSequence(forward));
        currentIslandIndex += forward ? 1 : -1;
        currentIslandIndex = Mathf.Clamp(currentIslandIndex, 0, islandsList.Count);
        currentIsland = islandsList[currentIslandIndex];

        Vector3 camNextPos = currentIsland.centerPos;
        camNextPos.z = transform.position.z;
        transform.DOMove(camNextPos, Vector2.Distance(transform.position, camNextPos) / cameraMoveSpeed);
    }

    public void UpdateButtonStates()
    {
        bool canPlay = LevelManager.GetLevel().Item1 > currentIslandIndex;
        UIManager.Instance.UpdateButtonsStates(canPlay, currentIslandIndex > 0, LevelManager.GetLevel().Item1 > currentIslandIndex + 1);
    }

    public int GetCurrentIslandIndex()
    {
        return currentIslandIndex;
    }

    private IEnumerator MovePlayerSequence(bool forward)
    {
        UIManager.Instance.UpdateButtonsStates(false, false, false);
        
        // Move the player and wait for completion
        yield return MovePlayerTo(forward ? currentIsland.exitPos : currentIsland.entryPos);
        yield return MovePlayerTo(forward ? currentIsland.entryPos : currentIsland.exitPos);
        yield return MovePlayerTo(currentIsland.centerPos);

        UpdateButtonStates();
    }

    private IEnumerator MovePlayerTo(Vector3 targetPos)
    {
        float duration = Vector2.Distance(player.position, targetPos) / playerMoveSpeed;
        var currentTween = player.DOMove(targetPos, duration).SetEase(Ease.Linear);
        yield return currentTween.WaitForCompletion();
    }
}
