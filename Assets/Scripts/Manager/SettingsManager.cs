using SFX;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Button btnBG, btnSE;

    public GameObject disableBG, disableSE;

    private void Start()
    {
        btnBG.onClick.AddListener(() => ChangeBG());
        btnSE.onClick.AddListener(() => ChangeSE());
    }

    bool BGmute
    {
        get { return SFX_Manager.Instance.BGmute; }
        set { SFX_Manager.Instance.BGmute = value; }
    }
    bool SEmute
    {
        get { return SFX_Manager.Instance.SEmute; }
        set { SFX_Manager.Instance.SEmute = value; }
    }

    public void ChangeBG()
    {
        BGmute = !BGmute;
        OnEnable();
    }

    public void ChangeSE()
    {
        SEmute = !SEmute;
        OnEnable();
    }

    public void ChangeLanguage()
    {
        ChangeCurrentLanguage();
    }

    public void Close()
    {
        PopupManager.Instance.CloseSettings();
    }

    private void OnEnable()
    {
        disableBG.SetActive(BGmute);
        disableSE.SetActive(SEmute);
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