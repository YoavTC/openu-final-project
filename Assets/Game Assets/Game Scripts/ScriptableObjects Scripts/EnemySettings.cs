using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [ShowAssetPreview] public Sprite sprite;
    public int reward;
    public int damage;
    public float speed;
    public float health;

    public bool hasPostDeathEffect;
    [EnableIf("hasPostDeathEffect")] public PostDeathEffect postDeathEffect;
}
