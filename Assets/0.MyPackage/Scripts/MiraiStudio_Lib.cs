using UnityEngine;

namespace _0._MiraiStudio.Scripts
{
    public class MiraiStudio_Lib : Singleton<MiraiStudio_Lib>
    {
        public string packageName = "";

        public void Rate()
        {
#if UNITY_IPHONE
        if (!string.IsNullOrEmpty(appid))
            Application.OpenURL("https://apps.apple.com/app/id" + appid + "?action=write-review");
#elif UNITY_WP8
			ShowRate.Rate rate = new ShowRate.Rate();
			rate.ShowMarket();
#else
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + this.packageName);
#endif
        }
    }
}