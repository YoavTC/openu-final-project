using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using External_Packages;
using NaughtyAttributes;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform islandsParent;
    private List<LevelIsland> islandsList = new List<LevelIsland>();

    private LevelIsland currentIsland;
    private int currentIslandIndex;

    [SerializeField] private Transform player;
    [SerializeField] private float playerMoveDuration;
    [SerializeField] private float cameraMoveDuration;

    private void Start()
    {
        islandsList = (HelperFunctions.GetChildren(islandsParent)).Select(a => a.GetComponent<LevelIsland>()).ToList();

        currentIsland = islandsList[0];
        currentIslandIndex = 0;
        
        UpdateButtonStates();
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

    private void MoveToIsland(bool forward)
    {
        StartCoroutine(MovePlayerSequence(forward));
        currentIslandIndex += forward ? 1 : -1;
        currentIslandIndex = Mathf.Clamp(currentIslandIndex, 0, islandsList.Count);
        currentIsland = islandsList[currentIslandIndex];

        Vector3 camNextPos = currentIsland.centerPos;
        camNextPos.z = transform.position.z;
        transform.DOMove(camNextPos, Vector2.Distance(transform.position, camNextPos) / cameraMoveDuration);
     
        //UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        // TODO: Unlock logic
        UIManager.Instance.UpdateButtonsStates(true, currentIslandIndex > 0, currentIslandIndex < islandsList.Count - 1);
    }

    private IEnumerator MovePlayerSequence(bool forward)
    {
        UIManager.Instance.UpdateButtonsStates(false, false, false);
        
        // A helper method to move the player and wait for completion
        yield return MovePlayerTo(forward ? currentIsland.exitPos : currentIsland.entryPos);
        yield return MovePlayerTo(forward ? currentIsland.entryPos : currentIsland.exitPos);
        yield return MovePlayerTo(currentIsland.centerPos);

        UpdateButtonStates();
    }

    private IEnumerator MovePlayerTo(Vector3 targetPos)
    {
        float duration = Vector2.Distance(player.position, targetPos) / playerMoveDuration;
        var currentTween = player.DOMove(targetPos, duration).SetEase(Ease.Linear);
        yield return currentTween.WaitForCompletion();
    }
}
