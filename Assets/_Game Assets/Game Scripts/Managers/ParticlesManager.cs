using External_Packages;
using UnityEngine;

public class ParticlesManager : Singleton<ParticlesManager>
{
    [SerializeField] private GameObject projectileDestroy;
    [SerializeField] private SplashParticleInitializer hitRadius;
    [SerializeField] private DeathParticleInitializer enemyDeath;

    public void PlayProjectileDestroy(Vector2 pos)
    {
        Instantiate(projectileDestroy, pos, Quaternion.identity, InSceneParentProvider.GetParent(SceneParentProviderType.PARTICLES));
    }

    public void PlayHitRadius(Vector2 pos, TowerSettings towerSettings)
    {
        Instantiate(hitRadius, pos, Quaternion.identity, InSceneParentProvider.GetParent(SceneParentProviderType.PARTICLES))
            .Play(towerSettings.projectileModifierEffect.splashRadiusSprite, towerSettings.areaOfEffect + towerSettings.modifierAreaOfEffect);
    }
    
    public void PlayHitRadius(Vector2 pos, EnemySettings enemySettings)
    {
       Instantiate(hitRadius, pos, Quaternion.identity, InSceneParentProvider.GetParent(SceneParentProviderType.PARTICLES))
            .Play(enemySettings.postDeathModifierEffect.type.splashRadiusSprite, enemySettings.postDeathModifierEffect.radius);
    }

    public void PlayEnemyDeath(Vector2 pos, SpriteRenderer spriteRenderer)
    {
        Instantiate(enemyDeath, pos, Quaternion.identity, InSceneParentProvider.GetParent(SceneParentProviderType.PARTICLES))
            .Play(spriteRenderer);
    }
}