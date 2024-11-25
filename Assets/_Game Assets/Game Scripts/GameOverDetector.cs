using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDetector : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject background;
    
    private bool spawnerStopped;
    
    public void OnSpawnerStoppedUnityEventListener() => spawnerStopped = true;
    
    public void OnDieUnityEventListener() => StartCoroutine(SendToLevelSelectionScene(false));
    public void OnEnemyKilledUnityEventListener()
    {
        if (spawnerStopped && EnemyManager.Instance.AllEntitiesDead())
        {
            StartCoroutine(SendToLevelSelectionScene(true));
        }
    }
    
    private IEnumerator SendToLevelSelectionScene(bool won)
    {
        yield return null;
        // if (g)
        // {
        //     yield return new WaitForSecondsRealtime(1.2f);
        //     bg.SetActive(true);
        //     PlayerPrefs.SetInt("LVL", PlayerPrefs.GetInt("LVL") + 1);
        //     goodScreen.SetActive(true);
        // } else
        // {
        //     bg.SetActive(true);
        //     badScreen.SetActive(true);
        // }
        //
        // yield return new WaitForSecondsRealtime(2f);
        //
        // SceneManager.LoadScene("Level Selection Scene");
    }
}
