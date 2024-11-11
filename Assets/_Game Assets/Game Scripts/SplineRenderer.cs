using System.Collections.Generic;
using External_Packages;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SplineRenderer : MonoBehaviour
{
    [SerializeField] private List<Material> lineMaterials = new List<Material>();
    [SerializeField] private float lineWidthMultiplier = 1f; 
    
    private LineRenderer lineRenderer;
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        
        //Set positions
        Vector3[] positions = GetChildPositions(transform);
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);

        //Draw the line with material(s)
        lineRenderer.materials = lineMaterials.ToArray();
        lineRenderer.widthMultiplier = lineWidthMultiplier;
        
        lineRenderer.enabled = true;
    }
    
    private Vector3[] GetChildPositions(Transform parent)
    {
        Transform[] children = HelperFunctions.GetChildren(parent).ToArray();
        Vector3[] positions = new Vector3[children.Length];

        for (int i = 0; i < children.Length; i++)
        {
            positions[i] = children[i].position;
        }
        
        return positions;
    }
}
