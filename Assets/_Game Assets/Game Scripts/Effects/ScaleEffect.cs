using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class ScaleEffect : EffectBase
{
    [SerializeField] private ScaleEffectType scaleEffectType;
    
    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;
    
    [EnableIf("scaleEffectType", ScaleEffectType.PUNCH)]
    [SerializeField] private float scaleFactor;

    public override void DoEffect()
    {
        if (transform != null)
        {
            transform.DOKill(true);
            tweener = scaleEffectType == ScaleEffectType.PUNCH
                ? transform.DOPunchScale(transform.localScale * scaleFactor, duration, vibrato, strength)
                : transform.DOShakeScale(duration, strength, vibrato);
            
            base.DoEffect();
        }
    }
}

public enum ScaleEffectType
{
    PUNCH, SHAKE
}
