using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateMockUpImage : Editor
{
    [UnityEditor.MenuItem("GameObject/MockUp/Create Mock Up Image", false, 48)]
    public static void CreateMockUp()
    {
        var canvas = FindObjectOfType<Canvas>();
        var go = new GameObject("MockUp");
        go.transform.SetParent(canvas.transform, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.SetSiblingIndex(2);
        var rectTransform = go.AddComponent<RectTransform>();

        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchorMin = Vector2.zero;

        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        go.AddComponent<Image>();
        Selection.activeGameObject = go;

        EditorGUIUtility.PingObject(go);

        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath("Assets/Temp/ThangTemp/MockUp", typeof(UnityEngine.Object));
        Selection.activeObject = obj;

        EditorGUIUtility.PingObject(obj);
    }


    [UnityEditor.MenuItem("GameObject/MockUp/Remove MockUp", false, 48)]
    public static void RemoveMockUp()
    {
        var canvas = FindObjectOfType<Canvas>();
        var go = canvas.transform.GetChild(2);
        if(go.name == "MockUp" && go.GetComponent<Image>())
        {
            DestroyImmediate(go.gameObject);
        }
    }
}
