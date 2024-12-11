using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class VersionDisplayHandler : MonoBehaviour
{
    [SerializeField] private string format;
    [SerializeField] private BuildInfoSO buildInfo;
    
    void Start()
    {
        string prefix = "version";
        
        #if UNITY_EDITOR
        prefix = "editor";
        buildInfo.buildDate = String.Empty;
        #endif
        
        GetComponent<TMP_Text>().text = String.Format(format, prefix, Application.version, buildInfo.buildDate);
    }
}