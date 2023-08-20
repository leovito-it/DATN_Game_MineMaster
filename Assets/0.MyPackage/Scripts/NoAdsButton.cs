using UnityEngine;


public class NoAdsButton : MonoBehaviour
{
    const string NO_ADS = "NO_ADS";
    bool NoAds = false;

    public void OnClick()
    {

    }

    private void Start()
    {
        HideBtnOnNoAds(null);
    }

    private void HideBtnOnNoAds(object data)
    {
        if (true)
        {
            gameObject.SetActive(false);
        }
    }
}