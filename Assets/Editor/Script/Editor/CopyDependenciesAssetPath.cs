using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CopyDependenciesAssetPath : EditorWindow
{
    private static string[] dependencies = Array.Empty<string>();
    private static string dependenciesAsFileString = string.Empty;
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("GameObject/Find Dependencies")]
    public static void FindDependencies()
    {
        dependencies = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(Selection.activeObject), true)
                                    .SelectMany(x => new[] { x, x + ".meta" }).ToArray();;
        dependenciesAsFileString = string.Join("\n", dependencies);
        EditorWindow.GetWindow(typeof(CopyDependenciesAssetPath));
    }

    [MenuItem("GameObject/Find Dependencies", true)]
    public static bool Validate()
    {
        if (!Application.isPlaying)
        {
            return AssetDatabase.Contains(Selection.activeObject);
        }
        return false;
    }

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.TextArea(dependenciesAsFileString);
        GUILayout.EndScrollView();
        if (GUILayout.Button("Copy"))
        {
            GUIUtility.systemCopyBuffer = dependenciesAsFileString;
        }
    }
}
