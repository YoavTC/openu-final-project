using System;
using System.Collections;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : HealthBase
{
    [SerializeField] private EnemySettings _enemySettings;
    public EnemySettings enemySettings
    {
        get => _enemySettings;
        private set => _enemySettings = value;
    }
    
    private Action<Enemy> OnDeathAction;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AnimateOnSpline animateOnSpline;

    public void Init(EnemySettings enemySettings, Action<Enemy> enemyReachEndListener, Action<Enemy> enemyDeathListener,Spline currentSpline)
    {
        _enemySettings = enemySettings;
        
        EnemyManager.Instance.AddEnemy(this);
        OnDeathAction += enemyDeathListener;
        
        animateOnSpline.Init(currentSpline, enemySettings.speed, enemyReachEndListener, RemoveEnemyListener);

        spriteRenderer.sprite = enemySettings.sprite;
        health = enemySettings.health;
    }

    public override void TakeDamage(float amount)
    {
        UtilsClass.CreateWorldTextPopup(
            null,
            "-" + amount,
            transform.position,
            10,
            Color.red,
            transform.position + new Vector3(0, 20),
            2f);
        base.TakeDamage(amount);
    }
    
    protected override void Die()
    {
        if (enemySettings.hasPostDeathEffect) PostDeathEffect();
        OnDeathAction?.Invoke(this);
        StartCoroutine(DeathCoroutine());
    }

    private void PostDeathEffect()
    {
        PostDeathEffect postDeathEffect = enemySettings.postDeathEffect;
        Transform[] targets;
        if (postDeathEffect.toFriendly)
        {
            Enemy[] towersInRadius = Utility.GetObjectsInRadius<Enemy>(transform.position, postDeathEffect.radius);
        }
        else
        {
            Tower[] towersInRadius = Utility.GetObjectsInRadius<Tower>(transform.position, postDeathEffect.radius);
        }
    }

    //Used from the AnimateOnSpline script to remove an enemy once it reaches the end
    private void RemoveEnemyListener()
    {
        Debug.Log("Enemy reached end of path!");
        StartCoroutine(DeathCoroutine());
    }
    
    private IEnumerator DeathCoroutine()
    {
        //Wait for the Enemy manager to safely remove enemy from list
        bool callback = false;
        EnemyManager.Instance.RemoveEnemy(this, () => callback = true);
        yield return new WaitUntil(() => callback);
        
        Destroy(gameObject);
    }

    public void CalculateDamage(float damage)
    {
        float futureHealth = health - damage;
        if (futureHealth <= 0)
        {
            isDead = true;
        }
    }
}
