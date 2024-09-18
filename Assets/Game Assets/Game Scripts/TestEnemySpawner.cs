using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawner : MonoBehaviour
{
    [SerializeField] private Spline spline;
    [SerializeField] private GameObject testEnemy;
    [SerializeField] private float cooldown;
    [SerializeField] private float elapsedTime;

    [SerializeField] private EnemySettings[] enemyTypes;
    
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.P) && elapsedTime > cooldown)
        {
            elapsedTime = 0f;
            AnimateOnSpline animateOnSpline = Instantiate(testEnemy, transform.position, Quaternion.identity).GetComponent<AnimateOnSpline>();
            EnemySettings randomEnemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
            
            animateOnSpline.GetComponent<SpriteRenderer>().sprite = randomEnemyType.sprite;
            animateOnSpline.GetComponent<Enemy>().health = randomEnemyType.health;
            animateOnSpline.Init(spline, randomEnemyType.speed, EnemyReachedEnd);
        }
    }

    public void EnemyReachedEnd(Transform enemy)
    {
        Debug.Log("Enemy " + enemy + " reached end!");
    }
}
