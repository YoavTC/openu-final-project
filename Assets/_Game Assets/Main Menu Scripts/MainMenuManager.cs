using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("popups")]
    [SerializeField] private GameObject tutorialPopup;
    private bool showTutorialPopup;
    [SerializeField] private GameObject quitPopup;

    [Header("Scenes")] 
    [SerializeField] [Scene] private int tutorialScene;
    [SerializeField] [Scene] private int levelSelectionScene;

    [Header("Intro")] 
    [SerializeField] private PlayableDirector introDirector;
    [SerializeField] private KeyCode[] skipKeyCodes;

    [Header("Screens")] 
    [SerializeField] private RectTransform mainScreen;
    [SerializeField] private RectTransform creditsScreen;

    [Header("Screen Transition Settings")] 
    [SerializeField] private float screenTransitionDuration;
    [SerializeField] [ReadOnly] private bool mainScreenActive;
    [SerializeField] [ReadOnly] private int screenWidth;
    [SerializeField] private Sprite[] creditsButtonIcons;
    [SerializeField] private Button creditsButton;

    private void Start()
    {
        var currentLevel = LevelManager.GetLevel();
        showTutorialPopup = currentLevel.Item1 == 1;

        mainScreenActive = true;
        screenWidth = Screen.currentResolution.width;
    }

    private void Update()
    {
        if (introDirector.state == PlayState.Playing && UserSkippingIntro())
        {
            SkipIntro();
        }
    }

    private bool UserSkippingIntro()
    {
        for (int i = 0; i < skipKeyCodes.Length; i++)
        {
            if (Input.GetKey(skipKeyCodes[i])) return true;
        }

        return false;
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
            SceneTransitionManager.Instance.LoadScene(levelSelectionScene);
            // SceneManager.LoadScene(levelSelectionScene);
        }
    }

    public void OnPressPlayTutorialButton()
    {
        // SceneManager.LoadScene(tutorialScene);
        SceneTransitionManager.Instance.LoadScene(tutorialScene);
    }

    [Button]
    public void OnPressCreditsButton()
    {
        mainScreen.DOAnchorPosX(mainScreenActive ? mainScreen.anchoredPosition.x + screenWidth : 0, screenTransitionDuration);
        creditsScreen.DOAnchorPosX(mainScreenActive ? 0 : -screenWidth, screenTransitionDuration);
        creditsButton.image.sprite = creditsButtonIcons[mainScreenActive ? 1 : 0];

        mainScreenActive = !mainScreenActive;
    }

    public void OnPressQuitGameButton(bool force)
    {
        if (force)
        {
            Application.Quit();
        }
        
        quitPopup.SetActive(true);
    }
}