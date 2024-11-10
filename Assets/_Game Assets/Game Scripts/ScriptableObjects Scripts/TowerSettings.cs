using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/TowerDefault Settings")]
public class TowerSettings : ScriptableObject
{
    [Header("Information")] 
    public string towerName;
    public string description;
    
    [Header("Base Settings")] 
    public float damage;
    public float attackCooldown;
    public float maxRange;
    public float dps;
    public float health;
    public float areaOfEffect;
    public int cost;
    [ShowAssetPreview] public Sprite sprite;

    [Header("Projectile Settings")] 
    public AnimationCurve projectileCurve;
    public AnimationCurve easingCurve; 
    public float projectileMaxHeight;
    public float projectileMaxMoveSpeed;
    public ModifierEffect projectileModifierEffect;
    public float modifierAreaOfEffect;
    public Sprite splashRadiusSprite;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        dps = attackCooldown > 0 ? damage / attackCooldown : 0;
    }
    #endif
}
