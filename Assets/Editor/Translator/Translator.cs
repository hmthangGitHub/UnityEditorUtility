using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Translator
{
    [MenuItem("CustomTools/Translator/TranslateAll")]
    public static void TranslateAll()
    {
            // ComponentSearcher.GetAllComponentInAllScene<TextMeshProUGUI>(false).ForEach(x => x.GetOrAddComponent<AutoTranslateTextMeshProUGUI>());
    }
}
