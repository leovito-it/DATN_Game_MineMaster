using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : Singleton<Timer>
{
    [SerializeField] Color cdbarColor, textColor;

    public static float currentTime = 0f;
    public static float remainingTime = 0f;

    [SerializeField] TextMeshProUGUI txtTime;
    [SerializeField] Image countdownBar;

    [Header("Alert")]
    [SerializeField] Image alertObj;
    [SerializeField] float timeAlert = 10f;

    bool alertShowing = false;

    public void Run()
    {
        Run(null);
    }

    public void Run(Action callback)
    {
        Reset();
        _ = StartCoroutine(TimerStart(0, callback));
    }

    public void RunCountDown(float time)
    {
        RunCountDown(time, null);
    }

    public void RunCountDown(float time, Action callback)
    {
        Reset();
        _ = StartCoroutine(TimerStart(time, callback));
    }

    IEnumerator TimerStart(float cdTime, Action callback)
    {
        remainingTime = cdTime;

        while (true)
        {
            if (DEFINE.Status == DEFINE.GameStatus.Playing)
            {
                currentTime += Time.fixedDeltaTime;
                remainingTime = cdTime - currentTime;

                if (remainingTime <= 0)
                {
                    remainingTime = 0;
                }

                if (txtTime != null)
                {
                    txtTime.text = ToString(cdTime > 0 ? remainingTime : currentTime);
                }

                if (cdTime > 0)
                {
                    SetCountdownBar(cdTime);

                    if (remainingTime <= timeAlert && !alertShowing)
                        ShowAlert();
                }

                callback?.Invoke();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void ShowAlert()
    {
        alertShowing = true;
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        bool red = true;

        while (remainingTime > 0 && remainingTime <= timeAlert)
        {
            float step;
            if (red)
            {
                alertObj.color = new Color(1, 0, 0, 0.1f);
                step = 0.2f;
            }
            else
            {
                alertObj.color = new Color(1, 1, 1, 0);
                step = 0.8f;
            }

            red = !red;
            yield return new WaitForSecondsRealtime(step);
        }

        alertShowing = false;
        alertObj.color = new Color(1, 1, 1, 0);
    }


    void SetCountdownBar(float max)
    {
        if (countdownBar == null)
            return;

        countdownBar.fillAmount = 1 - currentTime / max;
    }

    public void Reset()
    {
        currentTime = 0f;
    }

    public static string ToString(float time)
    {
        return TimeSpan.FromSeconds(time).ToString("mm':'ss");
    }

    private void OnValidate()
    {
        countdownBar.color = cdbarColor;
        txtTime.color = textColor;
    }
}
