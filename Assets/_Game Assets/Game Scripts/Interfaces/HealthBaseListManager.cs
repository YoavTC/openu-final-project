﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

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
        {
            entityList.Add(entity);
            EntityAddedUnityEvent?.Invoke(entity);
        }
        else
        {
            entityList.Remove(entity);
            EntityRemovedUnityEvent?.Invoke(entity);
        }

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
            float dist = (invokerTransform.position - entityList[i].transform.position).sqrMagnitude;

            if (dist < closestDistance && dist <= sqrMaxRange)
            {
                closestDistance = dist;
                closestEntity = entityList[i];
            }
        }

        isLooping = false;
        return closestEntity;
    }

    public virtual HealthBase[] GetClosestEntities(Transform invokerTransform, float maxRange, int count)
    {
        if (entityList.Count <= 0 || count <= 0) return null;

        float sqrMaxRange = maxRange * maxRange;
        List<HealthBase> candidates = new List<HealthBase>();

        isLooping = true;
        for (int i = 0; i < entityList.Count; i++)
        {
            if (entityList[i].isDead || entityList[i].transform == invokerTransform) continue;

            float dist = (invokerTransform.position - entityList[i].transform.position).sqrMagnitude;
            if (dist <= sqrMaxRange)
            {
                candidates.Add(entityList[i]);
            }
        }
        isLooping = false;

        // Sort the candidates by distance
        candidates.Sort((a, b) =>
        {
            float distA = (invokerTransform.position - a.transform.position).sqrMagnitude;
            float distB = (invokerTransform.position - b.transform.position).sqrMagnitude;
            return distA.CompareTo(distB);
        });

        // Return the closest 'count' entities
        return candidates.Take(count).ToArray();
    }
    
    // Get the closest entity within the specified range
    public virtual HealthBase GetClosestHurtEntity(Transform invokerTransform, float maxRange)
    {
        if (entityList.Count <= 0) return null;

        HealthBase closestEntity = null;
        float closestDistance = Mathf.Infinity;
        float sqrMaxRange = maxRange * maxRange;

        isLooping = true;
        for (int i = 0; i < entityList.Count; i++)
        {
            if (entityList[i].transform == invokerTransform) continue;
            if (entityList[i].isDead || entityList[i].health == entityList[i].maxHealth) continue;
            
            float dist = (invokerTransform.position - entityList[i].transform.position).sqrMagnitude;

            if (dist < closestDistance && dist <= sqrMaxRange)
            {
                closestDistance = dist;
                closestEntity = entityList[i];
            }
        }

        isLooping = false;
        return closestEntity;
    }

    
    [Foldout("Events")] public UnityEvent<HealthBase> EntityAddedUnityEvent;
    [Foldout("Events")] public UnityEvent<HealthBase> EntityRemovedUnityEvent;
}