using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyCountdown : MonoBehaviour
{
    public float timeCd;
    public TextMeshProUGUI textCd;
    public Image imgBg;

    float currentCD = 0;
    const float timeStep = 1f;

    bool isCDing;

    public void StartCD(Action callback)
    {
        if (isCDing)
            return;

        callback?.Invoke();
        _ = StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        isCDing = true;
        currentCD = timeCd;
        imgBg.color = Color.gray;

        while (currentCD > 0)
        {
            textCd.text = ((int)currentCD).ToString();
            currentCD -= timeStep;
            yield return new WaitForSeconds(timeStep);
        }
        if (currentCD <= 0)
        {
            Reset();
        }
    }

    public void Reset()
    {
        currentCD = 0;
        textCd.text = DEFINE.BLANK;
        imgBg.color = Color.white;
        isCDing = false;
    }

    public void ModifyCurrentCD(int value)
    {
        if (currentCD <= 0)
            return;

        float tempCD = currentCD + value;

        if (tempCD <= 0)
            Reset();
        else
            currentCD = tempCD;
    }

    private void OnEnable()
    {
        
    }
}
