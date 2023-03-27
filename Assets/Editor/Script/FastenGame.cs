using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FastenGame : MonoBehaviour
{

#if UNITY_EDITOR
    private static GameObject fasternObject;
    [MenuItem("CustomTools/FastenGame %Q")]
    public static void FasternTheGame()
    {
        if (Application.isPlaying)
        {
            if (fasternObject == null)
            {
                fasternObject = new GameObject("FasternTheGame");
                fasternObject.AddComponent<FastenGame>();
                DontDestroyOnLoad(fasternObject);
            }
            else
            {
                fasternObject.SetActive(!fasternObject.activeSelf);
                if (!fasternObject.activeSelf)
                {
                    Time.timeScale = 1;
                }
            }

        }
    }
#endif
    // Start is called before the first frame update
    public float speed = 0.1f;
    // Update is called once per frame
    void Update()
    {
        Time.timeScale = speed;
    }

    [MenuItem("CustomTools/Modify Damage")]
    public static void modifyDamage()
    {
        var modifed = PlayerPrefs.GetInt("MODIFIED_DAMGE");
        PlayerPrefs.SetInt("MODIFIED_DAMGE", modifed > 0 ? 0 : 1);
    }

}
