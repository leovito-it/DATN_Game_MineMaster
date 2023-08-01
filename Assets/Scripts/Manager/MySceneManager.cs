﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    public bool showAds = false;
    public AudioClip clipBg;
    public GameObject fadeObject;
    [Range(0f, 5f)]
    public float fadeTime = 2f;

    private void Awake()
    {
        if (fadeObject != null)
            fadeObject.SetActive(false);

        ResizeByPlatform();
    }

    void ResizeByPlatform()
    {
        Debug.Log(Application.platform + " - "  + SystemInfo.deviceModel);

        foreach (CanvasScaler scaler in FindObjectsOfType<CanvasScaler>())
        {
            scaler.matchWidthOrHeight = SystemInfo.deviceModel.StartsWith("iPad") ? 1:0;
        }
    }

    private void Start()
    {
        if (FindObjectOfType<PopupManager>() != null)
        {
            PopupManager.Instance.CloseAll();
        }

        FadeOut();
        PlayBG();
        Invoke(nameof(ShowAds), fadeTime);
    }

    void PlayBG()
    {
        AudioManager.Instance.PlayBG(clipBg);
    }

    void ShowAds()
    {
        try
        {
            AdmobManager.Instance.ShowBanner(showAds);
        }
        catch
        {

        }
    }

    void FadeOut()
    {
        if (fadeObject != null)
            fadeObject.SetActive(true);
    }

    public static void LoadScene(string name)
    {
         GameObject.FindObjectOfType<MonoBehaviour>().StartCoroutine(AnimationLoadScene(name));
    }

    static IEnumerator AnimationLoadScene(string name)
    {
        if (FindObjectOfType<PopupManager>() != null)
        {
            PopupManager.Instance.CloseAll();
            PopupManager.Instance.ShowLoading();
        }

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
