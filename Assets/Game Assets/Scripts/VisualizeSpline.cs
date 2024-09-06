using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualizeSpline : MonoBehaviour
{
    private List<Transform> points = new List<Transform>();
    private string pathPoint = "pathPoint";
    private string pathEnd = "pathEnd";
    private string pathStart = "pathStart";

    private void OnDrawGizmos()
    {
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
                Gizmos.DrawIcon(points[i].position, pathPoint, false);
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
        }
        
        Gizmos.DrawIcon(points[points.Count - 1].position, pathEnd, false);
        Gizmos.DrawIcon(points[0].position, pathStart, false);
    }
}