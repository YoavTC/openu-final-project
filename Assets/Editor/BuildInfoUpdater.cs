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
        // Increment build version
        if (Application.version.Contains("."))
        {
            var versionNumber = Application.version.Split('.');
            int subVersionNumber = 1 + int.Parse(versionNumber[1]);
            PlayerSettings.bundleVersion = $"{versionNumber[0]}.{subVersionNumber}";
        }
        
        // Find the BuildInfo asset
        BuildInfoSO buildInfo = AssetDatabase.LoadAssetAtPath<BuildInfoSO>("Assets/_Game Assets/Meta/Build/BuildInfo.asset");
        
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