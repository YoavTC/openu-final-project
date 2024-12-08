using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialPopup;
    private bool showTutorialPopup;

    [Header("Scenes")] 
    [SerializeField] [Scene] private int tutorialScene;
    [SerializeField] [Scene] private int levelSelectionScene;

    [Header("Intro")] 
    [SerializeField] private PlayableDirector introDirector;

    private void Start()
    {
        var currentLevel = LevelManager.GetLevel();
        showTutorialPopup = currentLevel.Item1 == 1;
    }

    private void Update()
    {
        if (introDirector.state == PlayState.Playing && Input.anyKey)
        {
            SkipIntro();
        }
    }

    private void SkipIntro()
    {
        introDirector.time = introDirector.duration;
        introDirector.GetComponent<AudioSource>().Stop();
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
