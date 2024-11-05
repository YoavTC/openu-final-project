using System;
using System.Collections;
using System.Collections.Generic;
using External_Packages;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private bool isLooping;
    [SerializeField] private List<Enemy> enemyList = new List<Enemy>();
    
    public void AddEnemy(Enemy enemy) => StartCoroutine(ModifyEnemyList(enemy, true));
    public void RemoveEnemy(Enemy enemy, Action callback) => StartCoroutine(ModifyEnemyList(enemy, false, callback));
    
    private IEnumerator ModifyEnemyList(Enemy enemy, bool add, Action callback = null)
    {
        yield return new WaitUntil(() => !isLooping);
        if (add) enemyList.Add(enemy);
        else enemyList.Remove(enemy);
        
        callback?.Invoke();
    }

    public bool AllEnemiesDead() => enemyList.Count <= 0;
    
    public Enemy GetClosestEnemy(Vector3 position, float maxRange)
    {
        if (enemyList.Count <= 0) return null;
        
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        float sqrMaxRange = maxRange * maxRange;
        
        isLooping = true;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].isDead) continue;
            Transform enemyTransform = enemyList[i].transform;
            float dist = (position - enemyTransform.position).sqrMagnitude;
            if (dist < closestDistance && dist <= sqrMaxRange)
            {
                closestDistance = dist;
                closestEnemy = enemyList[i];
            }
        }
        isLooping = false;
        
        return closestEnemy;
    }
}
