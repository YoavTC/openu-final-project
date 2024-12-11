using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPopup : MonoBehaviour
{
    [Header("Tween Settings")] 
    [SerializeField] private float strength;
    [SerializeField] private float exitDuration, enterDuration;

    [Header("Popup")]
    [SerializeField] private float delay;
    [SerializeField] private bool playOnAwake;
    [SerializeField] private bool isOneShot = false;
    private bool hasPlayed = false;
    private bool hasFinished = false;

    [Header("Audio")]
    [SerializeField] private AudioClip popupSoundEffect;
    [SerializeField] private AudioClipSettingsStruct audioClipSettings;

    [Foldout("Events")] public UnityEvent<TutorialPopup> OnPopupTriggerUnityEvent;
    [Foldout("Events")] public UnityEvent<TutorialPopup> OnPopupStopUnityEvent;
    
    private float originalTimeScale;
    private TutorialPopupManager tutorialPopupManager;

    private void Awake()
    {
        HideChildren(true);
        tutorialPopupManager = GetComponentInParent<TutorialPopupManager>();
        
        if (playOnAwake) Trigger();
    }

    public void Trigger()
    {
        if (isOneShot && hasPlayed) return;
        StartCoroutine(TriggerCoroutine());
    }

    private IEnumerator TriggerCoroutine()
    {
        // Delay
        yield return new WaitForSeconds(delay);
        
        OnPopupTriggerUnityEvent?.Invoke(this);
        
        // Show elements
        HideChildren(false);
        transform.localScale = Vector3.one;
        transform.DOPunchScale(transform.localScale * strength, enterDuration).SetUpdate(true);
        
        // Play SFX
        AudioManager.Instance.PlayAudioClip(popupSoundEffect, audioClipSettings);
        
        // Freeze game
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        
        // Call manager event
        tutorialPopupManager.OnPopupTriggered(this);

        hasPlayed = true;
    }

    public void Stop(float stopDelay = 0)
    {
        if (hasPlayed && !hasFinished) StartCoroutine(StopCoroutine(stopDelay));
    }

    private IEnumerator StopCoroutine(float stopDelay)
    {
        // Delay
        yield return new WaitForSecondsRealtime(stopDelay);
        
        OnPopupStopUnityEvent?.Invoke(this);
        
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

        hasFinished = true;
    }
    
    private void HideChildren(bool hidden)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!hidden);
        }
    }
}