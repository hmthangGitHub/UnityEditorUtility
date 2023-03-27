using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public static class DefaultScenePlayer
{
    static DefaultScenePlayer()
    {
        if (EditorPrefs.GetBool("PLAY_DEFAULT_SCENE"))
        {
            TriggerPlayDefaultScene();
        }

        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    private static void TriggerPlayDefaultScene()
    {
        EditorPrefs.SetBool("PLAY_DEFAULT_SCENE", true);
        EditorApplication.playModeStateChanged += PlayDefaultScene;
        if (EditorBuildSettings.scenes.Any()) {
            var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
            EditorSceneManager.playModeStartScene = sceneAsset;
        }
    }

    static void OnToolbarGUI()
    {
    	GUILayout.FlexibleSpace();

        if (EditorPrefs.GetBool("PLAY_DEFAULT_SCENE") == true)
        {
            if(GUILayout.Button(new GUIContent("1st", "Play default scene")))
            {
                TogglePlayDefaultScene();
            }
        }
    }

    static void PlayDefaultScene(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
    }

    [MenuItem("CustomTools/Play default scene #P")] // Shift P
    static void TogglePlayDefaultScene()
    {
        if (EditorPrefs.GetBool("PLAY_DEFAULT_SCENE") == true)
        {
            TriggerOffPlayDefaultScene();
        }
        else
        {
            TriggerPlayDefaultScene();
        }
    }

    private static void TriggerOffPlayDefaultScene()
    {
        EditorPrefs.SetBool("PLAY_DEFAULT_SCENE", false);
        Debug.Log("Play default scene : false");

        EditorApplication.playModeStateChanged -= PlayDefaultScene;
        EditorSceneManager.playModeStartScene = null;
    }
}
