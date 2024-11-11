using UnityEngine;

public class DeathEffect : EffectBase
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DeathParticleInitializer particles;

    public override void DoEffect()
    {
        Instantiate(particles, 
                transform.position, 
                Quaternion.identity,
                SceneParentProvider.GetParent(SceneParentProviderType.PARTICLES))
            .Play(spriteRenderer);
    }
}
