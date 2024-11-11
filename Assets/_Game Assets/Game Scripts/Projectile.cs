// Credit: UnPaws
// Source: https://www.youtube.com/watch?v=OPDl2uVaN_Q

using UnityEngine;
public class Projectile : MonoBehaviour
{
    [Header("Components & Settings")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ProjectileModifierEffect projectileModifierEffect;
    [SerializeField] private GameObject impactParticleSystem;
    [SerializeField] private GameObject radiusImpactParticleSystem;
    
    private Transform target;
    private Transform projectileOwner;

    // Speed & Trajectory Settings
    private float moveSpeed;
    private float maxMoveSpeed;
    private AnimationCurve trajectoryAnimationCurve;
    private AnimationCurve projectileSpeedAnimationCurve;
    private AnimationCurve axisCorrectionAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);

    // Damage & Area of Effect
    private float damage;
    private float areaOfEffect;
    private float modifierAreaOfEffect;
    private Sprite splashRadiusSprite;

    // Trajectory Calculations
    private float trajectoryMaxRelativeHeight;
    private Vector3 trajectoryStartPoint;
    private Vector3 trajectoryRange;
    private float nextYTrajectoryPosition;
    private float nextXTrajectoryPosition;
    private float nextPositionYCorrectionAbsolute;
    private float nextPositionXCorrectionAbsolute;

    // Target Reached Status
    private bool reachedTarget;
    private float distanceToTargetToDestroyProjectile = 1f;
    
    public void Init(Transform target, TowerSettings towerSettings, Transform projectileOwner)
    {
        this.target = target;
        this.projectileOwner = projectileOwner;
        
        maxMoveSpeed = towerSettings.projectileMaxMoveSpeed;
        trajectoryAnimationCurve = towerSettings.projectileCurve;
        projectileSpeedAnimationCurve = towerSettings.easingCurve;
        
        areaOfEffect = towerSettings.areaOfEffect;
        modifierAreaOfEffect = towerSettings.modifierAreaOfEffect;
        damage = towerSettings.damage;
        
        splashRadiusSprite = towerSettings.splashRadiusSprite;
        reachedTarget = false;

        float xDistanceToTarget = target.position.x - transform.position.x;
        trajectoryMaxRelativeHeight = Mathf.Abs(xDistanceToTarget) * towerSettings.projectileMaxHeight;
    }
    
    private void Start() 
    {
        trajectoryStartPoint = transform.position;
    }

    private void Update() 
    { 
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        if (!reachedTarget)
        {
            UpdateProjectilePosition();
            
            if (Vector3.Distance(transform.position, target.position) < distanceToTargetToDestroyProjectile)
            {
                reachedTarget = true;
                HandleTargetReached();
            }
        }
    }

    private void HandleTargetReached()
    {
        Instantiate(impactParticleSystem,
            transform.position,
            Quaternion.identity,
            SceneParentProvider.GetParent(SceneParentProviderType.PARTICLES));
        
        ApplyDamage();
        ApplyModifierEffect();
        
        if (areaOfEffect + modifierAreaOfEffect > 0) InstantiateSplashParticle();
        else projectileModifierEffect.ApplyEffectToTarget(target, projectileOwner);
        
        Destroy(gameObject);
    }

    private void ApplyDamage()
    {
        if (areaOfEffect > 0)
        {
            Collider2D[] affectedEnemies = Physics2D.OverlapCircleAll(target.position, areaOfEffect, layerMask);
            foreach (Collider2D enemy in affectedEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        else target.GetComponent<Enemy>().TakeDamage(damage);
    }

    private void ApplyModifierEffect()
    {
        if (modifierAreaOfEffect > 0)
        {
            Collider2D[] affectedTargets = Physics2D.OverlapCircleAll(target.position, modifierAreaOfEffect, layerMask);
            foreach (var affectedTarget in affectedTargets)
            {
                projectileModifierEffect.ApplyEffectToTarget(affectedTarget.transform, projectileOwner);
            }
        }
    }

    private void InstantiateSplashParticle()
    {
        GameObject splashParticle = Instantiate(radiusImpactParticleSystem,
            target.position,
            Quaternion.identity,
            SceneParentProvider.GetParent(SceneParentProviderType.PARTICLES));
        
        splashParticle.GetComponent<SplashParticleInitializer>().Play(splashRadiusSprite, areaOfEffect + modifierAreaOfEffect);
    }

    #region Movement Calculation
    private void UpdateProjectilePosition() 
    {
        if (target == null) 
        {
            Destroy(gameObject);
            return;
        }

        trajectoryRange = target.position - trajectoryStartPoint;

        if (Mathf.Abs(trajectoryRange.normalized.x) < Mathf.Abs(trajectoryRange.normalized.y))
        {
            if (trajectoryRange.y < 0) moveSpeed = -moveSpeed;
            UpdatePositionWithXCurve();
        }
        else
        {
            if (trajectoryRange.x < 0) moveSpeed = -moveSpeed;
            UpdatePositionWithYCurve();
        }
    }

    private void UpdatePositionWithXCurve() 
    {
        float nextPositionY = transform.position.y + moveSpeed * Time.deltaTime;
        float normalizedY = (nextPositionY - trajectoryStartPoint.y) / trajectoryRange.y;

        nextXTrajectoryPosition = trajectoryAnimationCurve.Evaluate(normalizedY) * trajectoryMaxRelativeHeight;
        nextPositionXCorrectionAbsolute = axisCorrectionAnimationCurve.Evaluate(normalizedY) * trajectoryRange.x;

        if (trajectoryRange.x > 0 && trajectoryRange.y > 0 ||
            trajectoryRange.x < 0 && trajectoryRange.y < 0)
        {
            nextXTrajectoryPosition = -nextXTrajectoryPosition;
        }

        Vector3 newPosition = new Vector3(
            trajectoryStartPoint.x + nextXTrajectoryPosition + nextPositionXCorrectionAbsolute,
            nextPositionY, 0
        );

        UpdateProjectileDirectionAndPosition(newPosition, normalizedY);
    }

    private void UpdatePositionWithYCurve() 
    {
        float nextPositionX = transform.position.x + moveSpeed * Time.deltaTime;
        float normalizedX = (nextPositionX - trajectoryStartPoint.x) / trajectoryRange.x;

        nextYTrajectoryPosition = trajectoryAnimationCurve.Evaluate(normalizedX) * trajectoryMaxRelativeHeight;
        nextPositionYCorrectionAbsolute = axisCorrectionAnimationCurve.Evaluate(normalizedX) * trajectoryRange.y;

        Vector3 newPosition = new Vector3(
            nextPositionX,
            trajectoryStartPoint.y + nextYTrajectoryPosition + nextPositionYCorrectionAbsolute, 0
        );

        UpdateProjectileDirectionAndPosition(newPosition, normalizedX);
    }

    private void UpdateProjectileDirectionAndPosition(Vector3 newPosition, float normalizedPosition)
    {
        CalculateNextProjectileSpeed(normalizedPosition);
        transform.position = newPosition;
    }

    private void CalculateNextProjectileSpeed(float normalizedPosition) 
    {
        moveSpeed = projectileSpeedAnimationCurve.Evaluate(normalizedPosition) * maxMoveSpeed;
    }
    #endregion
}