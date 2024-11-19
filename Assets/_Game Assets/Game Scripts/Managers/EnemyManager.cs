using UnityEngine;

public class EnemyManager : HealthBaseListManager
{
    #region Singleton
    private static EnemyManager _instance;

    public static EnemyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(EnemyManager).Name);
                    _instance = singletonObject.AddComponent<EnemyManager>();
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