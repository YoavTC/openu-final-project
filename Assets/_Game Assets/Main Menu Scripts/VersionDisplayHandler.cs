using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
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