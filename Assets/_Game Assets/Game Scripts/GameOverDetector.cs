using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDetector : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject background;
    [SerializeField] private Animator animator;

    [Header("Settings")] 
    [SerializeField] private Vector2 gameOverScreenDelay;
    
    private bool spawnerStopped;
    
    public void OnSpawnerStoppedUnityEventListener() => spawnerStopped = true;
    
    public void OnDieUnityEventListener() => StartCoroutine(SendToLevelSelectionScene(false));
    public void OnEnemyKilledUnityEventListener()
    {
        if (spawnerStopped && EnemyManager.Instance.AllEntitiesDead())
        {
            // Reset timescale
            Time.timeScale = 1f;
            
            StartCoroutine(SendToLevelSelectionScene(true));
        }
    }
    
    private IEnumerator SendToLevelSelectionScene(bool won)
    {
        yield return new WaitForSecondsRealtime(gameOverScreenDelay.x);
        
        animator.SetTrigger(won ? "Win" : "Lose");
        if (won) LevelManager.LevelUp();
        
        yield return new WaitForSecondsRealtime(gameOverScreenDelay.y);
     
        SceneTransitionManager.Instance.LoadScene("Level Selection Scene");
        // SceneManager.LoadScene("Level Selection Scene");
    }
}
