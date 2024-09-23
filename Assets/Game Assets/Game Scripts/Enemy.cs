using System;
using System.Collections;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySettings _enemySettings;
    private Action<Enemy> OnDeathAction;

    public EnemySettings enemySettings
    {
        get
        {
            return _enemySettings;
        }
        private set
        {
            _enemySettings = value;
        }
    }
    
    private float health;
    public bool isDead;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AnimateOnSpline animateOnSpline;

    public void Init(EnemySettings enemySettings, Action<Enemy> enemyReachEndListener, Action<Enemy> enemyDeathListener,Spline currentSpline)
    {
        _enemySettings = enemySettings;
        
        EnemyManager.Instance.AddEnemy(this);
        OnDeathAction += enemyDeathListener;
        
        animateOnSpline.Init(currentSpline, enemySettings.speed, enemyReachEndListener);

        spriteRenderer.sprite = enemySettings.sprite;
        health = enemySettings.health;
    }

    public void TakeDamage(float damage) => ApplyDamage(damage, Vector3.zero);
    public void TakeDamage(float damage, Vector3 dir) => ApplyDamage(damage, dir);
    
    private void ApplyDamage(float damage, Vector3 dir)
    {
        UtilsClass.CreateWorldTextPopup(
            null,
            "-" + damage,
            transform.position,
            10,
            Color.red,
            transform.position + new Vector3(0, 20),
            2f);
        
        if (health - damage <= 0)
        {
            KillEnemy(this);
        }
        else
        {
            health -= damage;
        } 
    }

    private void KillEnemy(Enemy enemy)
    {
        OnDeathAction?.Invoke(enemy);
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
