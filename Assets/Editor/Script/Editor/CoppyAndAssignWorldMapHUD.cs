// using Castle.Core.Internal;
// using JetBrains.Annotations;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using UnityEditor;
// using UnityEditor.SceneManagement;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class CoppyAndAssignWorldMapHUD : EditorWindow
// {
//     private string resourceName = "";
//     Vector2 scrollPosition = Vector2.zero;
//     private static List<Image> allImages = new List<Image>();
//     private static List<Component> allObject = new List<Component>();
//     static string woldMapHUDFolder = "Assets/Art/Hud/WorldMap_HUD/";

//     [MenuItem("CustomTools/Gather dependencies")]
//     public static void gather()
//     {
//         string worldMapHUDScene = "Assets/Scenes/s_world_map_hud.unity";
//         string[] names = AssetDatabase.GetDependencies(worldMapHUDScene, true);
//         string[] pngs = names.FindAll(name => (name.Contains(".png") || name.Contains(".PNG"))
//                                     && !name.Contains("Assets/Art/Hud/Background/Dungeon")
//                                     && !name.Contains("Assets/Art/Scene_worldmap")
//                                     && !name.Contains("Assets/Art/Hud/WorldMapUI")
//                                     );
//         foreach (var png in pngs)
//         {
//             var fileName = png.Split('/').Last();
//             AssetDatabase.CopyAsset(png, woldMapHUDFolder + fileName);
//         }
//     }

//     [MenuItem("CustomTools/Gather All Image In Scenes")]
//     public static void gatherAllImageInScene()
//     {
//         allImages = new List<Image>();
//         var go = UnityEngine.Object.FindObjectOfType<GameObject>();
//         var roots = go.scene.GetRootGameObjects();
//         foreach (var root in roots)
//         {
//             allImages.AddRange(root.GetComponentsInChildren<Image>(true));
//         }
//         //allImages = new List<Image>(UnityEngine.Object.FindObjectsOfType<Image>());
//         //allImages[0].gameObject.scene.GetRootGameObjects()
//         GetWindow(typeof(CoppyAndAssignWorldMapHUD));
//     }
//     public void OnGUI()
//     {
//         EditorGUILayout.BeginHorizontal();

//         resourceName = GUILayout.TextArea(resourceName);
//         if (GUILayout.Button("Find"))
//         {
//             findSprite(resourceName);
//         }
//         if (GUILayout.Button("FindComponent"))
//         {
//             findComponent(resourceName);
//         }

//         EditorGUILayout.EndHorizontal();

//         if (allImages.Count > 0)
//         {
//             EditorGUILayout.BeginVertical();
//             if (GUILayout.Button("Replace all"))
//             {
//                 foreach (var item in allImages)
//                 {
//                     var spritePath = AssetDatabase.GetAssetPath(item.sprite);
//                     if (!spritePath.Contains(woldMapHUDFolder))
//                     {
//                         var spriteName = spritePath.Split('/').Last();
//                         if (!spriteName.IsNullOrEmpty())
//                         {
//                             var sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{woldMapHUDFolder}{spriteName}");
//                             if (sprite == null)
//                             {
//                                 //EditorUtility.DisplayDialog("Not found", $"Not found {woldMapHUDFolder}{spriteName}", "OK");
//                                 Debug.LogError($"Not found {woldMapHUDFolder}{spriteName}");
//                             }
//                             else
//                             {
//                                 item.sprite = sprite;
//                                 EditorUtility.SetDirty(item);
//                                 EditorSceneManager.MarkSceneDirty(item.gameObject.scene);
//                             }
//                         }
//                     }
//                 }
//             }
//             scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
//             foreach (var item in allImages)
//             {
//                 EditorGUILayout.BeginHorizontal();
//                 EditorGUILayout.ObjectField(item.GetType().ToString(), item, typeof(Image), allowSceneObjects: true);
//                 var spritePath = AssetDatabase.GetAssetPath(item.sprite);
//                 if (!spritePath.Contains(woldMapHUDFolder))
//                 {
//                     var spriteName = spritePath.Split('/').Last();
//                     if (!spriteName.IsNullOrEmpty())
//                     {
//                         if (GUILayout.Button($"Replace {spriteName}"))
//                         {
//                             var sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{woldMapHUDFolder}{spriteName}");
//                             if (sprite == null)
//                             {
//                                 EditorUtility.DisplayDialog("Not found", $"Not found {woldMapHUDFolder}{spriteName}", "OK");
//                             }
//                             else
//                             {
//                                 item.sprite = sprite;
//                                 EditorUtility.SetDirty(item);
//                                 EditorSceneManager.MarkSceneDirty(item.gameObject.scene);
//                             }
//                         }
//                     }

