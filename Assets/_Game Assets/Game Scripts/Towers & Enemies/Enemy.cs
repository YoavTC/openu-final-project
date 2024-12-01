using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Enemy : EntityBase
{
    [Header("Settings & Components")]
    [SerializeField] private EnemySettings _enemySettings;
    public EnemySettings enemySettings
    {
        get => _enemySettings;
        private set => _enemySettings = value;
    }
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AnimateOnSpline animateOnSpline;
    
    [Header("Events & Actions")]
    private Action<Enemy> OnDeathAction;
    
    public void Init(EnemySettings enemySettings, Action<Enemy> enemyReachEndListener, Action<Enemy> enemyDeathListener, Spline currentSpline)
    {
        _enemySettings = enemySettings;
        spriteRenderer.sprite = enemySettings.sprite;
        SetHealth(enemySettings.health);

        EnemyManager.Instance.AddEntity(this);
        OnDeathAction += enemyDeathListener;

        animateOnSpline.Init(currentSpline, enemySettings.speed, enemyReachEndListener, RemoveEnemyListener);

        if (enemySettings.towerSettings)
        {
            gameObject.AddComponent<EnemyShooter>()
                .InitializeComponents(enemySettings.towerSettings);
        }
    }

    #region Damange & Health
    // public override void TakeDamage(float amount)
    // {
    //     UtilsClass.CreateWorldTextPopup(
    //         null,
    //         "-" + amount,
    //         transform.position,
    //         10,
    //         Color.red,
    //         transform.position + new Vector3(0, 5),
    //         0.5f);
    //     base.TakeDamage(amount);
    // }

    protected override void Die()
    {
        ParticlesManager.Instance.PlayEnemyDeath(transform.position, spriteRenderer);
        if (enemySettings.postDeathModifierEffect) PostDeathModifierEffect();
        OnDeathAction?.Invoke(this);
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        //Wait for the Enemy manager to safely remove enemy from list
        bool callback = false;
        EnemyManager.Instance.RemoveEntity(this, () => callback = true);
        yield return new WaitUntil(() => callback);

        // yield return new WaitForSeconds(.5f);
        transform.DOKill();
        transform.DOComplete();
        Destroy(gameObject);
    }
    #endregion
    
    //Used from the AnimateOnSpline script to remove an enemy once it reaches the end
    private void RemoveEnemyListener() => StartCoroutine(DeathCoroutine());
    
    private void PostDeathModifierEffect()
    {
        PostDeathModifierEffect postDeathModifierEffect = enemySettings.postDeathModifierEffect;
        List<EntityBase> targets = new List<EntityBase>();
        
        if (postDeathModifierEffect.toFriendly)
        {
            targets.AddRange(Utility.GetObjectsInRadius<Enemy>(transform.position, postDeathModifierEffect.radius));
        }
        else
        {
            targets.AddRange(Utility.GetObjectsInRadius<TowerBase>(transform.position, postDeathModifierEffect.radius));
        }

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].StartEffect(postDeathModifierEffect.type, transform);
        }
        
        ParticlesManager.Instance.PlayHitRadius(transform.position, enemySettings);
    }
    
    protected override void ApplyEffect(ModifierEffectType type, float amount)
    {
        switch (type)
        {
            case ModifierEffectType.HEALTH:
                TakeDamage(amount);
                break;
            case ModifierEffectType.SPEED:
                animateOnSpline.speed = amount * enemySettings.speed;
                break;
            default:
                return;
        }
    }

    public override void FinishEffect()
    {
        animateOnSpline.speed = enemySettings.speed;
        base.FinishEffect();
    }
}
