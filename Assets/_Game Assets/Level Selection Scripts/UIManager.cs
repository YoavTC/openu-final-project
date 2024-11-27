using System;
using System.Collections.Generic;
using DG.Tweening;
using External_Packages;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Button playLevelButton;
    [SerializeField] private Button lastIslandButton;
    [SerializeField] private Button nextIslandButton;

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
            //graphic.color = alpha;
            graphic.DOFade(alpha, 1.5f).SetEase(Ease.Unset);
        }
    }
    #endregion
}
