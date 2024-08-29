using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawner : MonoBehaviour
{
    [SerializeField] private AnimateOnSpline testEnemy;
    [SerializeField] private float cooldown;
    private float elapsedTime;
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.P) && elapsedTime >= cooldown)
        {
            elapsedTime = 0f;
            AnimateOnSpline animateOnSpline = Instantiate(testEnemy, transform.position, Quaternion.identity);
            animateOnSpline.Init(GetComponent<Spline>(), 5f, EnemyReachedEnd);
        }
    }

    public void EnemyReachedEnd(Transform enemy)
    {
        Debug.Log("Enemy " + enemy + " reached end!");
    }
}
