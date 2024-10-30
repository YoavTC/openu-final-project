using System;
using System.Collections;
using CodeMonkey.Utils;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Enemy : HealthBase
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

        EnemyManager.Instance.AddEnemy(this);
        OnDeathAction += enemyDeathListener;

        animateOnSpline.Init(currentSpline, enemySettings.speed, enemyReachEndListener, RemoveEnemyListener);
    }

    #region Damange & Health
    public override void TakeDamage(float amount)
    {
        UtilsClass.CreateWorldTextPopup(
            null,
            "-" + amount,
            transform.position,
            10,
            Color.red,
            transform.position + new Vector3(0, 5),
            0.5f);
        base.TakeDamage(amount);
    }

    protected override void Die()
    {
        //if (enemySettings.hasPostDeathEffect) PostDeathEffect();
        OnDeathAction?.Invoke(this);
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        //Wait for the Enemy manager to safely remove enemy from list
        bool callback = false;
        EnemyManager.Instance.RemoveEnemy(this, () => callback = true);
        yield return new WaitUntil(() => callback);

        // yield return new WaitForSeconds(.5f);
        transform.DOKill();
        transform.DOComplete();
        Destroy(gameObject);
    }

    //Used to calculate if a tower should target a soon-to-be dead enemy
    public void CalculateDamage(float damage)
    {
        float futureHealth = health - damage;
        if (futureHealth <= 0)
        {
            isDead = true;
        }
    }
    #endregion
    
    //Used from the AnimateOnSpline script to remove an enemy once it reaches the end
    private void RemoveEnemyListener() => StartCoroutine(DeathCoroutine());
    
    // private void PostDeathEffect()
    // {
    //     PostDeathEffect postDeathEffect = enemySettings.postDeathEffect;
    //     Transform[] targets;
    //     if (postDeathEffect.toFriendly)
    //     {
    //         Enemy[] towersInRadius = Utility.GetObjectsInRadius<Enemy>(transform.position, postDeathEffect.radius);
    //     }
    //     else
    //     {
    //         Tower[] towersInRadius = Utility.GetObjectsInRadius<Tower>(transform.position, postDeathEffect.radius);
    //     }
    // }
}
