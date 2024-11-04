using UnityEngine;

public class DeathParticleInitializer : MonoBehaviour
{
    private ParticleSystem particles;

    public void Play(SpriteRenderer spriteRenderer)
    {
        particles = GetComponent<ParticleSystem>();
        particles.GetComponent<Renderer>().material = spriteRenderer.material;
        particles.textureSheetAnimation.SetSprite(0, spriteRenderer.sprite);
        
        particles.Play();
    }
}