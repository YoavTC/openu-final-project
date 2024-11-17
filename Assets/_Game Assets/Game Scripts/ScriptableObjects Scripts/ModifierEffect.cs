using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Scriptable Objects/Modifier Effect")]
public class ModifierEffect : ScriptableObject
{
    public ModifierEffectType type;
    public AnimationCurve strengthCurve;

    public float tickRate;
    public float duration;
}
