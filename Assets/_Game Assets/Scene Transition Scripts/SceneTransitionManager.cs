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
    
    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    [Header("Components")]
    [SerializeField] private Canvas transitionCanvas;
    [SerializeField] private Animator transitionAnimator;
    
    [Header("Transition")]
    [SerializeField] private float transitionDuration;
    [SerializeField] private string transitionInParam;
    [SerializeField] private string transitionOutParam;
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // if (scene.buildIndex != 0) 
            StartCoroutine(Transition(true));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(false, () =>
        {
            SceneManager.LoadScene(sceneName);
        }));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(Transition(false, () =>
        {
            SceneManager.LoadScene(sceneIndex);
        }));
    }

    private IEnumerator Transition(bool transitionIn, Action finishCallback = null)
    {
        transitionAnimator.SetTrigger(transitionIn ? transitionInParam : transitionOutParam);
        yield return new WaitForSeconds(transitionDuration);
        finishCallback?.Invoke();
    }
}
