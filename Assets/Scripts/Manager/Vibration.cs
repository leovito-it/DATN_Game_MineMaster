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
        set { PlayerPrefs.SetInt(VIBRATION, value ? 1 : 0); PlayerPrefs.Save(); }
    }

    // Hàm để rung thiết bị
    public static void Vibrate()
    {
        // Kiểm tra nếu thiết bị hỗ trợ rung
        if (SystemInfo.supportsVibration && Enable)
        {
            // Rung thiết bị
            Handheld.Vibrate();
        }
    }

    public static void Vibrate(int strong)
    {
        Vibrate();
    }

    public static void Peek()
    {
        Vibrate();
    }
}