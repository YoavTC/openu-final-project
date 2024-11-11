using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using External_Packages;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject enemyBasePrefab;
    [SerializeField] private Spline currentSpline;
    [SerializeField] private TMP_Text remainingEnemiesDisplay;
    
    [Foldout("Events")] public UnityEvent<Enemy> OnEnemyReachEndEvent;
    [Foldout("Events")] public UnityEvent<Enemy> OnEnemySpawnEvent;
    [Foldout("Events")] public UnityEvent<Enemy> OnEnemyDeathEvent;
    [Foldout("Events")] public UnityEvent OnSpawnerStopEvent;
    
    [Header("Dictionaries")]
    [SerializeField] private SerializedDictionary<EnemySettings, float> EnemyTypesWeightDictionary = new SerializedDictionary<EnemySettings, float>();
    [SerializeField] private SerializedDictionary<AnimationCurveObject, float> EnemyWaveTypesWeightDictionary = new SerializedDictionary<AnimationCurveObject, float>();
    private List<EnemySettings> enemySettingsList;

    [Header("Spawner Settings")] 
    [SerializeField] private float initialCooldown;
    [SerializeField] private float spawnCooldown;
    [SerializeField] [ReadOnly] private float elapsedTime;
    [SerializeField] [ReadOnly] private AnimationCurve currentSpawnWave;
    [SerializeField] [ReadOnly] private float currentSpawnWaveProgress;
    [SerializeField] [ReadOnly] private float nextSpawnDelay;
    
    [Header("Enemy Queue")]
    [SerializeField] private int enemyQueueLength;
    [SerializeField] [ReadOnly] private int[] enemyQueue;
    [SerializeField] [ReadOnly] private int enemyQueueIndex;
    private int originalRemainingEnemies;
    
    private void Start()
    {
        InitializeSpawner();
        InitializeUI();
    }
    
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        
        if (elapsedTime >= nextSpawnDelay && enemyQueue.Length > enemyQueueIndex)
        {
            nextSpawnDelay = GetNextSpawnDelay();
            elapsedTime = 0f;
            SpawnEnemy();
        }

        if (enemyQueueIndex >= enemyQueue.Length)
        {
            OnSpawnerStopEvent?.Invoke();
            Destroy(this);
        }
    }
    
    #region Initialization
    private void InitializeSpawner()
    {
        nextSpawnDelay = initialCooldown;
        enemySettingsList = EnemyTypesWeightDictionary.Keys.ToList();
        
        GenerateEnemyQueue();
        SetNewSpawnWave();
    }

    private void InitializeUI()
    {
        originalRemainingEnemies = enemyQueue.Length;
        UpdateRemainingEnemiesDisplay();
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
    }
    
    private AnimationCurve GetReversedAnimationCurve(AnimationCurve curve)
    {
        Keyframe[] keys = curve.keys;
        
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].value *= -1;
            keys[i].value += 1f;
        }
        
        return new AnimationCurve(keys);
    }

    private void SetNewSpawnWave()
    {
        currentSpawnWaveProgress = 0f;
        int randomSpawnWaveIndex = GetRandomByWeight(EnemyWaveTypesWeightDictionary);
        currentSpawnWave = GetReversedAnimationCurve(EnemyWaveTypesWeightDictionary.Keys.ToArray()[randomSpawnWaveIndex].curve);
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
    #endregion

    #region Spawning
    private void UpdateRemainingEnemiesDisplay()
    {
        remainingEnemiesDisplay.text = originalRemainingEnemies - enemyQueueIndex + "/" + originalRemainingEnemies;
    }

    private void SpawnEnemy()
    {
        EnemySettings newEnemySettings = enemySettingsList[enemyQueue[enemyQueueIndex]]; 
        enemyQueueIndex++;

        Enemy newEnemy = Instantiate(enemyBasePrefab,
            transform.position,
            Quaternion.identity
            ,InSceneParentProvider.GetParent(SceneParentProviderType.ENEMIES))
            .GetComponent<Enemy>();
        
        newEnemy.Init(newEnemySettings, EnemyReachEndListener, EnemyDeathListener, currentSpline);
        
        OnEnemySpawnEvent?.Invoke(newEnemy);
        UpdateRemainingEnemiesDisplay();
    }

    private float GetNextSpawnDelay()
    {
        currentSpawnWaveProgress += 1f / enemyQueue.Length;
        //return currentSpawnWave.Evaluate(currentSpawnWaveProgress);
        return Mathf.Clamp(currentSpawnWave.Evaluate(currentSpawnWaveProgress), 0.1f, 2f);
    }
    #endregion
    
    #region Events
    private void EnemyReachEndListener(Enemy enemy) => OnEnemyReachEndEvent?.Invoke(enemy);
    private void EnemyDeathListener(Enemy enemy) => OnEnemyDeathEvent?.Invoke(enemy);
    #endregion
}