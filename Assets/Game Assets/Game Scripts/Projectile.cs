// Credit: UnPaws
// Source: https://www.youtube.com/watch?v=OPDl2uVaN_Q

using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float moveSpeed;
    private float maxMoveSpeed;
    private float trajectoryMaxRelativeHeight;
    private float distanceToTargetToDestroyProjectile = 1f;
    private Transform projectileOwner;

    private float damage;
    
    private AnimationCurve trajectoryAnimationCurve;
    private AnimationCurve axisCorrectionAnimationCurve = AnimationCurve.Linear(0,0,1,1);
    private AnimationCurve projectileSpeedAnimationCurve;
    
    private Vector3 trajectoryStartPoint;
    private Vector3 projectileMoveDir;
    private Vector3 trajectoryRange;
    
    private float nextYTrajectoryPosition;
    private float nextXTrajectoryPosition;
    private float nextPositionYCorrectionAbsolute;
    private float nextPositionXCorrectionAbsolute;

    private float areaOfEffect;
    private float modifierAreaOfEffect;

    private bool reachedTarget;
    
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private ProjectileModifierEffect projectileModifierEffect;

    [SerializeField] private GameObject impactParticleSystem;
    
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
        if (target == null) Destroy(gameObject);
        else if (!reachedTarget) {
            UpdateProjectilePosition();
            
            if (Vector3.Distance(transform.position, target.position) < distanceToTargetToDestroyProjectile)
            {
                reachedTarget = true;
                ReachedTarget();
            }
        }
    }

    private void ReachedTarget()
    {
        Instantiate(impactParticleSystem, transform.position, Quaternion.identity);
        if (areaOfEffect > 0)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(target.position, areaOfEffect, layerMask);
            for (int i = 0; i < collider2Ds.Length; i++)
            {
                collider2Ds[i].GetComponent<Enemy>().TakeDamage(damage);
            }
        } else target.GetComponent<Enemy>().TakeDamage(damage);
        
        if (modifierAreaOfEffect > 0)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(target.position, modifierAreaOfEffect, layerMask);
            for (int i = 0; i < collider2Ds.Length; i++)
            {
                projectileModifierEffect.ApplyEffectToTarget(collider2Ds[i].transform, projectileOwner);
            }
        }
        if ((areaOfEffect + modifierAreaOfEffect) > 0)
        {
            
        } else projectileModifierEffect.ApplyEffectToTarget(target, projectileOwner);
        
        Destroy(gameObject);
    }


    private void UpdateProjectilePosition() 
    {
        if (target == null) Destroy(gameObject);
        else {
            trajectoryRange = target.position - trajectoryStartPoint;


            if(Mathf.Abs(trajectoryRange.normalized.x) < Mathf.Abs(trajectoryRange.normalized.y)) 
            {
                // Projectile will be curved on the X axis
                if (trajectoryRange.y < 0) {
                    // Target is located under shooter
                    moveSpeed = -moveSpeed;
                }
                UpdatePositionWithXCurve();
                
            } else {
                // Projectile will be curved on the Y axis
                
                if (trajectoryRange.x < 0) {
                    // Target is located behind shooter
                    moveSpeed = -moveSpeed;
                }
                UpdatePositionWithYCurve();
            }
        }
    }


    private void UpdatePositionWithXCurve() 
    {
        float nextPositionY = transform.position.y + moveSpeed * Time.deltaTime;
        float nextPositionYNormalized = (nextPositionY - trajectoryStartPoint.y) / trajectoryRange.y;
        
        float nextPositionXNormalized = trajectoryAnimationCurve.Evaluate(nextPositionYNormalized);
        nextXTrajectoryPosition = nextPositionXNormalized * trajectoryMaxRelativeHeight;
        
        float nextPositionXCorrectionNormalized = axisCorrectionAnimationCurve.Evaluate(nextPositionYNormalized);
        nextPositionXCorrectionAbsolute = nextPositionXCorrectionNormalized * trajectoryRange.x;


        if (trajectoryRange.x > 0 && trajectoryRange.y > 0) 
        {
            nextXTrajectoryPosition = -nextXTrajectoryPosition;
        }
        
        if (trajectoryRange.x < 0 && trajectoryRange.y < 0) 
        {
            nextXTrajectoryPosition = -nextXTrajectoryPosition;
        }

        float nextPositionX = trajectoryStartPoint.x + nextXTrajectoryPosition + nextPositionXCorrectionAbsolute;
        Vector3 newPosition = new Vector3(nextPositionX, nextPositionY, 0);
        CalculateNextProjectileSpeed(nextPositionYNormalized);
        
        projectileMoveDir = newPosition - transform.position;
        transform.position = newPosition;
    }


    private void UpdatePositionWithYCurve() 
    {
        float nextPositionX = transform.position.x + moveSpeed * Time.deltaTime;
        float nextPositionXNormalized = (nextPositionX - trajectoryStartPoint.x) / trajectoryRange.x;
        
        float nextPositionYNormalized = trajectoryAnimationCurve.Evaluate(nextPositionXNormalized);
        nextYTrajectoryPosition = nextPositionYNormalized * trajectoryMaxRelativeHeight;
        float nextPositionYCorrectionNormalized = axisCorrectionAnimationCurve.Evaluate(nextPositionXNormalized);
        nextPositionYCorrectionAbsolute = nextPositionYCorrectionNormalized * trajectoryRange.y;
        float nextPositionY = trajectoryStartPoint.y + nextYTrajectoryPosition + nextPositionYCorrectionAbsolute;
        
        Vector3 newPosition = new Vector3(nextPositionX, nextPositionY, 0);
        CalculateNextProjectileSpeed(nextPositionXNormalized);
        
        projectileMoveDir = newPosition - transform.position;
        transform.position = newPosition;
    }


    private void CalculateNextProjectileSpeed(float nextPositionXNormalized) 
    {
        float nextMoveSpeedNormalized = projectileSpeedAnimationCurve.Evaluate(nextPositionXNormalized);
        moveSpeed = nextMoveSpeedNormalized * maxMoveSpeed;
    }
}
