using UnityEngine;

public class TowerManager : HealthBaseListManager
{
    #region Singleton
    private static TowerManager _instance;

    public static TowerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TowerManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(TowerManager).Name);
                    _instance = singletonObject.AddComponent<TowerManager>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
    }
    #endregion
}