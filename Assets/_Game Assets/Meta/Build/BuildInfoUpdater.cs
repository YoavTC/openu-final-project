using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildInfoUpdater : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // Find the BuildInfo asset
        BuildInfoSO buildInfo = AssetDatabase.LoadAssetAtPath<BuildInfoSO>("Assets/_Game Assets/Meta/BuildInfo.asset");
        
        if (buildInfo == null)
        {
            Debug.LogError("BuildInfo asset not found! Please create one at 'Assets/BuildInfo.asset'.");
            return;
        }

        // Update the build date
        buildInfo.buildDate = "[" + DateTime.Now.ToString("dd-MM-yyyy") + "]";
        EditorUtility.SetDirty(buildInfo);

        // Save the asset to persist the change
        AssetDatabase.SaveAssets();

        Debug.Log($"BuildInfo updated: {buildInfo.buildDate}");
    }
}