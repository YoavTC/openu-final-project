using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    public bool isDead;
    
    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
    }

    public void TakeDamage(float damage) => ApplyDamage(damage, Vector3.zero);
    public void TakeDamage(float damage, Vector3 dir) => ApplyDamage(damage, dir);
    
    private void ApplyDamage(float damage, Vector3 dir)
    {
        if (health - damage <= 0)
        {
            StartCoroutine(DeathCoroutine());
        }
        else
        {
            health -= damage;
        } 
    }

    private IEnumerator DeathCoroutine()
    {
        //Wait for the Enemy manager to safely remove enemy from list
        bool callback = false;
        EnemyManager.Instance.RemoveEnemy(this, () => callback = true);
        yield return new WaitUntil(() => callback);
        
        Destroy(gameObject);
    }

    public void CalculateDamage(float damage)
    {
        float futureHealth = health - damage;
        if (futureHealth <= 0)
        {
            isDead = true;
        }
    }
}
