using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SplashParticleInitializer : MonoBehaviour
{
    private ParticleSystem particles;

    public void Play(Sprite sprite, float range)
    {
        particles = GetComponent<ParticleSystem>();
        particles.textureSheetAnimation.SetSprite(0, sprite);

        float spriteDiameter = sprite.bounds.size.x;
        float scale = range * 2 / spriteDiameter;

        transform.localScale = 10 * new Vector3(scale, scale, 1f);
        
        particles.Play();
    }
}
