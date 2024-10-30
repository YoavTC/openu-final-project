using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionChecker : MonoBehaviour
{
    public void SpawningStopped()
    {
        StartCoroutine(Check());
    }

    private IEnumerator Check()
    {
        Debug.Log("STARTED");
        bool AAA = EnemyManager.Instance.AllEnemiesDead();
        while (!AAA)
        {
            AAA = EnemyManager.Instance.AllEnemiesDead();
            Debug.Log("NOPE");
            yield return new WaitForSeconds(3f);
        }
        
        Debug.Log("YEP");
        SceneManager.LoadScene(0);
    }
}
