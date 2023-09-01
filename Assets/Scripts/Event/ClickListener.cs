using UnityEngine;

public class ClickListener : MonoBehaviour
{
    public void BackToGameList()
    {
        Scenes.DressUp.LoadScene();
    }

    public void BackToHome()
    {
        Scenes.Home.LoadScene();
    }

    public void ReloadLevel()
    {
        //load scene
        DEFINE.LoadScene(DEFINE.SceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        PopupManager.Instance.OpenSettings();
    }

    public void CloseSettings()
    {
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
}