//                 }
//                 EditorGUILayout.EndHorizontal();
//             }
//             EditorGUILayout.EndScrollView();
//             EditorGUILayout.EndVertical();
//         }

//         if (allObject.Count > 0)
//         {
//             EditorGUILayout.BeginVertical();

//             scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
//             foreach (var item in allObject)
//             {
//                 EditorGUILayout.BeginHorizontal();
//                 EditorGUILayout.ObjectField(item.GetType().ToString(), item, typeof(Component), allowSceneObjects: true);
//                 EditorGUILayout.EndHorizontal();
//             }
//             EditorGUILayout.EndScrollView();
//             EditorGUILayout.EndVertical();
//         }

//     }

//     private void findSprite(string resourceName)
//     {
//         allImages = new List<Image>();
//         var numScenes = SceneManager.sceneCount;
//         List<string> sceneNames = new List<string>(numScenes);
//         List<Scene> scenes = new List<Scene>();
//         for (int i = 0; i < numScenes; ++i)
//         {
//             scenes.Add(SceneManager.GetSceneAt(i));
//         }
//         foreach (var scene in scenes)
//         {
//             foreach (var root in scene.GetRootGameObjects())
//             {
//                 var images = root.GetComponentsInChildren<Image>(true);
//                 allImages.AddRange(images.FindAll(image => AssetDatabase.GetAssetPath(image.sprite).Contains(resourceName)));
//             }
//         }


//         var go = UnityEngine.Object.FindObjectOfType<GameObject>();
//         var roots = go.scene.GetRootGameObjects();
//         foreach (var root in roots)
//         {
//             var images = root.GetComponentsInChildren<Image>(true);
//             allImages.AddRange(images.FindAll(image => AssetDatabase.GetAssetPath(image.sprite).Contains(resourceName)));
//         }

//     }

//     private void findComponent(string resourceName)
//     {
//         allObject = new List<Component>();
//         var numScenes = SceneManager.sceneCount;
//         List<string> sceneNames = new List<string>(numScenes);
//         List<Scene> scenes = new List<Scene>();
//         for (int i = 0; i < numScenes; ++i)
//         {
//             scenes.Add(SceneManager.GetSceneAt(i));
//         }
//         foreach (var scene in scenes)
//         {
//             foreach (var root in scene.GetRootGameObjects())
//             {
//                 var allChild = root.GetComponentsInChildren<Transform>(true);
//                 var allGO = allChild.Select(child => child.gameObject).ToArray();
//                 findMissingReferences("", allGO, resourceName);
//             }

//         }
//     }
//     private void findMissingReferences(string context, GameObject[] gameObjects, string resourceName)
//     {
//         if (gameObjects == null)
//         {
//             return;
//         }

//         foreach (var go in gameObjects)
//         {
//             var components = go.GetComponents<Component>();

//             foreach (var component in components)
//             {
//                 // Missing components will be null, we can't find their type, etc.
//                 if (!component)
//                 {
//                     Debug.LogErrorFormat(go, $"Missing Component {0} in GameObject: {1}", component.GetType().FullName, go);

//                     continue;
//                 }

//                 SerializedObject so = new SerializedObject(component);
//                 var sp = so.GetIterator();

//                 var objRefValueMethod = typeof(SerializedProperty).GetProperty("objectReferenceStringValue",
//                     BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

//                 // Iterate over the components' properties.
//                 while (sp.NextVisible(true))
//                 {
//                     if (sp.propertyType == SerializedPropertyType.ObjectReference)
//                     {
//                         string objectReferenceStringValue = string.Empty;

//                         if (objRefValueMethod != null)
//                         {
//                             objectReferenceStringValue = (string)objRefValueMethod.GetGetMethod(true).Invoke(sp, new object[] { });
//                         }

//                         if (sp.objectReferenceValue == null
//                             && (sp.objectReferenceInstanceIDValue != 0 || objectReferenceStringValue.StartsWith("Missing")))
//                         {
//                             //ShowError(context, go, component.GetType().Name, ObjectNames.NicifyVariableName(sp.name));

//                         }
//                         if(sp.objectReferenceValue != null)
//                         {
//                             if (!sp.objectReferenceValue.name.IsNullOrEmpty() && sp.objectReferenceValue.name.Contains(resourceName))
//                             {
//                                 allObject.Add(component);
//                             }
//                         }


//                     }
//                 }
//             }
//         }
//     }


// }
