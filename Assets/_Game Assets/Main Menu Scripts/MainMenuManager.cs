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

    [Header("Music")] 
    [SerializeField] private AudioSource mainMusicSource;
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Sprite[] musicToggleIcons;
    [SerializeField] private float musicTransitionDuration;
    [SerializeField] private float musicForceMuteDuration;
    [SerializeField] [ReadOnly] private bool musicToggleState;
    [SerializeField] [ReadOnly] private float initialMusicVolume; 

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

        musicToggleState = false;
        initialMusicVolume = mainMusicSource.volume;
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
            OnPressToggleMusicButton(true);
            if (LevelManager.GetLevel().Item1 == 1) LevelManager.LevelUp();
            SceneTransitionManager.Instance.LoadScene(levelSelectionScene);
            // SceneManager.LoadScene(levelSelectionScene);
        }
    }

    public void OnPressPlayTutorialButton()
    {
        // SceneManager.LoadScene(tutorialScene);
        OnPressToggleMusicButton(true);
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

    public void OnPressToggleMusicButton(bool forceMute)
    {
        if (forceMute)
        {
            mainMusicSource.DOFade(0, musicForceMuteDuration);
        }
        else
        {
            musicToggleButton.image.sprite = musicToggleIcons[musicToggleState ? 1 : 0];
            mainMusicSource.DOFade(musicToggleState ? initialMusicVolume : 0, musicTransitionDuration);
        
            musicToggleState = !musicToggleState;
        }
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