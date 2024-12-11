using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Build")]
public class LevelBuildSO : ScriptableObject
{
    public int enemyCount;
    public int spawnsPerSpawn;
    public SerializedDictionary<EnemySettings, float> EnemyTypesWeightDictionary = new SerializedDictionary<EnemySettings, float>();
    public List<TowerSettings> towerBases = new List<TowerSettings>();
}