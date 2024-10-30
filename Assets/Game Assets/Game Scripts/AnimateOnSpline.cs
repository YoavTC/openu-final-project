using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class AnimateOnSpline : MonoBehaviour, IModifierAffectable
{
    [SerializeField] [ReadOnly] private float speed;
    [SerializeField] [ReadOnly] private Transform nextPoint;
    [SerializeField] [ReadOnly] private Spline currentSpline;
    private Action<Enemy> onReachSplineEndAction;
    private Action removeEnemyOnReachEndAction;
    
    public void Init(Spline currentSpline, float speed, Action<Enemy> enemyReachEndListener, Action enemyReachEndAction)
    {
        this.currentSpline = currentSpline;
        this.speed = speed;
        onReachSplineEndAction += enemyReachEndListener;
        removeEnemyOnReachEndAction += enemyReachEndAction;
        
        transform.position = currentSpline.GetFirstPoint().position;
    }

    void Update()
    {
        if (nextPoint == null || transform.position == nextPoint.position) nextPoint = GetNextPoint();
        else transform.position = Vector3.MoveTowards(transform.position,nextPoint.position, (Time.deltaTime * speed));
        

        if (transform.position == currentSpline.GetLastPoint().position)
        {
            onReachSplineEndAction?.Invoke(transform.GetComponent<Enemy>());
            removeEnemyOnReachEndAction?.Invoke();
            Destroy(this);
        }
    }

    private Transform GetNextPoint()
    {
        return currentSpline.GetNextPointFromPath(transform.position);
    }
    
    #region Modifier Effects
    
    public ModifierEffect modifierEffect;
    
    [Button]
    public void TestEffect()
    {
        currentEffect = modifierEffect;
        StartEffect();
    }
    
    public ModifierEffect currentEffect { get; set; }
    
    public void StartEffect()
    {
        if (currentEffect.type == ModifierEffectType.SPEED)
        {
            Debug.Log($"Started effect {currentEffect.type} on {gameObject.name} for {currentEffect.duration}!");
            StartCoroutine(TickEffect());
        }
    }
    
    public IEnumerator TickEffect()
    {
        float durationProgress = 0f;
        float tickRate = currentEffect.tickRate;
        float originalSpeed = speed;
        
        while (durationProgress <= currentEffect.duration)
        {
            speed = originalSpeed / Mathf.Max(1, currentEffect.strengthCurve.Evaluate(durationProgress));
            
            yield return new WaitForSeconds(tickRate);
            durationProgress += tickRate;
            Debug.Log($"{durationProgress}/{currentEffect.duration}");
        }

        speed = originalSpeed;
    }
    #endregion
}
