using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Object = System.Object;

public class GenerateTowerScriptableObjects : Editor
{
    [MenuItem("Automation/Generate Tower Items")]
    static void GenerateItems()
    {
        GenerateTowerScriptableObjectsWindow.OpenWindow();
    }
}

public class GenerateTowerScriptableObjectsWindow : EditorWindow
{
    private const int WINDOW_SIZE = 550;
    private const int BUTTON_WIDTH = 150;
    private const int BUTTON_HEIGHT = 25;
    private Rect buttonRect;

    private string path;
    
    public static void OpenWindow()
    {
        Rect windowRect = new Rect(
            (Screen.currentResolution.width / 2f) - (WINDOW_SIZE / 2f),
            (Screen.currentResolution.height  / 2f) - (WINDOW_SIZE / 2f),
            WINDOW_SIZE,
            WINDOW_SIZE
        );
        
        //Close and reopen window to prevent bugs
        GetWindow<GenerateTowerScriptableObjectsWindow>().Close();
        
        GenerateTowerScriptableObjectsWindow window = GetWindowWithRect<GenerateTowerScriptableObjectsWindow>(
            windowRect,
            focusedWindow,
            "Generate Tower Items Window",
            true);

        window.position = windowRect;
    }

    private void Awake()
    {
        buttonRect = new(
            WINDOW_SIZE / 2f - (BUTTON_WIDTH / 2f),
            WINDOW_SIZE / 2f - (BUTTON_HEIGHT / 2f),
            BUTTON_WIDTH, BUTTON_HEIGHT
        );

        FindSpriteAssets();
        spriteList.Clear();
    }
    
    private List<Sprite> spriteList = new List<Sprite>();
    private Vector2 scrollPosition;

    private void OnGUI()
    {
        // Drag-and-drop area
        Rect dropArea = GUILayoutUtility.GetRect(0, 100, GUILayout.ExpandWidth(true));
        GUIStyle boxStyle = new("Box");
        boxStyle.alignment = TextAnchor.MiddleCenter;
        GUI.Box(dropArea, "Drag Sprites Here", boxStyle);

        // Handle drag-and-drop
        HandleDragAndDrop(dropArea);

        // Display the imported sprites
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < spriteList.Count; i++)
        {
            spriteList[i] = (Sprite)EditorGUILayout.ObjectField(spriteList[i].name, spriteList[i], typeof(Sprite), false);
        }
        EditorGUILayout.EndScrollView();
        
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = false;
        string pathToDisplay = path;
        if (pathToDisplay.Length > 65)
        {
            pathToDisplay = "..." + path.Substring(path.Length - 65);
        }
        EditorGUILayout.TextField(pathToDisplay);
        GUI.enabled = true;

        GUIStyle browseDirStyle = new("Button");
        browseDirStyle.fixedWidth = 70;
        
        if (GUILayout.Button("Browse..", browseDirStyle))
        {
            path = EditorUtility.OpenFolderPanel("Select a Folder", "Assets/", "");
            if (!path.Contains("Assets"))
            {
                EditorUtility.DisplayDialog("Invalid Path", "Please choose a directory inside the project!", "OK");
                path = "";
            }
        }
        EditorGUILayout.EndHorizontal();

        // Optional: Button to clear the sprite list
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Sprites"))
        {
            spriteList.Clear();
        }
        if (GUILayout.Button("Refresh GUID Cache"))
        {
            FindSpriteAssets();
        }
        EditorGUILayout.EndHorizontal();

        GUIStyle guiStyle = new("Button");
        guiStyle.fixedHeight = 75f;
        guiStyle.fontSize = 30;
        guiStyle.fontStyle = FontStyle.Bold;

        GUI.enabled = path != "";
        
        if (GUILayout.Button("Generate", guiStyle))
        {
            Generate();
        }
        
        GUI.enabled = true;
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        Event e = Event.current;

        // Check if we are in the drop area
        if (dropArea.Contains(e.mousePosition))
        {
            switch (e.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    e.Use();
                    break;
                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is Texture2D texture2D)
                        {
                            // Retrieve the associated sprite
                            Sprite sprite = GetSpriteFromTexture(texture2D);
                            if (sprite != null)
                            {
                                spriteList.Add(sprite);
                            }
                            else
                            {
                                Debug.LogWarning($"No sprite found for texture: {texture2D.name}");
                            }
                        }
                    }
                    e.Use();
                    break;
            }
        }
    }

    private Sprite GetSpriteFromTexture(Texture2D texture)
    {
#if UNITY_EDITOR
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        
            if (sprite != null && sprite.texture == texture)
            {
                return sprite; // Return the first sprite found with this texture
            }
        }
#endif
        return null; // Return null if no sprite was found
    }

    private string[] guids;
    
    private void FindSpriteAssets()
    {
        guids = AssetDatabase.FindAssets("t:sprite");
    }


    private void Generate()
    {
        foreach (Sprite sprite in spriteList)
        {
            TowerSettings newTowerSettingsItem = CreateInstance<TowerSettings>();
            newTowerSettingsItem.sprite = sprite;
            newTowerSettingsItem.name = sprite.name;

            int startPathIndex = path.IndexOf("Assets");

            if (startPathIndex == -1)
            {
                EditorUtility.DisplayDialog("Invalid Path", "Please choose a directory inside the project!", "OK");
                return;
            }
            
            AssetDatabase.CreateAsset(newTowerSettingsItem, path.Substring(startPathIndex) + "/" + newTowerSettingsItem.name + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
