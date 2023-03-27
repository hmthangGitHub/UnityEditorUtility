using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ComponentSearcher
{
    public static T[] GetAllComponentInAllScene<T>(bool includeInActiveGameObject) where T : Component
    {
        var result = new List<T>();
        var countLoaded = SceneManager.sceneCount;
        var loadedScenes = new Scene[countLoaded];

        for (var i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }

        foreach (var item in loadedScenes)
        {
            var rootObjects = item.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                result.AddRange(rootObject.GetComponentsInChildren<T>(includeInActiveGameObject));
            }
        }

        if(Application.isPlaying)
        {
            var temp = new GameObject();
            Object.DontDestroyOnLoad(temp);

            var rootObjs = temp.scene.GetRootGameObjects();
            foreach (var rootObject in rootObjs)
            {
                result.AddRange(rootObject.GetComponentsInChildren<T>(includeInActiveGameObject));
            }

            Object.DestroyImmediate(temp);
        }
        return result.ToArray();
    }
}
