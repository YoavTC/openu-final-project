using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase : HealthBase
{
    private ModifierEffect currentEffect;
    [SerializeField] private List<Transform> currentEffectGivers = new List<Transform>();

    public virtual void StartEffect(ModifierEffect newModifierEffect, Transform effectGiver)
    {
        currentEffect = newModifierEffect;
        if (!currentEffectGivers.Contains(effectGiver))
        {
            currentEffectGivers.Add(effectGiver);
            StartCoroutine(TickEffect(newModifierEffect.type, effectGiver));
        }
    }
    
    public virtual IEnumerator TickEffect(ModifierEffectType type, Transform effectGiver)
    {
        float durationProgress = 0f;
        float tickRate = currentEffect.tickRate;
        while (durationProgress <= currentEffect.duration)
        {
            ApplyEffect(type, currentEffect.strengthCurve.Evaluate(durationProgress));
            
            yield return new WaitForSeconds(tickRate);
            durationProgress += tickRate;
        }
        
        FinishEffect();
        currentEffectGivers.Remove(effectGiver);
    }

    public virtual void FinishEffect() {}
    protected abstract void ApplyEffect(ModifierEffectType type, float amount);
}