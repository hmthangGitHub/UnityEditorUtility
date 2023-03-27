#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasUITouchDetector : EditorWindow
{
    [MenuItem("CustomTools/UITouchDetector")]
    public static void Show()
    {
        EditorWindow.GetWindow<CanvasUITouchDetector>();
    }
    private GameObject[] gameObjectList = Array.Empty<GameObject>();

    private void OnGUI()
    {
        foreach (var gameObject in gameObjectList)
        {
            if (gameObject != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(gameObject.name, gameObject, typeof(GameObject), allowSceneObjects: true);
                var isActive = gameObject.activeSelf;
                isActive = GUILayout.Toggle(isActive, new GUIContent("Active"));
                gameObject.gameObject.SetActive(isActive);
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Application.isPlaying)
        {
            var eventSystemRaycastResults = GetEventSystemRaycastResults();
            gameObjectList = eventSystemRaycastResults.Select(x => x.gameObject).ToArray();
        }
    }

    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults )
    {
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position =  Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, raysastResults );
        return raysastResults;
    }
}

#endif
