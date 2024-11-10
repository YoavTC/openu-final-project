using DG.Tweening;
using UnityEngine;

public class ScaleEffect : EffectBase
{
    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;
    [SerializeField] private float scaleFactor;

    public override void DoEffect()
    {
        if (transform != null)
        {
            transform.DOKill(true);
            transform.DOPunchScale(transform.localScale * scaleFactor, duration, vibrato, strength);
            base.DoEffect();
        }
    }
}
