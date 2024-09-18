using System;
using System.Collections;
using External_Packages;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TestEnemySpawner : MonoBehaviour
{
    [SerializeField] private Spline spline;
    [SerializeField] private GameObject testEnemy;
    [SerializeField] private float cooldown;
    [SerializeField] private float elapsedTime;

    [SerializeField] private TMP_Text enemyBankDisplay;
    public int enemiesLeft = 400;

    [SerializeField] private EnemySettings[] enemyTypes;

    private IEnumerator Start()
    {
        float tempCooldown = cooldown;
        cooldown = 10000f;
        yield return HelperFunctions.GetWait(2f);
        elapsedTime = 0f;
        cooldown = tempCooldown;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > cooldown && enemiesLeft > 0)
        {
            if (enemiesLeft == 150)
            {
                cooldown /= 2;
            }
            enemiesLeft--;
            enemyBankDisplay.text = enemiesLeft + "/500";
            elapsedTime = 0f;
            AnimateOnSpline animateOnSpline = Instantiate(testEnemy, transform.position, Quaternion.identity).GetComponent<AnimateOnSpline>();
            EnemySettings randomEnemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
            
            animateOnSpline.GetComponent<SpriteRenderer>().sprite = randomEnemyType.sprite;
            animateOnSpline.GetComponent<Enemy>().health = randomEnemyType.health;
            animateOnSpline.Init(spline, randomEnemyType.speed, EnemyReachedEnd);
        }
    }

    [SerializeField] private GameObject deathScreen;

    public void EnemyReachedEnd(Transform enemy)
    {
        bool canAfford = ElixirManager.Instance.TryAffordOperation(Mathf.RoundToInt(enemy.GetComponent<Enemy>().health * 2.5f));
        if (!canAfford)
        {
            Debug.Log("Died!");
            deathScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        Debug.Log("Enemy " + enemy + " reached end!");
        StartCoroutine(enemy.GetComponent<Enemy>().DeathCoroutine());
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
