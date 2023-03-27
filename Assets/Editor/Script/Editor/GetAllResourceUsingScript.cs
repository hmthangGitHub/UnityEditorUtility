using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GetAllResourceUsingScript : EditorWindow
{
    string scriptName = "";
    UnityEngine.Object[] resources = default;
    [MenuItem("CustomTools/GetAllResourceUsingScript")]
    static void CreateWindow()
    {
        GetAllResourceUsingScript window = (GetAllResourceUsingScript)EditorWindow.GetWindowWithRect(typeof(GetAllResourceUsingScript), new Rect(0, 0, 400, 700));
    }

    void OnGUI()
    {
        GUILayout.Label("Enter script name");
        scriptName = GUILayout.TextField(scriptName);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Get Asset Path", GUILayout.Width(120)))
        {
            var type = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(t => t.Name == scriptName);

            // Resources.FindObjectsOfTypeAll()
            resources = FindAssetsByType2(type).ToArray();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Abort", GUILayout.Width(120)))
            Close();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (resources != default)
        {
            var type = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(t => t.Name == scriptName);

            foreach (var item in resources)
            {
                EditorGUILayout.ObjectField(item, type, allowSceneObjects: false);
            }
        }
    }

    public static List<UnityEngine.Object> FindAssetsByType2(Type t)
    {
        List<UnityEngine.Object> assets = new List<UnityEngine.Object>();

        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(assetPath);
            if (asset != null)
            {
                if (asset.GetComponentsInChildren(t, true).Count() > 0)
                {
                    assets.Add(asset);
                }
            }
        }
        return assets;
    }
}
