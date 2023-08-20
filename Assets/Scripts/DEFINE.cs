using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DEFINE
{
    public static bool isPlaying = false;
    public static bool isOpeningSettings = false;

    #region NAME
    public const string BLANK = "";
    public const string NUMBER = "Number";
    public const string PLAYER = "Player";
    public const string PLAYER1 = "Player 1";
    public const string PLAYER2 = "Player 2";
    #endregion

    #region SCENE
    public const string SCENE_HOME = "Home";
    public const string SCENE_GAME_LIST = "GameList";
    #endregion

    public static string LEVEL => LanguageManager.IsVietnamese ? "Cấp" : "Level";
    public const string LANGUAGE = "Language";


    public static string CurrentScene => SceneManager.GetActiveScene().name;

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);

        if (GameObject.FindObjectOfType<PopupManager>() != null)
            PopupManager.Instance.CloseAll();
    }

    public static string SetColor(this string txt, string colorId) => $"<b><color={colorId}>{txt}</color></b>";

    public static void SetText(TextMeshProUGUI txtObj, string txt)
    {
        if (txtObj != null)
            txtObj.text = txt;
    }

    public static void SetText(Text txtObj, string txt)
    {
        if (txtObj != null)
            txtObj.text = txt;
    }

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

    public static string Format(this string input)
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
}
