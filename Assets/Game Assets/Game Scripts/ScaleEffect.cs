using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class ScaleEffect : MonoBehaviour
{
    [SerializeField] private ScaleEffectType scaleEffectType;
    
    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;
    
    [EnableIf("scaleEffectType", ScaleEffectType.PUNCH)]
    [SerializeField] private float scaleFactor;
    
    private Tweener shaker;
    
    public void DoEffect()
    {
        if (shaker == null || !shaker.IsActive())
        { 
            shaker = scaleEffectType == ScaleEffectType.PUNCH
                ? transform.DOPunchScale(transform.localScale * scaleFactor, duration, vibrato, strength)
                : transform.DOShakeScale(duration, strength, vibrato);
        }
    }

    public void KillEffect()
    {
        StartCoroutine(KillEffectRoutine());
    }

    private IEnumerator KillEffectRoutine()
    {
        yield return new WaitUntil(shaker.IsActive);
        shaker.Kill();
    }
}

public enum ScaleEffectType
{
    PUNCH, SHAKE
}
