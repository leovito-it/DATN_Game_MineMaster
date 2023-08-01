using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public string vietnamese, english;

    public static string GetLanguage()
    {
        return PlayerPrefs.GetString(DEFINE.LANGUAGE, SystemLanguage.English.ToString());
    }

    public static bool IsVietnamese => GetLanguage().Equals(SystemLanguage.Vietnamese.ToString());

    private void Start()
    {
        UseLanguageText();
    }

    public void UseLanguageText()
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

    private void OnValidate()
    {
        UseLanguageText();
    }
}
