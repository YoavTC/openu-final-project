using System;
using System.Collections;
using External_Packages;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    private void Start()
    {
        DontDestroyOnLoad(this);

        transitionCanvas = GetComponent<Canvas>();
        transitionAnimator = GetComponentInChildren<Animator>();
    }

    [Header("Components")]
    [SerializeField] private Canvas transitionCanvas;
    [SerializeField] private Animator transitionAnimator;
    
    [Header("Transition")]
    [SerializeField] private float transitionDuration = 1f;
    
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        StartCoroutine(Transition(() =>
        {
            SceneManager.LoadScene(sceneName);
        }));
    }

    public void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1f;
        StartCoroutine(Transition(() =>
        {
            SceneManager.LoadScene(sceneIndex);
        }));
    }

    private IEnumerator Transition(Action finishCallback = null)
    {
        Time.timeScale = 1f;
        transitionAnimator.SetTrigger("Transition");
        yield return new WaitForSecondsRealtime(transitionDuration);
        Time.timeScale = 1f;
        finishCallback?.Invoke();
    }
}
