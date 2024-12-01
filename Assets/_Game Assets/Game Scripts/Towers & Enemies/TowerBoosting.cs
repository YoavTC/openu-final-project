using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TowerBoosting : TowerBase
{
    private HealthBaseListManager towerManager;
    private Transform[] closestTowers;
    
    public override void InitializeComponents(TowerSettings towerSettings)
    {
        towerManager = TowerManager.Instance;
        base.InitializeComponents(towerSettings);
    }

    private void Start()
    {
        boostBeamPrefab = Utility.GetBoostBeamPrefab();
        PopulateLineRenderers();
    }

    protected override void FindNextTarget()
    {
        HealthBase[] closestEntities = towerManager.GetClosestEntities(transform, towerSettings.maxRange, towerSettings.boostingCount);
        if (closestEntities != null)
        {
            closestTowers = closestEntities.Select(a => a.transform).ToArray();
        }
    }

    protected override void CooldownAction()
    {
        FindNextTarget();
        if (closestTowers.Length > 0) Boost();
    }

    private void LateUpdate()
    {
        SetupLineRenderers();
    }

    private void Boost()
    {
        if (LineRenderersOrClosestTowersInvalid()) return;
        for (int i = 0; i < closestTowers.Length; i++)
        {
            closestTowers[i].GetComponent<TowerBase>().StartEffect(towerSettings.projectileModifierEffect, transform);
        }
    }

    private LineRenderer[] lineRenderers;
    private LineRenderer boostBeamPrefab;

    // TODO CHECK IF WORKS AT ALL
    
    private void PopulateLineRenderers()
    {
        lineRenderers = new LineRenderer[towerSettings.boostingCount];
        
        for (int i = 0; i < towerSettings.boostingCount; i++)
        {
            lineRenderers[i] = Instantiate(boostBeamPrefab, transform);
            lineRenderers[i].positionCount = 2;
            lineRenderers[i].SetPosition(0, transform.position);
            lineRenderers[i].SetPosition(1, transform.position);
        }
        
    }

    private void SetupLineRenderers()
    {
        // if (LineRenderersOrClosestTowersInvalid()) return;
        if ((lineRenderers == null) || (closestTowers == null)) return;
        for (int i = 0; i < towerSettings.boostingCount; i++)
        {
            if (closestTowers.Length > i)
            {
                lineRenderers[i].SetPosition(1, closestTowers[i].transform.position);
            } else lineRenderers[i].SetPosition(1, transform.position);
        }
    }

    private bool LineRenderersOrClosestTowersInvalid()
    {
        if ((lineRenderers == null) || (closestTowers == null)) return true;
        if (lineRenderers.Length == 0 || closestTowers.Length == 0) return true;
        return false;
    }
}
