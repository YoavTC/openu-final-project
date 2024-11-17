using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Post Death Modifier Effect")]
public class PostDeathModifierEffect : ScriptableObject
{
    public ModifierEffect type;
    public bool toFriendly;
    public float amount;
    public float radius;
    public Sprite splashRadiusSprite;
}