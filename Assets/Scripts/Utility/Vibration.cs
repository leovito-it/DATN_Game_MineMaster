#if UNITY_IOS
using System.Collections;
using System.Runtime.InteropServices;
#endif

using UnityEngine;

public class Vibration
{
    public const string VIBRATION = "Vibration";

    public static bool Enable
    {
        get { return PlayerPrefs.GetInt(VIBRATION, 1) == 1; }
        set { DEFINE.SaveKey(VIBRATION, value ? 1 : 0); }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void Vibrate()
    {
        if (!Enable)
            return;

        if (IsAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }

    public static void Vibrate(long milliseconds)
    {
        if (!Enable)
            return;

        if (IsAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    public static void Peek()
    {
        if (!Enable)
            return;

        long[] pattern = { 0, 100 };
        if (IsAndroid())
            vibrator.Call("vibrate", pattern, -1);
        else
            Handheld.Vibrate();
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (!Enable)
            return;

        if (IsAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }

    public static bool HasVibrator()
    {
        return IsAndroid();
    }

    public static void Cancel()
    {
        if (IsAndroid())
            vibrator.Call("cancel");
    }

    private static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
  return true;
#else
        return false;
#endif
    }
}