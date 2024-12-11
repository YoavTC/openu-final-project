using System;
using System.Collections.Generic;
using DG.Tweening;
using External_Packages;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Components")]
    [SerializeField] private Button playLevelButton;
    [SerializeField] private Button lastIslandButton;
    [SerializeField] private Button nextIslandButton;

    [Header("Settings")]
    [SerializeField] private float fadeTransitionDuration;

    private CameraManager cameraManager;

    private void Start()
    {
        cameraManager = Camera.main.GetComponent<CameraManager>();
        
        // ToggleButtonState(playLevelButton, false);
        // ToggleButtonState(lastIslandButton, false);
        // ToggleButtonState(nextIslandButton, false);
        //
        ToggleButtonState(playLevelButton, true);
        ToggleButtonState(lastIslandButton, true);
        ToggleButtonState(nextIslandButton, true);
    }

    public void OnPlayLevelButtonPressed()
    {
        int level = cameraManager.GetCurrentIslandIndex();
        Debug.Log($"LVL_{level}");
        SceneTransitionManager.Instance.LoadScene($"LVL_{level}");
    }

    public void OnLastLevelButtonPressed()
    {
        cameraManager.GoToLastIsland();
    }

    public void OnNextLevelButtonPressed()
    {
        cameraManager.GoToNextIsland();
    }

    #region Disabling & Enabling
    public void UpdateButtonsStates(bool canPlay, bool canLast, bool canNext)
    {
        ToggleButtonState(playLevelButton, canPlay);
        ToggleButtonState(lastIslandButton, canLast);
        ToggleButtonState(nextIslandButton, canNext);
    }
    
    private void ToggleButtonState(Button button, bool state) 
    {
        button.interactable = state;
        
        List<Graphic> graphics = new List<Graphic>();
        graphics.AddRange(button.GetComponentsInChildren<Graphic>());
        
        foreach (Graphic graphic in graphics)
        {
            float alpha = button.interactable ? 1f : 0.2f;
            graphic.DOFade(alpha, fadeTransitionDuration).SetEase(Ease.Unset);
        }
    }
    #endregion
}
