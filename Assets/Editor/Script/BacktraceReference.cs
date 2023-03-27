#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;


using UnityEditor;
public class BacktraceReference : EditorWindow
{
    /// <summary> The result </summary>
    public static List<Component> ReferencingSelection = new List<Component>();
    /// <summary> allComponents in the scene that will be searched to see if they contain the reference </summary>
    private static Component[] allComponents;
    /// <summary> Selection of gameobjects the user made </summary>
    private static GameObject[] selections;

    public static bool AllScenes { get; private set; } = false;

    /// <summary>
    /// Adds context menu to hierarchy window https://answers.unity.com/questions/22947/adding-to-the-context-menu-of-the-hierarchy-tab.html
    /// </summary>
    [UnityEditor.MenuItem("GameObject/Find Objects Referencing This", false, 48)]
    public static void InitHierarchy()
    {
        selections = UnityEditor.Selection.gameObjects;
        BacktraceSelection(selections);
        GetWindow(typeof(BacktraceReference));
    }
    /// <summary>
    /// Display referenced by components in window
    /// </summary>
    public void OnGUI()
    {
        BacktraceReference.AllScenes = GUILayout.Toggle(BacktraceReference.AllScenes, "Search all scenes");
        if (GUILayout.Button("Search current selection"))
        {
            selections = UnityEditor.Selection.gameObjects;
            BacktraceSelection(selections);
        }
        if (selections == null || selections.Length < 1)
        {
            GUILayout.Label("Select source object/s from scene Hierarchy panel.");
            return;
        }
        // display reference that is being checked
        GUILayout.Label(string.Join(", ", selections.Where(go => go != null).Select(go => go.name).ToArray()));
        // handle no references
        if (ReferencingSelection == null || ReferencingSelection.Count == 0)
        {
            GUILayout.Label("is not referenced by any gameobjects in the scene");
            return;
        }
        // display list of references using their component name as the label
        foreach (var item in ReferencingSelection)
        {
            EditorGUILayout.ObjectField(item.GetType().ToString(), item, typeof(GameObject), allowSceneObjects: true);
        }
    }
    // This script finds all objects in scene
    private static Component[] GetAllActiveInScene()
    {
        // Use new version of Resources.FindObjectsOfTypeAll(typeof(Component)) as per https://forum.unity.com/threads/editorscript-how-to-get-all-gameobjects-in-scene.224524/
        List<Component> result = new List<Component>();

        var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage == null)
        {
            if (BacktraceReference.AllScenes)
            {
                int countLoaded = SceneManager.sceneCount;
                Scene[] loadedScenes = new Scene[countLoaded];

                for (int i = 0; i < countLoaded; i++)
                {
                    loadedScenes[i] = SceneManager.GetSceneAt(i);
                }

                foreach (var item in loadedScenes)
                {
                    var rootObjects = item.GetRootGameObjects();
                    foreach (var rootObject in rootObjects)
                    {
                        result.AddRange(rootObject.GetComponentsInChildren<Component>(true));
                    }
                }

                var go = new GameObject("Sacrificial Lamb");
                DontDestroyOnLoad(go);

                foreach (var root in go.scene.GetRootGameObjects())
                {
                    result.AddRange(root.GetComponentsInChildren<Component>(true));
                }
                DestroyImmediate(go);
            }
            else
            {
                if (Selection.activeGameObject != null)
                {
                    var rootObjects = Selection.activeGameObject.scene.GetRootGameObjects();
                    foreach (var rootObject in rootObjects)
                    {
                        result.AddRange(rootObject.GetComponentsInChildren<Component>(true));
                    }
                }
            }
        }
        else
        {
            result.AddRange(prefabStage.prefabContentsRoot.GetComponentsInChildren<Component>(true));
        }

        return result.ToArray();
    }
    private static void BacktraceSelection(GameObject[] selections)
    {
        if (selections == null || selections.Length < 1)
            return;
        allComponents = GetAllActiveInScene();
        if (allComponents == null) return;
        ReferencingSelection.Clear();
        foreach (GameObject selection in selections)
        {
            foreach (Component cOfSelection in selection.GetComponents<Component>())
            {
                FindObjectsReferencing(cOfSelection);
            }
        }
    }
    private static void FindObjectsReferencing<T>(T cOfSelection) where T : Component
    {
        foreach (Component sceneComponent in allComponents)
        {
            try
            {
                componentReferences(sceneComponent, cOfSelection);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{sceneComponent} has error cant inspect");
            }

        }
    }
    /// <summary>
    /// Determines if the component makes any references to the second "references" component in any of its inspector fields
    /// </summary>
    private static void componentReferences(Component component, Component references)
    {
        // find all fields exposed in the editor as per https://answers.unity.com/questions/1333022/how-to-get-every-public-variables-from-a-script-in.html
        SerializedObject serObj = new SerializedObject(component);
        SerializedProperty prop = serObj.GetIterator();

        while (prop.NextVisible(true))
        {
            bool isObjectField = prop.propertyType == SerializedPropertyType.ObjectReference && prop.objectReferenceValue != null;
            if (isObjectField && prop.objectReferenceValue == references)
            {
                ReferencingSelection.Add(component);
            }
        }
    }
}
#endif
