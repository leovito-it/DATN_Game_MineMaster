using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [Header("Set TEXT")]
    public Text txtScore;
    public Text txtTarget;
    public Text txtLevel;

    public void SetTextTarget(int numTarget)
    {
        DEFINE.SetText(txtTarget, numTarget.ToString());
    }

    public void SetTextScore(int score, int scoreScale)
    {
        DEFINE.SetText(txtScore, score * scoreScale + "");
    }

    public void SetTextLevel(int level)
    {
        DEFINE.SetText( txtLevel, "LV. " +level.ToString());
    }
}
