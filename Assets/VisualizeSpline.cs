using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class VisualizeSpline : MonoBehaviour
{
    private Transform[] points;
    private string pathPoint = "pathPoint";
    private string pathEnd = "pathEnd";
    private string pathStart = "pathStart";

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i);
        }

        if (points == null || points.Length < 2)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < points.Length - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
            {
                Gizmos.DrawIcon(points[i].position, pathPoint, false);
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
        }
        
        Gizmos.DrawIcon(points[points.Length - 1].position, pathEnd, false);
        Gizmos.DrawIcon(points[0].position, pathStart, false);
    }
}