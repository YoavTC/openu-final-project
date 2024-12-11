using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private List<ScaleEffect> hearts;
    [SerializeField] private float destroyDelay;
    [SerializeField] private AudioSource audioSource;
    
    private int health;
    private bool dead;
    
    public UnityEvent Die;

    private void Start()
    {
        dead = false;
        health = 2;
    }
    
    [Button]
    public void OnEnemyReachEnd()
    {
        if (dead) return;
        health--;
        StartCoroutine(PopHeart());
        if (health < 0)
        {
            dead = true;
            Die?.Invoke();
        }
    }

    private IEnumerator PopHeart()
    {
        audioSource.Play();
        hearts[health + 1].DoEffect();
        
        yield return new WaitForSeconds(destroyDelay);
        hearts[health + 1].GetComponent<Image>().enabled = false;
    }
}