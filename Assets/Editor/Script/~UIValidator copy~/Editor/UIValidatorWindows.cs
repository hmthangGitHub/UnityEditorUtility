using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using OniUI;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class UIValidatorWindows : EditorWindow
{
    [MenuItem("Window/UIValidatorWindows")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(UIValidatorWindows));
    }
    public enum TestCase
    {
        RoundingRectTransform,
        OverridingOnSet,
        ResetAnimation,
        UsingCanvasSortingOrder,
        UsingSePlayer,
        TextMeshProValidate,
        NotUsingPlaceHolderImage,
        WarningXIUITextNumFormat,
        MoveCustomScriptToDestinationFolder,
        CheckIfUsingVariableList,
        WarningXIUIFrameUsingRightFormat,
        MissingScript
    }

    public enum Result
    {
        None,
        Fail,
        Pass,
        Warning,
        NotImplemented
    }
    public class TestCaseClass
    {
        public Func<Result> preTest;
        public Func<Result> test;
        public Result result;
        public void ExcuteTest()
        {
            try
            {
                result = preTest.Invoke();
                if (result == Result.Pass)
                {
                    result = test.Invoke();
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
                result = Result.Fail;
            }

        }
    }

    private Dictionary<TestCase, TestCaseClass> testcases;

    private void Awake()
    {
        testcases = new Dictionary<TestCase, TestCaseClass>();

        AddTestCase(TestCase.RoundingRectTransform, RoundingRectTransform);
        AddTestCase(TestCase.OverridingOnSet, OverridingOnSet);
        AddTestCase(TestCase.ResetAnimation, ResetAnimation);
        AddTestCase(TestCase.UsingCanvasSortingOrder, UsingCanvasSortingOrder);
        AddTestCase(TestCase.UsingSePlayer, UsingSePlayer);
        AddTestCase(TestCase.TextMeshProValidate, TextMeshProValidate);
        AddTestCase(TestCase.NotUsingPlaceHolderImage, NotUsingPlaceHolderImage);
        AddTestCase(TestCase.WarningXIUITextNumFormat, WarningXIUITextNumFormat);
        AddTestCase(TestCase.MoveCustomScriptToDestinationFolder, MoveCustomScriptToDestinationFolder);
        AddTestCase(TestCase.CheckIfUsingVariableList, NotImplemented);
        AddTestCase(TestCase.WarningXIUIFrameUsingRightFormat, WarningXIUIFrameUsingRightFormat);
        AddTestCase(TestCase.MissingScript, MissingScript);
    }

    private Result MoveCustomScriptToDestinationFolder()
    {
        bool IsDirectoryEmpty(string path)
        {
            string[] vs = Directory.GetFiles(path);
            return !vs.Any(x => x != $"{path}.cs" && x != $"{path}.gitkeep");
        }
        var xiuiFolder = $"{Application.dataPath}/Tempest/UI/XIUI/";

        if(!IsDirectoryEmpty(xiuiFolder))
        {
            var assetFolder = AssetDatabase.LoadAssetAtPath("Assets/Tempest/UI/XIUI", typeof(UnityEngine.Object));
            LogErrorWithTag(TestCase.MoveCustomScriptToDestinationFolder, "Not move custom UI script into destination folder", assetFolder);
            return Result.Fail;
        }
        return Result.Pass;
    }
    private Result MissingScript()
    {
        var roots = Selection.activeGameObject.scene.GetRootGameObjects();
        var allGo = new List<GameObject>();
        foreach (var item in roots)
        {
            allGo.AddRange(item.GetComponentsInChildren<Transform>(true).Select(x => x.gameObject));
        }
        bool flag = true;
        foreach (var item in allGo)
        {
            if (item.GetComponents<Component>().Any(x => x == null))
            {
                flag = false;
                LogErrorWithTag(TestCase.MissingScript, $"{item.name} has missing script", item);
            }
        }
        return flag ? Result.Pass : Result.Fail;
    }
    private Result CheckIfUsingVariableList()
    {
        var go = Selection.activeGameObject;
        var uiCustomClass = go.GetComponent<UIClassBase>();
        Assembly assembly = Assembly.GetAssembly(typeof(TestScript));

        // bool isUsingVariableList(XIUICustomBase customBase)
        // {
        //     customBase.
        //     return true;
        // }

        var xiUICustomBaseClass = go.GetComponentsInChildren<XIUICustomBase>(true);


        var type = uiCustomClass.GetType();
        var entityType = assembly.GetType($"OniUI.{uiCustomClass.gameObject.name}.Entity");
        bool flag = true;

        Type repositoryType = typeof(OniUI.XIUIDataListBase<,>).MakeGenericType(type, entityType);

        var allXIUIList = Selection.activeGameObject.GetComponentsInChildren(repositoryType, true);

        foreach (var item in allXIUIList)
        {
            // item.item
        }
        var repository = Activator.CreateInstance(repositoryType);


        return flag ? Result.Pass : Result.Warning;
    }
    private Result WarningXIUIFrameUsingRightFormat()
    {
        var allXIUIFrame = Selection.activeGameObject.GetComponentsInChildren<OniUI.XIUIFrame>(true);
        bool flag = true;
        foreach (var item in allXIUIFrame)
        {
            if (!item.enumTypeString.Contains(Selection.activeGameObject.name))
            {
                LogErrorWithTag(TestCase.WarningXIUIFrameUsingRightFormat, $"<color=#FF00FF>{item.gameObject.name}</color> not using internal Enum", item);
                flag = false;
            }
        }
        return flag ? Result.Pass : Result.Warning;
    }

    private Result CheckIfMoveCustomScriptToDestinationFolder()
    {
        return Result.Pass;
    }

    private Result WarningXIUITextNumFormat()
    {
        var allXIText = Selection.activeGameObject.GetComponentsInChildren<OniUI.XIUIText>(true);
        bool CheckNumFormat(OniUI.XIUIText text)
        {
            if (text.category == XIUIText.TextCategory.Num)
            {
                if (text.FormatText == "{0:#,0}")
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        bool flag = true;
        foreach (var item in allXIText)
        {
            if (!CheckNumFormat(item))
            {
                LogErrorWithTag(TestCase.WarningXIUITextNumFormat, $"<color=#FF00FF>{item.gameObject.name}</color> category == Num not using 0:#,0 format", item);
                flag = false;
            }
        }
        return flag ? Result.Pass : Result.Warning;
    }
    private void AddTestCase(TestCase testCase, Func<Result> func)
    {
        testcases[testCase] = new TestCaseClass()
        {
            preTest = CheckIfSelectedObjectIsUIClassBase,
            test = func,
            result = Result.None
        };
    }

    private Result NotUsingPlaceHolderImage()
    {
        var allImages = Selection.activeGameObject.scene.GetRootGameObjects().Select(x => x.GetComponentsInChildren<Image>(true));

        bool flag = true;
        foreach (var item in allImages)
        {
            foreach (var image in item)
            {
                var path = AssetDatabase.GetAssetPath(image.sprite);
                if (!path.StartsWith("Assets/AssetBundles") && !string.IsNullOrEmpty(path))
                {
                    LogErrorWithTag(TestCase.NotUsingPlaceHolderImage, $"Image <color=#FF00FF>{image.gameObject.name}</color> not using asset bundle resource! : " + path, image);
                    flag = false;
                }
            }
        }
        return flag ? Result.Pass : Result.Fail;
    }

    private Result UsingCanvasSortingOrder()
    {
        if (Selection.activeGameObject.GetComponent<Tempest.CanvasSortingOrder>() == null)
        {
            LogErrorWithTag(TestCase.UsingCanvasSortingOrder, $"{Selection.activeGameObject.name} not using  CanvasSortingOrder", Selection.activeGameObject);
            return Result.Fail;
        }
        return Result.Pass;
    }

    private Result TextMeshProValidate()
    {
        var allTextMesh = Selection.activeGameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);
        bool IsUsingAutoSize(TMPro.TextMeshProUGUI textMesh)
        {
            return textMesh.enableAutoSizing;
        }

        bool IsUsingAutoSizeRightFormat(TMPro.TextMeshProUGUI textMesh)
        {
            return textMesh.fontSizeMin == 10;
        }

        bool IsUsingAutoSizeRightFormatRound(TMPro.TextMeshProUGUI textMesh)
        {
            return textMesh.fontSize % 1 == 0;
        }

        bool IsUsingValidSpacing(TMPro.TextMeshProUGUI textMesh)
        {
            return textMesh.characterSpacing == 0.0f && textMesh.wordSpacing == 0.0f && textMesh.lineSpacing == 0.0f && textMesh.paragraphSpacing == 0.0f;
        }

        bool flag = true;
        foreach (var item in allTextMesh)
        {
            if (!IsUsingAutoSize(item))
            {
                flag = false;
                LogErrorWithTag(TestCase.TextMeshProValidate, $"{item.gameObject.name} not using auto size", item);
                continue;
            }

            if (!IsUsingAutoSizeRightFormat(item))
            {
                flag = false;
                LogErrorWithTag(TestCase.TextMeshProValidate, $"{item.gameObject.name} not using auto size min size not 10", item);
                continue;
            }

            if (!IsUsingAutoSizeRightFormatRound(item))
            {
                flag = false;
                LogErrorWithTag(TestCase.TextMeshProValidate, $"{item.gameObject.name} not using rounding font size", item);
                continue;
            }

            if (!IsUsingValidSpacing(item))
            {
                flag = false;
                LogErrorWithTag(TestCase.TextMeshProValidate, $"{item.gameObject.name} using font space", item);
                continue;
            }
        }

        return flag ? Result.Pass : Result.Fail;

    }

    private Result UsingSePlayer()
    {
        if (Selection.activeGameObject.GetComponent<OniUI.XIUISEPlayer>() != null)
        {
            return Result.Pass;
        }
        else
        {
            LogErrorWithTag(TestCase.UsingSePlayer, $"{Selection.activeGameObject.name} not using UsingSePlayer", Selection.activeGameObject);
            return Result.Fail;
        }
    }

    private void DoRounding()
    {
        var gameObject = Selection.activeGameObject;
        var allRectTransform = gameObject.GetComponentsInChildren<RectTransform>(true);

        foreach (var item in allRectTransform)
        {
            item.RoundRectTransformValue();
        }
    }

    private Result NotImplemented()
    {
        return Result.NotImplemented;
    }

    private Result RoundingRectTransform()
    {
        DoRounding();
        return Result.Pass;
    }

    private Result OverridingOnSet()
    {
        var uiBaseClasss = Selection.activeGameObject.GetComponent<OniUI.UIClassBase>();
        Assembly assembly = Assembly.GetAssembly(typeof(OniUI.UIForgeEquipList));
        var type = assembly.GetType($"OniUI.{uiBaseClasss.gameObject.name}");
        if (type.GetMethod("OnSet", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).DeclaringType != type)
        {
            LogErrorWithTag(TestCase.OverridingOnSet, $"{Selection.activeGameObject.name} Not overriding OnSet", Selection.activeGameObject);
            return Result.Fail;
        }
        return Result.Pass;
    }

    private Result CheckIfSelectedObjectIsUIClassBase()
    {
        var go = Selection.activeGameObject;
        if (go == null)
        {
            throw new Exception("Please select a UIClassBase GameObject to Validate");
        }
        var uiClassBase = go.GetComponent<OniUI.UIClassBase>();
        if (uiClassBase == null)
        {
            throw new Exception("Please select a UIClassBase GameObject to Validate");
        }
        return Result.Pass;
    }

    private Result ResetAnimation()
    {
        var uiBaseClass = Selection.activeGameObject.GetComponent<OniUI.UIClassBase>();
        var animators = GetPropertyValue(uiBaseClass, "animationModule.resetAnimatorList") as Animator[];
        if (animators.Length > 0)
        {
            return Result.Pass;
        }
        else
        {
            LogErrorWithTag(TestCase.ResetAnimation, "Please register resetAnimatorList", uiBaseClass);
            return Result.Fail;
        }
    }

    public object GetPropertyValue(object obj, string propertyName)
    {
        var _propertyNames = propertyName.Split('.');

        for (var i = 0; i < _propertyNames.Length; i++)
        {
            if (obj != null)
            {
                var _propertyInfo = obj.GetType().GetProperty(_propertyNames[i], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (_propertyInfo != null)
                    obj = _propertyInfo.GetValue(obj);
                else
                {
                    var fieldInfo = obj.GetType().GetField(_propertyNames[i], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (fieldInfo != null)
                    {
                        obj = fieldInfo.GetValue(obj);
                    }
                    else
                    {
                        obj = null;
                    }
                }


            }
        }

        return obj;
    }


    void OnGUI()
    {
        if (testcases == null)
        {
            this.Awake();
        }

        GUILayout.BeginVertical();
        foreach (var item in testcases)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(item.Key.ToString(), GUILayout.Width(300)))
            {
                item.Value.ExcuteTest();
            }
            if (item.Value.result != Result.None)
            {
                switch (item.Value.result)
                {
                    case Result.Pass:
                    case Result.NotImplemented:
                        GUILayout.Label(item.Value.result.ToString());
                        break;
                    case Result.Fail:
                    case Result.Warning:
                        if(GUILayout.Button(item.Value.result.ToString()))
                        {
                            ForcusConsoleWithTag($"[{item.Key.ToString()}]");
                        }
                        break;
                    default:
                        break;
                }

            }
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Test All", GUILayout.Width(300)))
        {
            foreach (var item in testcases)
            {
                item.Value.ExcuteTest();
            }
        }
        GUILayout.EndVertical();
    }

    private void LogErrorWithTag(TestCase tag, string content, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError($"[{tag.ToString()}] {content}", context);
    }


    private void ForcusConsoleWithTag(string tag)
    {
        var allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        var console = allWindows.FirstOrDefault(x => x.GetType().ToString() == "UnityEditor.ConsoleWindow");
        var field = console.GetType().GetMethod("SetFilter", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        console.Focus();
        field.Invoke(console, new object[] {tag});
    }

}
