using DG.Tweening;
using UnityEngine;

public class BobEffect : EffectBase
{
    [SerializeField] private float duration;
    [SerializeField] private Vector2 bobDirection;
    [SerializeField] private bool ignoreTimeScale;
    [SerializeField] private bool loop;
    [SerializeField] private LoopType loopType;

    public override void DoEffect()
    {
        if (transform != null)
        {
            transform.DOKill(true);
            transform.GetComponent<RectTransform>().DOAnchorPos(transform.GetComponent<RectTransform>().anchoredPosition + bobDirection, duration)
            
                .SetLoops(loop ? -1 : 0, loopType)
                .SetUpdate(ignoreTimeScale);
            base.DoEffect();
        }
    }
}