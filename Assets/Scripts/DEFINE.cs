using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DEFINE
{
    public static GameStatus Status;
    public enum GameStatus { Win, Lose, Skipped, Playing, Waiting, Tutorial }

    #region SCENE
    public static string SceneName => SceneManager.GetActiveScene().name;

    public static Scenes LastScene = Scenes.None;
    public static Scenes CurrentScene = Scenes.Loading;

    public static void LoadScene(this Scenes scene)
    {
        LastScene = CurrentScene;
        CurrentScene = scene;
        LoadScene($"{scene}");
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);

        if (GameObject.FindObjectOfType<PopupManager>() != null)
            PopupManager.Instance.CloseAll();
    }
    #endregion

    #region TEXT
    public static string Format_LableStyle(this string input)
    {
        // Tìm và tách cả phần số cuối chuỗi
        string numericPart = Regex.Match(input, @"\d+$").Value;
        if (numericPart.Length != 0)
            input = input.Replace(numericPart, "").Trim();

        // Sử dụng Regex để tìm các từ hoặc chữ in hoa
        string formatted = Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");

        // Chuyển chữ cái đầu tiên thành chữ in hoa
        formatted = char.ToUpper(formatted[0]) + formatted[1..];

        // Kết hợp với phần số nếu có
        if (!string.IsNullOrEmpty(numericPart))
        {
            formatted += " " + numericPart;
        }

        return formatted;
    }

    public static string SetColor(this string txt, string colorId) => $"<b><color=#{colorId}>{txt}</color></b>";

    public static TextMeshProUGUI SetText(this TextMeshProUGUI txt, string content)
    {
        if (txt != null)
            txt.text = content;
        return txt;
    }

    public static Text SetText(this Text txt, string content)
    {
        if (txt != null)
            txt.text = content;
        return txt;
    }

    public static TextMeshProUGUI Clear(this TextMeshProUGUI txt, bool hide = false)
    {
        txt.SetText("");
        if (hide)
            txt.gameObject.SetActive(false);
        return txt;
    }

    public static Text Empty(this Text txt, bool hide = false)
    {
        txt.SetText("");
        if (hide)
            txt.gameObject.SetActive(false);
        return txt;
    }
    #endregion

    public static void SaveKey(this string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void SaveKey(this string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static void SaveKey(this string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static void SaveText(this string message, string fileName)
    {

    }
}
