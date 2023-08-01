using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DEFINE
{
    public static float scaleRate = 1.5f;
    public static bool isPlaying = false;

    public static string BLANK = "";
    public static string NUMBER = "Number";
    public static string PLAYER = "Player";
    public static string PLAYER1 = "Player 1";
    public static string PLAYER2 = "Player 2";

    public static string LEVEL => LanguageManager.IsVietnamese ? "Cấp" : "Level";
    public static string LANGUAGE = "Language";

    public static string SCENE_HOME = "Home";
    public static string SCENE_GAME_LIST = "GameList";

    public static string KEY_BEST = "Best";

    public static string KEY_UNLOCK_LEVEL = "UnlockLevel";
    public static string KEY_CLEAR_LEVEL = "ClearLevel";

    public static bool isOpeningSettings = false;

    public static string CurrentScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    public static void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name, LoadSceneMode.Single);

        if (GameObject.FindObjectOfType<PopupManager>() != null)
            PopupManager.Instance.CloseAll();
    }

    public static string ColorText(string txt, string colorId) => "<b><color=" + colorId + ">" + txt + "</color></b>";

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

    public static void Save(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void Save(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static void Save(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static IEnumerator ShowAndHide(GameObject obj, float duration)
    {
        obj.SetActive(true);
        yield return new WaitForSecondsRealtime(duration);
        obj.SetActive(false);
    }
}
