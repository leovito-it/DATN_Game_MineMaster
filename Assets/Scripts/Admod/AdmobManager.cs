using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEditor;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager Instance;

    private BannerView bannerView;
    private RewardedAd rewarded;

    const string androidId = "ca-app-pub-9711240182969577~8123215217";
    const string iOSId = "ca-app-pub-9711240182969577~7859491331";

    const string androidBannerId = "ca-app-pub-9711240182969577/7957647966";
    const string iOSBannerId = "ca-app-pub-9711240182969577/9625952853";

    [TextArea(6,20)]
    [SerializeField] string BuildNote = $"Android: {androidId} \nIOS: {iOSId} \n\nAndroid BannerId: {androidBannerId} \nIOS BannerId: {iOSBannerId}";

    [Header("Banner Ads")]
    [SerializeField] string bannerIdAndroid ;
    [SerializeField] string bannerIdIOS;

    [Header("Reward Ads")]
    [SerializeField] string rewardIdAndroid;
    [SerializeField] string rewardIdIOS;

    [HideInInspector]
    public bool IsUserEarnedReward = false;
    [HideInInspector]
    public bool IsRewardLoaded = false;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        RequestConfiguration configuration = new RequestConfiguration.Builder().build();
        MobileAds.SetRequestConfiguration(configuration);
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
        RequestReward();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = bannerIdAndroid;
#elif UNITY_IPHONE
        string adUnitId = bannerIdIOS;
#else
        string adUnitId = "unexpected_platform";
#endif

        //AdSize adaptiveSize;
        //if (Screen.width / Screen.height * 1080 > 1920)
        //{
        //    int adsWidth = Screen.height * 1920 / 1080;
        //    int width = (int)(adsWidth / MobileAds.Utils.GetDeviceScale());
        //    adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(width);
        //}
        //else
        //{
        //    adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        //}

        // Create a 320x50 banner at the bot of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner , AdPosition.Bottom);
        this.bannerView.OnAdFailedToLoad += this.HandleOnBannerAdFailToLoad;
        this.bannerView.OnAdLoaded += this.HandleOnBannerAdLoaded;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    private void RequestReward()
    {
#if UNITY_ANDROID
        string adUnitId = rewardIdAndroid;
#elif UNITY_IPHONE
        string adUnitId = rewardIdIOS;
#else
        string adUnitId = "unexpected_platform";
#endif
        rewarded = new RewardedAd(adUnitId);
        rewarded.OnAdLoaded += HandleOnRewardAdLoaded;
        rewarded.OnAdClosed += HandleOnRewardAdClosed;
        rewarded.OnUserEarnedReward += HanldeOnUserEarnedReward;
        rewarded.OnAdFailedToLoad += HandleOnAdFalseToLoad;
        rewarded.OnAdFailedToShow += HandleOnAdFalseToShow;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        rewarded.LoadAd(request);
    }

    private void HandleOnBannerAdFailToLoad(object sender, EventArgs args)
    {
        RequestBanner();
    }

    private void HandleOnBannerAdLoaded(object sender, EventArgs args)
    {
        ShowBanner(false);
    }

    private void HandleOnRewardAdLoaded(object sender, EventArgs args)
    {
        IsRewardLoaded = true;
    }

    private void HandleOnRewardAdClosed(object sender, EventArgs args)
    {
        // request other reward
        RequestReward();
    }

    private void HanldeOnUserEarnedReward(object sender, EventArgs args)
    {
        // Unlock book
        UserEarnReward();
    }

    private void HandleOnAdFalseToLoad(object sender, EventArgs args)
    {
        // reload ads
        RequestReward();
    }

    private void HandleOnAdFalseToShow(object sender, EventArgs args)
    {
        if (IsRewardLoaded)
            UserEarnReward();
    }

    private void UserEarnReward()
    {
        IsUserEarnedReward = true;
        //Add function here

    }

    public void ShowRewardAds()
    {
        if (IsRewardLoaded)
        {
            Debug.Log("------ REWARD IS PLAYING --------");
            rewarded.Show();
        }
    }

    public void ShowBanner(bool isShow)
    {
        if (isShow) {
            if( GameObject.Find("BANNER(Clone)") == null)
            this.bannerView.Show();
        }
        else {
            if (bannerView != null )
            this.bannerView.Hide();
        }
    }
}
