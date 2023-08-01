using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    public Slider sliderBG, sliderSE;

    public void ChangeBG()
    {
        AudioManager.Instance.SetVolumnBG(sliderBG.value);
    }

    public void ChangeSE()
    {
        AudioManager.Instance.SetVolumnSE(sliderSE.value);
    }

    public void ChangeLanguage()
    {
        ChangeCurrentLanguage();
    }

    private void OnEnable()
    {
        sliderBG.value = AudioManager.Instance.BGvol;
        sliderSE.value = AudioManager.Instance.SEvol;
    }

    static void ChangeCurrentLanguage()
    {
        DEFINE.Save(DEFINE.LANGUAGE, LanguageManager.IsVietnamese ?
            SystemLanguage.English.ToString() : SystemLanguage.Vietnamese.ToString());

        foreach (LanguageManager lm in FindObjectsOfType<LanguageManager>())
        {
            lm.UseLanguageText();
        }

#if UNITY_EDITOR
        List<GameObject> objects = new List<GameObject>();
        foreach (Object obj in FindObjectsOfType(typeof(LanguageManager)))
        {
            objects.Add(GameObject.Find(obj.name));
        }
        Selection.objects = objects.ToArray();

        foreach (Object obj in Selection.objects)
        {
            EditorGUIUtility.PingObject(obj);
        }
#endif
    }

#if UNITY_EDITOR
    [MenuItem("Tools/ChangeLanguage")]
    static void ChangeLanguageEditor()
    {
        ChangeCurrentLanguage();
    }
#endif
}