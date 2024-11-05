using System.Collections.Generic;
using External_Packages;
using UnityEngine;

public class Spline : MonoBehaviour
{
    private List<Transform> splinePoints = new List<Transform>();

    private void Start()
    {
        splinePoints = HelperFunctions.GetChildren(transform);
    }
    
    public Transform GetLastPoint() => splinePoints[splinePoints.Count - 1];
    public Transform GetFirstPoint() => splinePoints[0];
    
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

    public Vector2 GetPointOnPathInTime(Vector2 currentPositionOnPath, float time, float speed)
    {
        Transform nextPoint = GetNextPointFromPath(currentPositionOnPath);
        Vector2 virtualPointOnPath = currentPositionOnPath;
        Vector2 moveDir = currentPositionOnPath - (Vector2) nextPoint.position;

        virtualPointOnPath += (moveDir * speed) * time * Time.deltaTime;
        return virtualPointOnPath;
    }

    #region Spline Debug View
    
    private const string PATH_POINT = "pathPoint";
    private const string PATH_END = "pathEnd";
    private const string PATH_START = "pathStart";
    
    [SerializeField] private bool debugDrawSpline;
    private List<Transform> points = new List<Transform>();
    
    private void OnDrawGizmos()
    {
        if (!debugDrawSpline) return;
        points.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            points.Add(transform.GetChild(i));
        }

        if (points == null || points.Count < 2)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
            {
                Gizmos.DrawIcon(points[i].position, PATH_POINT, false);
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
        }
        
        Gizmos.DrawIcon(points[points.Count - 1].position, PATH_END, false);
        Gizmos.DrawIcon(points[0].position, PATH_START, false);
    }

    #endregion
}