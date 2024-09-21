using AYellowpaper.SerializedCollections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<EnemySettings, float> EnemyTypesWeightDictionary = new SerializedDictionary<EnemySettings, float>();
    [SerializeField] private SerializedDictionary<AnimationCurve, float> EnemyWaveTypesWeightDictionary = new SerializedDictionary<AnimationCurve, float>();
}