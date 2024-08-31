using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawner : MonoBehaviour
{
    [SerializeField] private Spline spline;
    [SerializeField] private GameObject testEnemy;
    [SerializeField] private float cooldown;
    [SerializeField] private float elapsedTime;
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.P) && elapsedTime > cooldown)
        {
            elapsedTime = 0f;
            AnimateOnSpline animateOnSpline = Instantiate(testEnemy, transform.position, Quaternion.identity).GetComponent<AnimateOnSpline>();
            animateOnSpline.Init(spline, 5f, EnemyReachedEnd);
        }
    }

    public void EnemyReachedEnd(Transform enemy)
    {
        Debug.Log("Enemy " + enemy + " reached end!");
    }
}
