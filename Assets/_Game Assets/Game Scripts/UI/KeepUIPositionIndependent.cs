using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class KeepUIPositionIndependent : MonoBehaviour
{
    private Vector3 initialPosition;
    
        void Start()
        {
            initialPosition = transform.position;
        }
    
        void LateUpdate()
        {
            transform.position = initialPosition;
        }
}
