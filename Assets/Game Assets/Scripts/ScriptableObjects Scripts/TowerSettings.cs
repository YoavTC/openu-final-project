using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Settings")]
public class TowerSettings : ScriptableObject
{
    [Header("Base Settings")] 
    public float maxRange;
    public float damage;
    public float attackCooldown;

    [Header("Projectile Settings")] 
    public AnimationCurve projectileCurve;
    public AnimationCurve easingCurve; 
    public float projectileMaxHeight;
    public float projectileMaxMoveSpeed;
}
