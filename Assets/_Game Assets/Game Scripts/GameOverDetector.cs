using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDetector : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject background;
    [SerializeField] private Animator animator;
    
    [Header("Audio")]
    [SerializeField] private AudioSource mainMusicSource;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winAudioClip, loseAudioClip;

    [Header("Settings")] 
    [SerializeField] private Vector2 gameOverScreenDelay;
    
    private bool spawnerStopped;
    private bool gameOver = false;
    
    public void OnSpawnerStoppedUnityEventListener() => spawnerStopped = true;
    
    public void OnDieUnityEventListener() => StartCoroutine(SendToLevelSelectionScene(false));
    public void OnEnemyKilledUnityEventListener()
    {
        if (gameOver) return;
        if (spawnerStopped && EnemyManager.Instance.AllEntitiesDead())
        {
            // Reset timescale
            Time.timeScale = 1f;

            gameOver = true;
            
            StartCoroutine(SendToLevelSelectionScene(true));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(SendToLevelSelectionScene(true));
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(SendToLevelSelectionScene(false));
        }
    }

    private IEnumerator SendToLevelSelectionScene(bool won)
    {
        yield return new WaitForSecondsRealtime(gameOverScreenDelay.x);
        
        animator.SetTrigger(won ? "Win" : "Lose");
        if (won) LevelManager.LevelUp();

        mainMusicSource.DOFade(0f, 0.5f);
        audioSource.clip = won ? winAudioClip : loseAudioClip;
        audioSource.Play();
        
        yield return new WaitForSecondsRealtime(gameOverScreenDelay.y);
     
        SceneTransitionManager.Instance.LoadScene("Level Selection Scene");
        // SceneManager.LoadScene("Level Selection Scene");
    }
}
