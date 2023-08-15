using DG.Tweening;
using SFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuffleController : MonoBehaviour
{
    [Header("Level's rule")]
    public Rule rule;

    [Header("Game value")]
    public GameObject[] cups;
    public GameObject obj;

    public GameObject right, failed;

    public Animator anim;
    public Toggle autoToggle;

    public float timeShuffle = 3f;
    public float timePerSwap = 0.5f;

    public AudioClip shuffle, getPoint, fail;

    static string TIME_SHUFFLE => LanguageManager.IsVietnamese ? "Thời gian đảo" : "Shuffle time";
    static string TIME_SWAP => LanguageManager.IsVietnamese ? "Thời gian mỗi lần đảo" : "Time per shuffle";

    static readonly int SCORE_TO_UNLOCK = 5;

    List<Vector3> startPosList = new List<Vector3>();

    int score = 0;
    bool autoShuffle = true;

    private void Start()
    {
        UIManager.Instance.SetTextLevel(LevelManager.Instance.Level);

        LevelManager.Instance.UpdateVars(ref timeShuffle, ref timePerSwap, rule);
        LevelManager.Instance.SetInfo(TIME_SHUFFLE, TIME_SWAP);
        LevelManager.Instance.SetProcess(0, SCORE_TO_UNLOCK);

        InitCups();
        UpdateButtonState(true);

        autoToggle.onValueChanged.AddListener((state) => SetAuto());

        Timer.Instance.RunCountDown(300, () => Failed());
    }

    void InitCups()
    {
        foreach (GameObject cup in cups)
        {
            startPosList.Add(cup.GetComponent<RectTransform>().localPosition);
        }
    }

    public void OpenCup(GameObject cup)
    {
        if (!canChoose)
            return;

        canChoose = false;
        //Animator anim = cup.GetComponent<Animator>();
        //if (anim != null)
        //    anim.Play("open");
        StartCoroutine(CheckObjectIn(cup));
    }

    private IEnumerator CheckObjectIn(GameObject cup)
    {
        bool success = cup.transform.childCount == 1;
        //yield return success ? null : new WaitForSeconds(1);

        obj.GetComponent<CanvasGroup>().alpha = 1;
        anim.enabled = true;
        anim.Play("end");

        UpdateButtonState(true);
        SFX_Manager.Instance.PlaySEOneShot(success ? getPoint : fail);

        GameObject notice = success ? right : failed;
        notice.SetActive(true);
        notice.transform.localPosition = cup.transform.localPosition;
        notice.transform.DOLocalMove(new Vector3(-455f, 785, 0), 1);

        yield return new WaitForSeconds(1f);
        AddScore(success ? 1 : -1);
        notice.SetActive(false);

        if (autoShuffle)
        {
            yield return new WaitForSeconds(0.02f);
            Play();
        }
    }

    private void AddScore(int value)
    {
        score += value;
        UIManager.Instance.SetTextScore(score, 1);
        LevelManager.Instance.SetProcess(score, SCORE_TO_UNLOCK);

        if (score == SCORE_TO_UNLOCK)
            LevelManager.Instance.UnlockNextLevel();
    }

    void Failed()
    {
        if (Timer.remainingTime == 0)
            LevelManager.Instance.Failed();
    }

    bool canChoose = false;

    public void Play()
    {
        if (canChoose)
            return;

        anim.enabled = true;
        anim.Play("gameStart");
        Invoke(nameof(Shuffle), 2.01f);

        UpdateButtonState(false);
    }

    void Shuffle()
    {
        anim.enabled = false;
        StartCoroutine(SwapRepeating(timeShuffle));
    }

    IEnumerator SwapRepeating(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            Vector2 swapIndex = Get2Pos();
            if (!isSwapping)
            {
                StartCoroutine(Swap((int)swapIndex.x, (int)swapIndex.y, timePerSwap));
                SFX_Manager.Instance.PlaySEInterval(shuffle, 0.5f, 1f);
                timer += timePerSwap;
                yield return new WaitForSecondsRealtime(timePerSwap);
            }
            else
                yield return new WaitForFixedUpdate();
        }

        // can choose if shuffle end
        canChoose = true;
    }

    Vector2 Get2Pos()
    {
        int pos1 = Random.Range(0, cups.Length);
        int pos2 = Random.Range(0, cups.Length);

        while (pos1 == pos2)
        {
            pos2 = Random.Range(0, cups.Length);
        }

        return new Vector2(pos1, pos2);
    }


    bool isSwapping = false;

    IEnumerator Swap(int pos1, int pos2, float duration)
    {
        float timer = 0;
        isSwapping = true;

        RectTransform obj1 = cups[pos1].GetComponent<RectTransform>();
        RectTransform obj2 = cups[pos2].GetComponent<RectTransform>();

        while (timer <= duration)
        {
            timer += Time.fixedDeltaTime;

            obj1.localPosition = Vector3.Lerp(startPosList[pos1], startPosList[pos2], timer / duration);
            obj2.localPosition = Vector3.Lerp(startPosList[pos2], startPosList[pos1], timer / duration);

            yield return new WaitForFixedUpdate();
        }

        obj1.localPosition = startPosList[pos2];
        obj2.localPosition = startPosList[pos1];

        // swap in list
        GameObject temp = cups[pos1];
        cups[pos1] = cups[pos2];
        cups[pos2] = temp;

        isSwapping = false;
    }

    void UpdateButtonState(bool enable)
    {
        GameObject btn = GameObject.Find("btnShuffle");
        btn.GetComponent<Button>().enabled = enable;
        btn.GetComponent<Animator>().Play(enable ? "Normal" : "Disabled");
    }

    public void SetAuto()
    {
        autoShuffle = autoToggle.isOn;
    }
}
