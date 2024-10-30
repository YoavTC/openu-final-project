using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovingPathPoint : MonoBehaviour
{
    [SerializeField] private Vector2 posB;

    [SerializeField] private float movementDuration;
    [SerializeField] private float delay;

    private void Start()
    {
        transform.DOMove(posB, movementDuration).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
    }
}
