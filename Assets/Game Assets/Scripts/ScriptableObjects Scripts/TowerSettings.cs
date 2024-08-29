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
    [CurveRange(0, 1, 0, 1)]
    public AnimationCurve projectilePath;
    public float projectileMovementSpeed;
}
