using System.Collections;
using CodeMonkey.Utils;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public bool isDead;
    
    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
    }

    public void TakeDamage(float damage) => ApplyDamage(damage, Vector3.zero);
    public void TakeDamage(float damage, Vector3 dir) => ApplyDamage(damage, dir);
    
    private void ApplyDamage(float damage, Vector3 dir)
    {
        //TextPopupManager.Instance.DisplayPopup("-" + damage, transform.position, 5f, Color.red, 2f);
        UtilsClass.CreateWorldTextPopup(
            null,
            "-" + damage,
            transform.position,
            10,
            Color.red,
            transform.position + new Vector3(0, 20),
            2f);
        
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
