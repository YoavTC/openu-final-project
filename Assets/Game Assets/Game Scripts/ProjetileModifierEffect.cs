using UnityEngine;

public class ProjetileModifierEffect : MonoBehaviour
{
    [SerializeField] private ModifierEffect modifierEffect;

    public void ApplyEffectToTarget(Transform target, Transform projectileOwner)
    {
        if (modifierEffect == null) return;
        target.GetComponent<ModifierAffectableBase>().StartEffect(modifierEffect, projectileOwner);

        // switch (modifierEffect.type)
        // {
        //     case ModifierEffectType.HEALTH:
        //         target.GetComponent<ModifierAffectableBase>().StartEffect(modifierEffect, projectileOwner);
        //         break;
        //     case ModifierEffectType.SPEED:
        //         target.GetComponent<ModifierAffectableBase>().StartEffect(modifierEffect, projectileOwner);
        //         break;
        //     default:
        //         return;
        // }
    }
}
