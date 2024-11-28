using DG.Tweening;
using UnityEngine;

public class ScaleEffect : EffectBase
{
    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;
    [SerializeField] private float scaleFactor;
    [SerializeField] private bool ignoreTimeScale;
	[SerializeField] private bool loop;
	[SerializeField] private LoopType loopType;

    public override void DoEffect()
    {
        if (transform != null)
        {
            transform.DOKill(true);
            transform.DOPunchScale(transform.localScale * scaleFactor, duration, vibrato, strength)
				.SetLoops(loop ? -1 : 0, loopType)
                .SetUpdate(ignoreTimeScale);
            base.DoEffect();
        }
    }
}
