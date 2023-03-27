using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
using System.Linq;
using System.Text.RegularExpressions;

using Object = UnityEngine.Object;

public class SearchHelper : EditorWindow
{

    private string m_stringToSearch;
    private static bool m_onlyActiveObject = false;
    //private GameObject[] allGameObjects;

    private Dictionary<string, List<TextMeshProUGUI>> m_allTextDictionary = new Dictionary<string, List<TextMeshProUGUI>>();
    Vector2 m_scrollPos;


    [UnityEditor.MenuItem("CustomTools/Search string %Q", false, 48)]
    public static void InitHierarchy()
    {
        GetWindow(typeof(SearchHelper));

    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        m_stringToSearch = EditorGUILayout.TextField(m_stringToSearch);
        m_onlyActiveObject = GUILayout.Toggle(m_onlyActiveObject, "Active Objects");

        if (GUILayout.Button("Find"))
        {
            m_allTextDictionary = SearchHelper.FindString(m_stringToSearch);
        }

        EditorGUILayout.EndHorizontal();
        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);

        if (m_allTextDictionary?.Any() == true)
        {
            EditorGUILayout.BeginVertical();
            foreach (var item in m_allTextDictionary)
            {
                EditorGUILayout.LabelField($"Scene : {item.Key}");
                foreach (var text in item.Value)
                {
                    if (text != null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField(text.GetType().ToString(), text, typeof(TextMeshProUGUI), allowSceneObjects: true);
                        var isActive = text.gameObject.activeSelf;
                        isActive = GUILayout.Toggle(isActive, new GUIContent("Active"));
                        text.gameObject.SetActive(isActive);
                        EditorGUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private static Dictionary<string, List<TextMeshProUGUI>> FindString(string stringToFind)
    {
        TextMeshProUGUI[] allTexts = null;
        var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        allTexts = prefabStage == null ? ComponentSearcher.GetAllComponentInAllScene<TextMeshProUGUI>(m_onlyActiveObject) : prefabStage.prefabContentsRoot.GetComponentsInChildren<TextMeshProUGUI>(!m_onlyActiveObject);

        return allTexts.Where(x => x != null && x.text != null && Regex.IsMatch(x.text, stringToFind))
                      .GroupBy(t => t.gameObject.scene.name)
                      .ToDictionary(x => x.Key, x => x.ToList());
    }
}
