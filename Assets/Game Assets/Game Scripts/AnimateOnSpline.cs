using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class AnimateOnSpline : MonoBehaviour
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
}
