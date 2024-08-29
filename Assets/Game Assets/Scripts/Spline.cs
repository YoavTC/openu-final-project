using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(VisualizeSpline))]
public class Spline : MonoBehaviour
{
    public Transform lastPoint => splinePoints.Last();
    public Transform firstPoint => splinePoints.First();
    private List<Transform> splinePoints = new List<Transform>();

    private void Start()
    {
        splinePoints = HelperFunctions.GetChildren(transform);
    }
    
    
    // Function to find the closest point on the spline and return the next point
    private Transform GetNextPointByDistance(Vector2 point)
    {
        if (splinePoints == null || splinePoints.Count == 0)
        {
            Debug.LogWarning("Spline points are not initialized.");
            return null;
        }

        int onPathIndexCheck = GetOnPath(point);
        if (onPathIndexCheck != -1) return splinePoints[onPathIndexCheck + 1];

        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;
        int closestIndex = -1;

        // Find the closest point on the spline
        for (int i = 0; i < splinePoints.Count; i++)
        {
            float distance = (splinePoints[i].position - (Vector3)point).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = splinePoints[i];
                closestIndex = i;
            }
        }

        // Return the next point in the spline or loop back to the start if at the end
        if (closestPoint != null)
        {
            int nextIndex = (closestIndex + 1) % splinePoints.Count;
            return splinePoints[nextIndex];
        }

        return null;
    }

    public Transform GetNextPointFromPath(Vector2 point)
    {
        for (int i = 0; i < splinePoints.Count; i++)
        {
            if ((Vector2)splinePoints[i].position == point)
            {
                return splinePoints[i + 1];
            }
        }

        //failsafe
        return GetNextPointByDistance(point);
    }

    private int GetOnPath(Vector2 point)
    {
        for (int i = 0; i < splinePoints.Count; i++)
        {
            if (point == (Vector2)splinePoints[i].position)
            {
                return i;
            }
        }

        return -1;
    }
}