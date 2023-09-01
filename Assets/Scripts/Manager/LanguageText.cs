using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
    const string LANGUAGE = "Language";

    public string vietnamese, english;

    public static string GetLanguage()
    {
        return PlayerPrefs.GetString(LANGUAGE, SystemLanguage.English.ToString());
    }

    public static bool IsVietnamese => GetLanguage().Equals(SystemLanguage.Vietnamese.ToString());

    private void Start()
    {
        ChangeText();
    }

    public void ChangeText()
    {
        if (IsVietnamese)
        {
            DEFINE.SetText(GetComponent<Text>(), vietnamese);
        }
        else
        {
            DEFINE.SetText(GetComponent<Text>(), english);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ChangeText();
    }
#endif
}
