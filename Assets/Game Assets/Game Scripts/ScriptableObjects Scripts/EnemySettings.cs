using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [ShowAssetPreview] public Sprite sprite;
    public float speed;
    public float health;
}
