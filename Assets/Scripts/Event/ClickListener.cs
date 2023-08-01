using UnityEngine;
using UnityEngine.UI;

public class ClickListener : MonoBehaviour
{
    Num100Controller manager => FindObjectOfType<Num100Controller>();

    public void BackToGameList()
    {
        LoadLevel(DEFINE.SCENE_GAME_LIST);
    }

    public void BackToHome()
    {
        LoadLevel(DEFINE.SCENE_HOME);
    }

    public void LoadLevel(string scene)
    {
        //load scene
        DEFINE.LoadScene(scene);
    }

    public void ReloadLevel()
    {
        //load scene
        DEFINE.LoadScene(DEFINE.CurrentScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EndGame()
    {
        manager.SetEndGGame(true);
    }

    public void OpenSettings()
    {
        DEFINE.isPlaying = false;
        PopupManager.Instance.OpenSettings();
    }

    public void CloseSettings()
    {
        DEFINE.isPlaying = true;
        PopupManager.Instance.CloseSettings();
    }

    public void ShowRewardAds()
    {
        AdmobManager.Instance.ShowRewardAds();
    }

    public void ShowBannerAds()
    {
        AdmobManager.Instance.ShowBanner(true);
    }

    public void PauseGame()
    {
        DEFINE.isPlaying = false;
    }

    public void UnPauseGame()
    {
        DEFINE.isPlaying = true;
    }
}