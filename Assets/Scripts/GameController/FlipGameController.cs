using SFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipGameController : MonoBehaviour
{
    public Rule rule;
    public ObjectCreateManager creator;

    public Transform container;

    public AudioClip flip, getPoint, fail;

    float overTime;
    float numObjects;
    int numScore = 0;

    bool win = false, lose = false;
    GameObject num1, num2;

    bool isReady = true;

    static string TIME_OVER => LanguageManager.IsVietnamese ? "Thời gian" : "Over time";
    static string NUM_END => LanguageManager.IsVietnamese ? "Số lượng thẻ" : "Num of card";

    private void Start()
    {
        UIManager.Instance.SetTextLevel(LevelManager.Instance.Level);

        LevelManager.Instance.UpdateVars(ref overTime, ref numObjects, rule);
        LevelManager.Instance.SetInfo(TIME_OVER, NUM_END);
        LevelManager.Instance.SetProcess(0, (int)numObjects);

        StartCoroutine(Create());

        if (overTime > 0)
            Timer.Instance.RunCountDown(overTime);
        else
            Timer.Instance.Run();
    }

    // Update is called once per frame
    void Update()
    {
        if (lose || win)
            return;

        if (DEFINE.isPlaying)
            CheckEndGame();
    }

    IEnumerator Create()
    {
        creator.maxObject = (int)numObjects / 2;
        creator.SpawnObjects();
        yield return new WaitForFixedUpdate();
        NumberManager.SetValues(100);
        yield return new WaitForFixedUpdate();
        Double();
        yield return new WaitForFixedUpdate();
        Mix();
        yield return new WaitForFixedUpdate();
        SetOnClick();
        yield return new WaitForFixedUpdate();
        DEFINE.isPlaying = true;
    }

    void Double()
    {
        foreach (NumberManager num in ObjectCreateManager.numbers)
        {
            Instantiate(num, container);
        }

        container.GetComponent<GridLayoutGroup>().constraintCount = Mathf.FloorToInt(Mathf.Sqrt(numObjects));
        if ((int)numObjects == 20)
            container.GetComponent<GridLayoutGroup>().cellSize *= 0.9f;
    }

    void Mix()
    {
        Debug.Log(container.childCount);
        for (int i = 0; i < container.childCount; i++)
        {
            container.GetChild(i).SetSiblingIndex(Random.Range(0, container.childCount));
        }
    }

    void SetOnClick()
    {
        for (int i = 0; i < container.childCount; i++)
        {
            int value = i;
            container.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
            {
                OnSelect(container.GetChild(value).gameObject);
            });
        }
    }

    void Flip(GameObject obj)
    {
        obj.GetComponent<Animator>().Play("flip");
    }

    void OnSelect(GameObject obj)
    {
        if (!isReady)
            return;

        isReady = false;

        Flip(obj);
        SFX_Manager.Instance.PlaySEOneShot(flip);

        if (num1 == null)
        {
            num1 = obj;
            ResetReady();
            return;
        }

        if (num2 == null)
        {
            if (obj != num1)
            {
                num2 = obj;
            }
            else
            {
                ResetReady();
                return;
            }
        }

        if (num1 != null && num2 != null)
            Invoke(nameof(Check), 1f);
    }


    void Check()
    {
        int value1 = num1.GetComponent<NumberManager>().value;
        int value2 = num2.GetComponent<NumberManager>().value;

        if (value1 == value2)
        {
            num1.GetComponent<Animator>().Play("ok");
            num2.GetComponent<Animator>().Play("ok");

            num1.GetComponent<Button>().enabled = false;
            num2.GetComponent<Button>().enabled = false;

            AddScore(2);
            SFX_Manager.Instance.PlaySEOneShot(getPoint);
        }
        else
        {
            num1.GetComponent<Animator>().Play("fail");
            num2.GetComponent<Animator>().Play("fail");
            SFX_Manager.Instance.PlaySEOneShot(fail);
        }

        Invoke(nameof(Reset), value1 == value2 ? 0 : 0.5f);
    }

    void AddScore(int value)
    {
        numScore += value;
        UIManager.Instance.SetTextScore(numScore, 1);
        LevelManager.Instance.SetProcess(numScore, (int)numObjects);
    }

    private void CheckEndGame()
    {
        // play time > request time
        CheckOutOfTime();

        // all target is cleared
        if (numScore == creator.maxObject * 2)
            win = true;

        if (lose)
            GameOverAction();

        if (win)
            LevelManager.Instance.UnlockNextLevel();
    }

    private void CheckOutOfTime()
    {
        if (overTime < 0f)
            return;

        if (Timer.currentTime >= overTime)
            lose = true;
    }

    private void GameOverAction()
    {
        DEFINE.isPlaying = false;
        SetHighestScore();

        LevelManager.Instance.Failed();
    }

    void SetHighestScore()
    {
        float current = Timer.currentTime;

        if (current < GetBest())
        {
            PlayerPrefs.SetFloat("FLIPGAME", current);
        }
    }

    float GetBest()
    {
        return PlayerPrefs.GetFloat("FLIPGAME", int.MaxValue);
    }

    private void Reset()
    {
        num1 = null;
        num2 = null;
        ResetReady();
    }

    void ResetReady()
    {
        isReady = true;
    }
}
