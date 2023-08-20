using _0._MiraiStudio.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateUs : MonoBehaviour
{
    [SerializeField] List<Button> stars;
    [SerializeField] Button btnYes, btnCancel;

    [SerializeField] Sprite starOn, starOff;

    int currentStar = 5;

    public static bool IsRateUsShowed
    {
        get
        {
            return PlayerPrefs.GetInt("RaseUs", 0) == 1;
        }

        set
        {
            PlayerPrefs.SetInt("RaseUs", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    private void OnEnable()
    {
        ShowStar();

        IsRateUsShowed = true;

        for (int i = 0; i < 5; i++)
        {
            int value = i + 1;
            stars[i].onClick.AddListener(() =>
            {
                currentStar = value;
                ShowStar();
            });
        }

        btnYes.onClick.RemoveAllListeners();
        btnYes.onClick.AddListener(() =>
        {
            CloseMe();
            if (currentStar <= 3)
            {

            }
            else
            {
                MiraiStudio_Lib.Instance.Rate();
            }
        });

        btnCancel.onClick.RemoveAllListeners();
        btnCancel.onClick.AddListener(() =>
        {
            CloseMe();
        });
    }

    void ShowStar()
    {
        for (int i = 0; i < 5; i++)
        {
            stars[i].image.sprite = i < currentStar ? starOn : starOff;
        }
    }

    void CloseMe()
    {
        PopupManager.Instance.CloseAll();
    }
}
