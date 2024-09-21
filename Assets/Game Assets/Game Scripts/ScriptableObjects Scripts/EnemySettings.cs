using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [ShowAssetPreview] public Sprite sprite;
    public float speed;
    public float health;
}
