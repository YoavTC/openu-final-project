using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class ScaleEffect : EffectBase
{
    [SerializeField] private ScaleEffectType scaleEffectType;
    
    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;
    
    [EnableIf("scaleEffectType", ScaleEffectType.PUNCH)]
    [SerializeField] private float scaleFactor;
    
    public Tweener tweener { get; set; }

    public override void DoEffect()
    {
        if (tweener == null || !tweener.IsActive())
        { 
            tweener = scaleEffectType == ScaleEffectType.PUNCH
                ? transform.DOPunchScale(transform.localScale * scaleFactor, duration, vibrato, strength)
                : transform.DOShakeScale(duration, strength, vibrato);
        }
    }
}

public enum ScaleEffectType
{
    PUNCH, SHAKE
}
