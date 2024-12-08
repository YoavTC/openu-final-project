using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject tutorialPopup;
    private bool showTutorialPopup;

    [Header("Scenes")] 
    [SerializeField] [Scene] private int tutorialScene;
    [SerializeField] [Scene] private int levelSelectionScene;

    private void Start()
    {
        var currentLevel = LevelManager.GetLevel();
        showTutorialPopup = currentLevel.Item1 == 1;
    }

    public void OnPressPlayButton()
    {
        if (showTutorialPopup)
        {
            tutorialPopup.SetActive(true);
            showTutorialPopup = false;
        }
        else
        {
            SceneManager.LoadScene(levelSelectionScene);
        }
    }

    public void OnPressPlayTutorialButton()
    {
        SceneManager.LoadScene(tutorialScene);
    }
}
