using SFX;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct Rule
{
    public List<Vector2> Defined;

    public Vector2 Modify; // gia tri thay doi
    public Vector2 Limit; // gia tri gioi han
}

public class LevelManager : Singleton<LevelManager>
{
    const string LEVEL = "Level";

    [SerializeField] TextMeshProUGUI txtLv, txtProcess;

    [SerializeField] GameObject info, congratulations, failed;

    [SerializeField] TextMeshProUGUI title, text1, text2;

    [SerializeField] AudioClip failClip, successClip;

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
        DEFINE.SetText(title, LEVEL + " " + GetLevel());
        DEFINE.SetText(text1, varName1 + ": " + DEFINE.SetColor(m_var1 + "", "f00"));
        DEFINE.SetText(text2, varName2 + ": " + DEFINE.SetColor(m_var2 + "", "f00"));
    }

    public void ShowInfo()
    {
        info.SetActive(true);

        DEFINE.Status = DEFINE.GameStatus.Waiting;
    }

    public void CloseInfo()
    {
        info.SetActive(false);
        showInfo = false;

        DEFINE.Status = DEFINE.GameStatus.Playing;
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
        DEFINE.LoadScene(DEFINE.SceneName);
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt(DEFINE.SceneName + LEVEL, 1);
    }

    void SaveLevel()
    {
        DEFINE.SaveKey(DEFINE.SceneName + LEVEL, Level);
    }

    void NextLevel()
    {
        DEFINE.SaveKey(DEFINE.SceneName + LEVEL, Level + 1);
    }

    public void UpdateVars(ref float var1, ref float var2, Rule rule)
    {
        float old1 = var1, old2 = var2;
        // Get var
        if (rule.Defined.Count > Level - 1)
        {
            var1 = rule.Defined[Level - 1].x;
            var2 = rule.Defined[Level - 1].y;
        }
        else
        {
            int maxLevelDefined = rule.Defined.Count;

            var1 = rule.Defined[maxLevelDefined - 1].x + (Level - maxLevelDefined) * rule.Modify.x;
            var2 = rule.Defined[maxLevelDefined - 1].y + (Level - maxLevelDefined) * rule.Modify.y;
        }

        // Limit var
        var1 = rule.Modify.x > 0 ? Mathf.Min(var1, rule.Limit.x) : Mathf.Max(var1, rule.Limit.x);
        var2 = rule.Modify.y > 0 ? Mathf.Min(var2, rule.Limit.y) : Mathf.Max(var2, rule.Limit.y);

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
