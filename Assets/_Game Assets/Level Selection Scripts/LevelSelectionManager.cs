using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveDuration;
    [SerializeField] private float dist;

    private void Start()
    {
        //player.position = spline.transform.GetChild(PlayerPrefs.GetInt("LVL") * 2).position;
        player.DOMoveX(PlayerPrefs.GetInt("LVL") * dist, moveDuration);
    }

    [SerializeField] private int lvl;
    [Button] 
    private void Set()
    {
        PlayerPrefs.SetInt("LVL", lvl);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightControl))
        {
            lvl = 0;
            Set();
            SceneManager.LoadScene("_Scenes/Level Selection Scene");
        }
    }
    
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene("LVL_" + index);
    }
}