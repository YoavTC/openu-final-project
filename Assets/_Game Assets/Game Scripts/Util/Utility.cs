using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Utility : MonoBehaviour
{
    private void Awake()
    {
        // Assign the serialized field to the static variable
        projectilePrefab = _projectilePrefab;
        boostBeamPrefab = _boostBeamPrefab;
    }
    
    public static T[] GetObjectsInRadius<T>(Vector2 center, float radius) where T : MonoBehaviour
    {
        List<T> objects = new List<T>();
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(center, radius);
    
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            if (collider2Ds[i].TryGetComponent(out T obj))
            {
                objects.Add(obj);
            }
        }

        return objects.ToArray();
    }

    
    [SerializeField] private Projectile _projectilePrefab;
    private static Projectile projectilePrefab { get; set; }

    public static Projectile GetProjectilePrefab()
    {
        return projectilePrefab;
    }
    
    [SerializeField] private LineRenderer _boostBeamPrefab;
    private static LineRenderer boostBeamPrefab { get; set; }

    public static LineRenderer GetBoostBeamPrefab()
    {
        return boostBeamPrefab;
    }
}