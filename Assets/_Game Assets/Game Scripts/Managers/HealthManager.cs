using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private int health;
    [SerializeField] private List<ScaleEffect> hearts;
    [SerializeField] private float destroyDelay;
    public UnityEvent Die;
    private bool dead;

    private void Start()
    {
        dead = false;
        health = 2;
    }
    
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
        hearts[health + 1].DoEffect();
        
        yield return new WaitForSeconds(destroyDelay);
        hearts[health + 1].GetComponent<Image>().enabled = false;
    }
}