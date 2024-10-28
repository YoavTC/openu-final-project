﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class EffectBase : MonoBehaviour, IEffect
{
    public Tweener tweener { get; set; }

    public virtual void DoEffect() { }

    public virtual void KillEffect()
    {
        StartCoroutine(KillEffectRoutine());
    }

    public IEnumerator KillEffectRoutine()
    {
        yield return new WaitUntil(tweener.IsActive);
        tweener.Kill();
    }
}
public interface IEffect
{
    public Tweener tweener { get; set; }
        
    public void DoEffect();
    public void KillEffect();
}