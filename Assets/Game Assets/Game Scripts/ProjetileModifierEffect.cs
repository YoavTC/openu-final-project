using UnityEngine;

public class ProjetileModifierEffect : MonoBehaviour
{
    [SerializeField] private ModifierEffect modifierEffect;

    public void ApplyEffectToTarget(Transform target)
    {
        if (modifierEffect == null) return;

        switch (modifierEffect.type)
        {
            case ModifierEffectType.HEALTH:
                target.GetComponent<HealthBase>().StartEffect(modifierEffect);
                break;
            case ModifierEffectType.SPEED:
                target.GetComponent<AnimateOnSpline>().StartEffect(modifierEffect);
                break;
            default:
                return;
        }
    }
}
