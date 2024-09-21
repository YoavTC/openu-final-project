using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class AnimateOnSpline : MonoBehaviour
{
    [SerializeField] [ReadOnly] private float speed;
    [SerializeField] [ReadOnly] private Transform nextPoint;
    [SerializeField] [ReadOnly] private Spline currentSpline;
    private Action<Enemy> OnReachSplineEndEvent;
    
    public void Init(Spline currentSpline, float speed, Action<Enemy> onReachSplineEndEvent)
    {
        this.currentSpline = currentSpline;
        this.speed = speed;
        OnReachSplineEndEvent += onReachSplineEndEvent;
        
        transform.position = currentSpline.GetFirstPoint().position;
    }

    void Update()
    {
        if (nextPoint == null || transform.position == nextPoint.position) nextPoint = GetNextPoint();
        else transform.position = Vector3.MoveTowards(transform.position,nextPoint.position, (Time.deltaTime * speed));
        

        if (transform.position == currentSpline.GetLastPoint().position)
        {
            OnReachSplineEndEvent?.Invoke(transform.GetComponent<Enemy>());
            Destroy(this);
        }
    }

    private Transform GetNextPoint()
    {
        return currentSpline.GetNextPointFromPath(transform.position);
    }
}
