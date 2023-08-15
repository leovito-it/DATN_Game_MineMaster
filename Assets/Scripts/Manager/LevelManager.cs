using SFX;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Rule
{
    public List<Vector2> defined;

    public Vector2 modify; // gia tri thay doi
    public Vector2 limit; // gia tri gioi han
}

#if UNITY_EDITOR
public static class DataCleaner
{
    [MenuItem("Tools/Reset level data this minigame")]
    public static void ResetLevelData()
    {
        PlayerPrefs.DeleteKey(DEFINE.CurrentScene + DEFINE.LEVEL);
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Reset all data")]
    public static void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
#endif

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance => GameObject.Find("LevelManager").GetComponent<LevelManager>();

    public Text txtLv, txtProcess;

    public GameObject info, congratulations, failed;

    public Text title, text1, text2;

    public AudioClip failClip, successClip;

    public int Level => GetLevel();
    float m_var1, m_var2;

    static bool showInfo = false;
    static bool showCongratulations = false;

    private void Start()
    {
        DEFINE.SetText(txtLv, "LV. " + Level);

        Reset();

        if (showInfo)
            Invoke(nameof(ShowInfo), showCongratulations ? 3 : 0);
        else
            CloseInfo();

        if (showCongratulations)
        {
            StartCoroutine(DEFINE.ShowAndHide(congratulations, 3));
            showCongratulations = false;
            SFX_Manager.Instance.PlaySEOneShot(successClip);
        }
    }

    public void SetProcess(int current, int final)
    {
        DEFINE.SetText(txtProcess, $"{current} / {final}");
    }

    public void SetInfo(string varName1, string varName2)
    {
        DEFINE.SetText(title, DEFINE.LEVEL + " " + GetLevel());
        DEFINE.SetText(text1, varName1 + ": " + DEFINE.ColorText(m_var1 + "", "#f00"));
        DEFINE.SetText(text2, varName2 + ": " + DEFINE.ColorText(m_var2 + "", "#f00"));
    }

    public void ShowInfo()
    {
        info.SetActive(true);

        DEFINE.isPlaying = false;
    }

    public void CloseInfo()
    {
        info.SetActive(false);
        showInfo = false;

        DEFINE.isPlaying = true;
    }

    public void Failed()
    {
        failed.SetActive(true);
        SFX_Manager.Instance.PlaySEOneShot(failClip);
    }

    public void UnlockNextLevel()
    {
        NextLevel();

        showInfo = true;
        showCongratulations = true;
        DEFINE.LoadScene(DEFINE.CurrentScene);
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt(DEFINE.CurrentScene + DEFINE.LEVEL, 1);
    }

    void SaveLevel()
    {
        DEFINE.Save(DEFINE.CurrentScene + DEFINE.LEVEL, Level);
    }

    void NextLevel()
    {
        DEFINE.Save(DEFINE.CurrentScene + DEFINE.LEVEL, Level + 1);
    }

    public void UpdateVars(ref float var1, ref float var2, Rule rule)
    {
        float old1 = var1, old2 = var2;
        // Get var
        if (rule.defined.Count > Level - 1)
        {
            var1 = rule.defined[Level - 1].x;
            var2 = rule.defined[Level - 1].y;
        }
        else
        {
            int maxLevelDefined = rule.defined.Count;

            var1 = rule.defined[maxLevelDefined - 1].x + (Level - maxLevelDefined) * rule.modify.x;
            var2 = rule.defined[maxLevelDefined - 1].y + (Level - maxLevelDefined) * rule.modify.y;
        }

        // Limit var
        var1 = rule.modify.x > 0 ? Mathf.Min(var1, rule.limit.x) : Mathf.Max(var1, rule.limit.x);
        var2 = rule.modify.y > 0 ? Mathf.Min(var2, rule.limit.y) : Mathf.Max(var2, rule.limit.y);

        m_var1 = var1;
        m_var2 = var2;
    }

    public void Reset()
    {
        info.SetActive(false);
        failed.SetActive(false);
        congratulations.SetActive(false);
    }
}
