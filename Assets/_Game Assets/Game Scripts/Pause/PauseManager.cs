using System;
using External_Packages;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] [Scene] private int pauseSceneIndex;
    [SerializeField] private KeyCode pauseKey;
    [SerializeField] private float pauseCooldown;
    private float elapsedTime;
    private bool isPaused;

    private void Start()
    {
        isPaused = false;
        
        #if UNITY_EDITOR
        if (pauseKey == KeyCode.Escape)
        {
            pauseKey = KeyCode.Tab;
        }
        #endif
    }

    private void Update()
    {
        elapsedTime += Time.unscaledDeltaTime;
        
        if (Input.GetKeyDown(pauseKey) && elapsedTime >= pauseCooldown)
        {
            elapsedTime = 0f;
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    private float originalTimeScale;

    private void PauseGame()
    {
        isPaused = true;
        
        // Freeze time
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // Load pause menu scene
        SceneManager.LoadScene(pauseSceneIndex, LoadSceneMode.Additive);
    }

    public void AddButtonListeners(Button resumeGameButton, Button quitGameButton)
    {
        resumeGameButton.onClick.AddListener(ResumeGame);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    private void ResumeGame()
    {
        isPaused = false;
        
        // Un-Freeze time
        Time.timeScale = originalTimeScale;
        
        // Unload pause menu scene
        SceneManager.UnloadSceneAsync(pauseSceneIndex);
    }

    private void QuitGame()
    {
        // Clean pause state cache
        ResumeGame();

        SceneManager.LoadScene("Level Selection Scene");
    }
}
