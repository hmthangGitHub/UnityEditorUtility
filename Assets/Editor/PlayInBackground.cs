using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PlayInBackground
{
    const string ITEM = "CustomTools/Play In Background";

    static PlayInBackground () {
        EditorApplication.delayCall += InitMenu;
    }

    static void InitMenu () {
        Menu.SetChecked(ITEM, Application.runInBackground);
    }

    [MenuItem(ITEM, priority = 300)]
    public static void TogglePlayInBackground () {
        Menu.SetChecked(ITEM, !Menu.GetChecked(ITEM));
        Application.runInBackground = Menu.GetChecked(ITEM);
    }

}
