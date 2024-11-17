// Original Logic: UnPaws
// Source: https://www.youtube.com/watch?v=OPDl2uVaN_Q

using NaughtyAttributes;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Initialized references
    [SerializeField] [ReadOnly] private Transform target;
    [SerializeField] [ReadOnly] private Transform projectileOwner;
    private TowerSettings towerSettings;

    // Speed & Trajectory Settings
    private float speed;
    private readonly AnimationCurve correctionCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
    // Trajectory Calculations
    private float maxRelativeHeight;
    private Vector3 startPoint;
    private Vector3 rangeToTarget;
    private float correctedYPosition;
    private float correctedXPosition;
    private float absoluteCorrectionY;
    private float absoluteCorrectionX;

    // Target Reached Status
    private readonly float destroyDistanceThreshold = 1f;
    
    public void InitializeProjectile(Transform newTarget, Transform newProjectileOwner, TowerSettings newTowerSettings)
    {
        target = newTarget;
        projectileOwner = newProjectileOwner;
        towerSettings = newTowerSettings;
        
        maxRelativeHeight = Mathf.Abs(newTarget.position.x - transform.position.x) * towerSettings.projectileMaxHeight;
        startPoint = transform.position;
    }
  
    private void Update() 
    { 
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        CalculateNextProjectilePosition();
        
        if (Vector3.Distance(transform.position, target.position) < destroyDistanceThreshold)
        {
            HandleImpact();
        }
    }

    private void HandleImpact()
    {
        ApplyDamage();
        ApplyModifierEffect();
        
        SpawnSplashParticles();
        SpawnImpactParticles();
        
        target = null;
    }

    private void ApplyDamage()
    {
        if (towerSettings.areaOfEffect > 0)
        {
            Collider2D[] affectedEnemies = Physics2D.OverlapCircleAll(target.position, towerSettings.areaOfEffect, towerSettings.targetedLayerMask);
            foreach (Collider2D entity in affectedEnemies)
            {
                entity.GetComponent<HealthBase>().TakeDamage(towerSettings.damage);
            }
        }
        else target.GetComponent<HealthBase>().TakeDamage(towerSettings.damage);
    }

    private void ApplyModifierEffect()
    {
        if (towerSettings.modifierAreaOfEffect > 0)
        {
            Collider2D[] affectedTargets = Physics2D.OverlapCircleAll(target.position, towerSettings.modifierAreaOfEffect, towerSettings.targetedLayerMask);
            foreach (var affectedTarget in affectedTargets)
            {
                affectedTarget.GetComponent<EntityBase>().StartEffect(towerSettings.projectileModifierEffect, projectileOwner);
            }
        }
    }
    
    #region Particles
    private void SpawnImpactParticles()
    {
        ParticlesManager.Instance.PlayProjectileDestroy(transform.position);
    }

    private void SpawnSplashParticles()
    {
        if (towerSettings.areaOfEffect + towerSettings.modifierAreaOfEffect > 0)
        {
            ParticlesManager.Instance.PlayHitRadius(transform.position, towerSettings);
        }
    }
    #endregion

    #region Movement Calculation
    private void UpdateProjectilePosition(Vector3 newPosition, float normalizedPosition)
    {
        speed = GetNextProjectileSpeed(normalizedPosition);
        transform.position = newPosition;
    }
    
    private void CalculateNextProjectilePosition() 
    {
        if (target == null) 
        {
            Destroy(gameObject);
            return;
        }

        rangeToTarget = (target.position - startPoint);
        
        if (Mathf.Abs(rangeToTarget.normalized.x) < Mathf.Abs(rangeToTarget.normalized.y))
        {
            if (rangeToTarget.y < 0) speed = -speed;
            CalculatePositionUsingXCurve();
        } 
        else 
        {
            if (rangeToTarget.x < 0) speed = -speed;
            CalculatePositionUsingYCurve();
        }
    }

    private void CalculatePositionUsingXCurve() 
    {
        float nextY = transform.position.y + speed * Time.deltaTime;
        float normalizedY = (nextY - startPoint.y) / rangeToTarget.y;

        correctedXPosition = towerSettings.projectileCurve.Evaluate(normalizedY) * maxRelativeHeight;
        absoluteCorrectionX = correctionCurve.Evaluate(normalizedY) * rangeToTarget.x;

        if (rangeToTarget.x * rangeToTarget.y > 0)
        {
            correctedXPosition = -correctedXPosition;
        }

        Vector3 newPosition = new Vector3(
            startPoint.x + correctedXPosition + absoluteCorrectionX,
            nextY, 0
        );

        UpdateProjectilePosition(newPosition, normalizedY);
    }

    private void CalculatePositionUsingYCurve() 
    {
        float nextX = transform.position.x + speed * Time.deltaTime;
        float normalizedX = (nextX - startPoint.x) / rangeToTarget.x;

        correctedYPosition = towerSettings.projectileCurve.Evaluate(normalizedX) * maxRelativeHeight;
        absoluteCorrectionY = correctionCurve.Evaluate(normalizedX) * rangeToTarget.y;

        Vector3 newPosition = new Vector3(
            nextX,
            startPoint.y + correctedYPosition + absoluteCorrectionY, 0
        );

        UpdateProjectilePosition(newPosition, normalizedX);
    }

    private float GetNextProjectileSpeed(float normalizedPosition) 
    {
        return towerSettings.easingCurve.Evaluate(normalizedPosition) * towerSettings.projectileMaxMoveSpeed;
    }
    #endregion
}