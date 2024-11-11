using System;
using System.Collections;
using UnityEngine;

public class InSceneParentProvider : MonoBehaviour
{
    private static Transform enemiesParent { get; set; }
    private static Transform projectilesParent { get; set; }
    private static Transform particlesParent { get; set; }
    private static Transform towersParent { get; set; }
    
    [SerializeField] private Transform _enemiesParent;
    [SerializeField] private Transform _projectilesParent;
    [SerializeField] private Transform _particlesParent;
    [SerializeField] private Transform _towersParent;
    
    private void Awake()
    {
        enemiesParent = _enemiesParent;
        projectilesParent = _projectilesParent;
        particlesParent = _particlesParent;
        towersParent = _towersParent;
    }

    public static Transform GetParent(SceneParentProviderType type)
    {
        switch (type)
        {
            case SceneParentProviderType.ENEMIES: return enemiesParent;
            case SceneParentProviderType.PROJECTILES: return projectilesParent;
            case SceneParentProviderType.PARTICLES: return particlesParent;
            case SceneParentProviderType.TOWERS: return towersParent;
        }

        return null;
    }

    [Space] 
    [SerializeField] private float childCountRenameCooldown;

    #if UNITY_EDITOR
    private IEnumerator Start()
    {
        GameObject[] parents =
        {
            enemiesParent.gameObject,
            projectilesParent.gameObject,
            particlesParent.gameObject,
            towersParent.gameObject
        };
        
        for (int i = 0; i < parents.Length; i++)
        {
            parents[i].name = "[" + Mathf.Min(parents[i].transform.childCount, 99).ToString("D2") + "] " + parents[i].name;
        }
        
        WaitForSeconds cooldown = new WaitForSeconds(childCountRenameCooldown);
        while (true)
        {
            for (int i = 0; i < parents.Length; i++)
            {
                parents[i].name = "[" + Mathf.Min(parents[i].transform.childCount, 99).ToString("D2") + "] " + parents[i].name.Substring(5);
            }
            yield return cooldown;
        }
    }
    #endif
}

public enum SceneParentProviderType
{
    ENEMIES,
    PROJECTILES,
    PARTICLES,
    TOWERS
}
