using UnityEngine;

public class LevelIsland : MonoBehaviour
{
    [HideInInspector] public Vector2 entryPos;
    [HideInInspector] public Vector2 exitPos;
    [HideInInspector] public Vector2 centerPos;

    private void Start()
    {
        entryPos = transform.GetChild(0).position;
        centerPos = transform.position;
        exitPos = transform.GetChild(1).position;
    }
}