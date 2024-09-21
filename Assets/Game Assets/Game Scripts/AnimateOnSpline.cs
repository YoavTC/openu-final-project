using System;
using UnityEngine;

public class AnimateOnSpline : MonoBehaviour
{
    private Spline currentSpline;
    private float speed;
    private Action<Enemy> OnReachSplineEndEvent;
    private Transform nextPoint;
    
    public void Init(Spline currentSpline, float speed, Action<Enemy> onReachSplineEndEvent = null)
    {
        this.currentSpline = currentSpline;
        this.speed = speed;
        OnReachSplineEndEvent = onReachSplineEndEvent;
    }

    private void Start()
    {
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
