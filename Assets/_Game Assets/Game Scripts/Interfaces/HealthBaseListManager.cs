using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthBaseListManager : MonoBehaviour
{
    private bool isLooping;
    [SerializeField] private List<HealthBase> entityList = new List<HealthBase>();

    // Add an entity to the list
    public void AddEntity(HealthBase entity) => StartCoroutine(ModifyEntityList(entity, true));

    // Remove an entity from the list and run a callback
    public void RemoveEntity(HealthBase entity, Action callback) => StartCoroutine(ModifyEntityList(entity, false, callback));

    // Modify the entity list, either adding or removing an entity
    private IEnumerator ModifyEntityList(HealthBase entity, bool add, Action callback = null)
    {
        yield return new WaitUntil(() => !isLooping);
        if (add) 
            entityList.Add(entity);
        else 
            entityList.Remove(entity);

        callback?.Invoke();
    }

    // Check if all entities are dead
    public bool AllEntitiesDead() => entityList.Count <= 0;

    // Get the closest entity within the specified range
    public virtual HealthBase GetClosestEntity(Transform invokerTransform, float maxRange)
    {
        if (entityList.Count <= 0) return null;

        HealthBase closestEntity = null;
        float closestDistance = Mathf.Infinity;
        float sqrMaxRange = maxRange * maxRange;

        isLooping = true;
        for (int i = 0; i < entityList.Count; i++)
        {
            if (entityList[i].isDead || entityList[i].transform == invokerTransform) continue;
            Transform entityTransform = entityList[i].transform;
            float dist = (invokerTransform.position - entityTransform.position).sqrMagnitude;

            if (dist < closestDistance && dist <= sqrMaxRange)
            {
                closestDistance = dist;
                closestEntity = entityList[i];
            }
        }

        isLooping = false;
        return closestEntity;
    }
}