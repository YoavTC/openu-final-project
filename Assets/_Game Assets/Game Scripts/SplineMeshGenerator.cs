using System.Collections;
using UnityEngine;

public class SplineMeshGenerator : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;
        
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        EdgeCollider2D edgeCollider2D = GetComponent<EdgeCollider2D>();
        
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);
        
        int pointCount = lineRenderer.positionCount;
        Vector3[] positions = new Vector3[pointCount];
        lineRenderer.GetPositions(positions);

        // Convert positions to 2D points for the collider
        Vector2[] edgePoints = new Vector2[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            Vector3 position = positions[i];
            edgePoints[i] = new Vector2(position.x, position.y); // Use exact center of the line
        }

        // Assign the points directly
        edgeCollider2D.points = edgePoints;
    }
}