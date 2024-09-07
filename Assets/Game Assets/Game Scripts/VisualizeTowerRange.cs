using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class VisualizeTowerRange : MonoBehaviour
{
    [SerializeField] private bool alwaysDraw;
    
    private void OnDrawGizmos()
    {
        if (alwaysDraw) Draw();
    }

    private void OnDrawGizmosSelected()
    {
        if (!alwaysDraw) Draw();
    }

    private void Draw()
    {
        float range = GetComponent<Tower>().GetTowerRange();
        Handles.color = Color.magenta;
        //Gizmos.DrawWireSphere(transform.position, range);
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, range);
    }
}
