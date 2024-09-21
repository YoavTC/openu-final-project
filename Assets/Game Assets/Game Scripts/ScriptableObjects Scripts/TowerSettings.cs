using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tower Settings")]
public class TowerSettings : ScriptableObject
{
    [Header("Base Settings")] 
    public float maxRange;
    public float damage;
    public float attackCooldown;
    public float areaOfEffect;
    public int cost;
    [ShowAssetPreview] public Sprite sprite;

    [Header("Projectile Settings")] 
    public AnimationCurve projectileCurve;
    public AnimationCurve easingCurve; 
    public float projectileMaxHeight;
    public float projectileMaxMoveSpeed;
}
