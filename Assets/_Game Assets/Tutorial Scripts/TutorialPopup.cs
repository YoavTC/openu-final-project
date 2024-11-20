using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    [Header("Tween Settings")] 
    [SerializeField] private float strength;
    [SerializeField] private float exitDuration, enterDuration;
    
    private float originalTimeScale;
    private TutorialPopupManager tutorialPopupManager;

    private void Awake()
    {
        HideChildren(true);
        tutorialPopupManager = GetComponentInParent<TutorialPopupManager>();
    }

    public void Trigger()
    {
        // Freeze game
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        
        // Call manager event
        tutorialPopupManager.OnPopupTriggered(this);
     
        // Show elements
        HideChildren(false);
        transform.localScale = Vector3.one;
        transform.DOPunchScale(transform.localScale * strength, enterDuration).SetUpdate(true);
        Debug.Log("Time frozen");
    }
    
    public void Stop()
    {
        // Unfreeze game
        Time.timeScale = originalTimeScale;
        
        //Hide elements
        transform.DOScale(Vector3.zero, exitDuration).SetUpdate(true).OnComplete(() =>
        {
            HideChildren(true);
            tutorialPopupManager.OnPopupStopped(this);
        });
        // HideChildren(true);
        Debug.Log("Time un-frozen");
    }
    
    private void HideChildren(bool hidden)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!hidden);
        }
    }
}