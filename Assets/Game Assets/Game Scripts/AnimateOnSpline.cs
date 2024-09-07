using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimateOnSpline : MonoBehaviour
{
    public Spline currentSpline { private set; get; }
    public float speed { private set; get; }
    public Action<Transform> OnReachSplineEndEvent;
    private Transform nextPoint;
    
    public void Init(Spline currentSpline, float speed)
    {
        this.currentSpline = currentSpline;
        this.speed = speed;
    }
    
    public void Init(Spline currentSpline, float speed, Action<Transform> onReachSplineEndEvent)
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
            OnReachSplineEndEvent?.Invoke(transform);
            Destroy(this);
        }
    }

    private Transform GetNextPoint()
    {
        return currentSpline.GetNextPointFromPath(transform.position);
    }
}
