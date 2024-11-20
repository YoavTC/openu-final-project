using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDetector : MonoBehaviour
{
    private bool spawnerStopped;
    
    public void OnEnemyKilled()
    {
        if (spawnerStopped && EnemyManager.Instance.AllEntitiesDead()) StartCoroutine(SendBackToGULAG(true));
    }

    public void OnSpawnerStopped()
    {
        spawnerStopped = true;
    }
    
    

    public void LoseAlready()
    {
        StartCoroutine(SendBackToGULAG(false));
    }

    [SerializeField] private GameObject goodScreen, badScreen, bg;

    private IEnumerator SendBackToGULAG(bool g)
    {
        

        
        
        if (g)
        {
            yield return new WaitForSecondsRealtime(1.2f);
            bg.SetActive(true);
            PlayerPrefs.SetInt("LVL", PlayerPrefs.GetInt("LVL") + 1);
            goodScreen.SetActive(true);
        } else
        {
            bg.SetActive(true);
            badScreen.SetActive(true);
        }
        
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene("Level Selection Scene");
    }
}
