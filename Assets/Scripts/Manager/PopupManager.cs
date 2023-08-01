using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    public delegate void PopupCallBack(bool isSuccess);
    public delegate void Action();

    GameObject SelectedPopup;

    [SerializeField] private GameObject BackgroundFade;

    [Header("Settings")]
    [SerializeField] private GameObject Settings;

    [Header("Loading")]
    [SerializeField] private GameObject Loading;

    public TextMeshProUGUI debugLog;

    protected void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(gameObject);
    }

    /*
     * -------- PUBLIC METHODS --------------
     */

    public void CloseAll()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OpenSettings()
    {
        SelectedPopup = Settings;
        OpenPopupType1();
    }

    public void CloseSettings()
    {
        HiddenPopup(Settings);
    }

    public void ShowLoading()
    {
        SelectedPopup = Loading;
        OpenPopupType1();
    }

    public void CloseLoading()
    {
        HiddenPopup(Loading);
    }

    /*
     * -------- PRIVATE METHODS --------------
     */

    private void OpenPopupType1()
    {
        OpenPopup(SelectedPopup);
        FadeInBackground();
    }

    private void FadeInBackground()
    {
        BackgroundFade.SetActive(true);
        StartCoroutine(DoLerpAlphaScale(BackgroundFade.GetComponent<CanvasGroup>(), 0, 1, 1, 1, 0.2f, () => {

        }));
    }
    private void FadeOutBackground()
    {
        StartCoroutine(DoLerpAlphaScale(BackgroundFade.GetComponent<CanvasGroup>(), 1, 0, 1, 1, 0.2f, () => {
            BackgroundFade.SetActive(false);
        }));
    }

    private void OpenPopup(GameObject obj)
    {
        FadeInBackground();
        StartCoroutine(DoLerpAlphaScale(obj.GetComponent<CanvasGroup>(), 0, 1, 1, 1f, 0.2f, () => {
            obj.SetActive(true);
            SelectedPopup = obj;
            ///save image for gallery//
        }));
    }

    private void OpenPopupNotBackground(GameObject obj)
    {
        StartCoroutine(DoLerpAlphaScale(obj.GetComponent<CanvasGroup>(), 0, 1, 1, 1f, 0.2f, () => {
            obj.SetActive(true);
            SelectedPopup = obj;
        }));
    }

    private void HiddenPopup(GameObject obj)
    {
        if (obj.activeInHierarchy)
        {
            StartCoroutine(DoLerpAlphaScale(obj.GetComponent<CanvasGroup>(), 1, 0, 1, 1f, 0.2f, () => {
                obj.SetActive(false);
            }));
            FadeOutBackground();
        }
    }

    private IEnumerator DoLerpAlphaScale(CanvasGroup c, float fromAlpha, float toAlpha, float fromScale, float toScale, float time, Action callback)
    {
        float timer = 0;

        c.gameObject.SetActive(true);
        c.alpha = fromAlpha;
        c.gameObject.GetComponent<RectTransform>().localScale = new Vector2(fromScale, fromScale);

        while (timer <= time)
        {
            timer += Time.deltaTime;
            c.alpha = Mathf.Lerp(fromAlpha, toAlpha, timer / time);
            c.gameObject.GetComponent<RectTransform>().localScale = new Vector2(Mathf.Lerp(fromScale, toScale, timer / time), Mathf.Lerp(fromScale, toScale, timer / time));
            yield return null;
        }

        c.alpha = toAlpha;
        c.gameObject.GetComponent<RectTransform>().localScale = new Vector2(toScale, toScale);

        callback?.Invoke();
    }
}
