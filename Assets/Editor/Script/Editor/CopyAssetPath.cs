using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CopyAssetPath : EditorWindow
{
    [MenuItem("CustomTools/CopyAssetPath")]
    static public void CopyPath()
    {
        if(Selection.activeObject)
        {
            EditorGUIUtility.systemCopyBuffer = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        }
    }
}
