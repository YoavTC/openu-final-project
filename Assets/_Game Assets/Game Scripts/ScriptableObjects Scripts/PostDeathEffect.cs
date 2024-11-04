using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Post Death Effect")]
public class PostDeathEffect : ScriptableObject
{
    public PostDeathEffectType type;
    public bool toFriendly;
    public float radius;
    public float maxAmount;
    public AnimationCurve curve;
}

public enum PostDeathEffectType
{
    HEALTH,
    SPEED,
}