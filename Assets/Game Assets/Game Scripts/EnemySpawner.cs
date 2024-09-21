using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using External_Packages;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject enemyBasePrefab;
    [SerializeField] private Spline currentSpline;
    
    [Header("Events")]
    public UnityEvent<Enemy> OnEnemyReachEndEvent;
    public UnityEvent<Enemy> OnEnemySpawnEvent;
    
    [Header("Dictionaries")]
    [SerializeField] private SerializedDictionary<EnemySettings, float> EnemyTypesWeightDictionary = new SerializedDictionary<EnemySettings, float>();
    [SerializeField] private SerializedDictionary<AnimationCurveObject, float> EnemyWaveTypesWeightDictionary = new SerializedDictionary<AnimationCurveObject, float>();
    private List<EnemySettings> enemySettingsList = new List<EnemySettings>();

    [Header("Settings")] 
    [SerializeField] private float startDelay;
    [SerializeField] [ReadOnly] private bool spawnerActive;

    [Header("Spawner Settings")] 
    [SerializeField] private float spawnCooldown;
    [SerializeField] [ReadOnly] private float elapsedTime;
    [SerializeField] [ReadOnly] private AnimationCurveObject currentSpawnWave;
    [SerializeField] [ReadOnly] private float currentSpawnWaveProgress;
    
    [Header("Enemy Queue")]
    [SerializeField] private int enemyQueueLength;
    [SerializeField] [ReadOnly] private int[] enemyQueue;
    [SerializeField] [ReadOnly] private int enemyQueueIndex;
    
    private IEnumerator Start()
    {
        spawnerActive = false;

        enemySettingsList = EnemyTypesWeightDictionary.Keys.ToList();
        GenerateEnemyQueue();
        SetNewSpawnWave();
        
        yield return HelperFunctions.GetWait(startDelay);
        spawnerActive = true;
    }

    private void Update()
    {
        if (!spawnerActive) return;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spawnCooldown)
        {
            elapsedTime = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        EnemySettings newEnemySettings = enemySettingsList[enemyQueueIndex]; 
        enemyQueueIndex++;

        AnimateOnSpline newEnemy = Instantiate(enemyBasePrefab, transform.position, Quaternion.identity).GetComponent<AnimateOnSpline>();
        newEnemy.Init(currentSpline, newEnemySettings.speed, OnEnemyReachEndCallback);
        
        OnEnemySpawnEvent?.Invoke(newEnemy.GetComponent<Enemy>());
    }

    private void OnEnemyReachEndCallback(Enemy enemy)
    {
        OnEnemyReachEndEvent?.Invoke(enemy);
    }

    private void GenerateEnemyQueue()
    {
        enemyQueue = new int[enemyQueueLength];
        enemyQueueIndex = 0;
        string readableQueue = "";

        for (int i = 0; i < enemyQueue.Length; i++)
        {
            enemyQueue[i] = GetRandomByWeight(EnemyTypesWeightDictionary);
            readableQueue += enemyQueue[i].ToString();
        }

        Debug.Log("Readable Queue: " + readableQueue);
    }

    private void SetNewSpawnWave()
    {
        currentSpawnWaveProgress = 0f;
        int randomSpawnWaveIndex = GetRandomByWeight(EnemyWaveTypesWeightDictionary);
        currentSpawnWave = EnemyWaveTypesWeightDictionary.Keys.ToArray()[randomSpawnWaveIndex];
    }
    
    private int GetRandomByWeight<TKey>(Dictionary<TKey, float> dict)
    {
        // Get the sum of all the weights
        float totalWeight = dict.Values.Sum();

        // Generate a random number in the range [0, totalWeight)
        Random random = new Random();
        float randomValue = (float)(random.NextDouble() * totalWeight);

        float cumulativeWeight = 0.0f;
        for (int i = 0; i < dict.Count; i++)
        {
            cumulativeWeight += dict.ElementAt(i).Value;
            if (randomValue < cumulativeWeight)
            {
                return i;  // Return the index of the selected KeyValuePair
            }
        }

        return -1;
    }
}