using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
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
}