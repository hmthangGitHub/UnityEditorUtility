using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FindMissingScripts : EditorWindow
{
    [MenuItem("Window/FindMissingScripts")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FindMissingScripts));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts in selected prefabs"))
        {
            FindInSelected();
        }
    }
    private static void FindInSelected()
    {
        var roots = Selection.activeGameObject.scene.GetRootGameObjects();
        var allGo = new List<GameObject>();
        foreach (var item in roots)
        {
            allGo.AddRange(item.GetComponentsInChildren<Transform>(true).Select(x => x.gameObject));
        }

        var tag = "Find missing script";

        foreach (var item in allGo)
        {
            if (item.GetComponents<Component>().Any(x => x == null))
            {
                Debug.LogError($"[{tag}] {item.gameObject.name} has missing script", item);
            }
        }

        ForcusConsoleWithTag($"[{tag}]");
    }


    public static void ForcusConsoleWithTag(string tag)
    {
        var allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        var console = allWindows.FirstOrDefault(x => x.GetType().ToString() == "UnityEditor.ConsoleWindow");
        var field = console.GetType().GetField("m_SearchText", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        console.Focus();
        field.SetValue(console, tag);
    }


   
}
