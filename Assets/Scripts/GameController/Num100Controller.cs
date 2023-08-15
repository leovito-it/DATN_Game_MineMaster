using SFX;
using UnityEngine;
using UnityEngine.UI;

public class Num100Controller : MonoBehaviour
{
    public Rule rule;

    [SerializeField] GameConfig config;

    [Header("Set game's rule values")]
    public NextTarget nextTarget = NextTarget.Higher;

    public enum NextTarget { Higher, Lower, Random }

    // values
    public int randomRange = 100;

    private int numScore = 0;
    private float numToUnlock = 0f;
    public float overTime = 0f;

    // check the game's ending;
    private bool lose = false;
    private bool win = false;

    [HideInInspector] public string mode;

    static int targetId = 0;
    int numObjects;
    public int NumTarget;

    static string TIME_OVER => LanguageManager.IsVietnamese ? "Thời gian" : "Over time";
    static string NUM_END => LanguageManager.IsVietnamese ? "Số lượng cần tìm" : "End score";

    // Start is called before the first frame update
    void Awake()
    {
        UIManager.Instance.SetTextLevel(LevelManager.Instance.Level);

        LevelManager.Instance.UpdateVars(ref overTime, ref numToUnlock, rule);
        LevelManager.Instance.SetInfo(TIME_OVER, NUM_END);
        LevelManager.Instance.SetProcess(0, (int)numToUnlock);

        FindObjectOfType<ObjectCreateManager>().maxObject = 50;
        GameStart();
    }

    void GameStart()
    {
        NumberManager.values = new System.Collections.Generic.List<int>();

        ObjectCreateManager creator = FindObjectOfType<ObjectCreateManager>();
        creator.SpawnObjects();
        numObjects = creator.maxObject;

        NumberManager.SetValues(randomRange < numObjects ? numObjects : randomRange);
        SetOnClick();

        nextTarget = NextTarget.Random;

        ChangeTarget();

        if (overTime > 0)
            Timer.Instance.RunCountDown(overTime, () => CheckEndGame());
        else
            Timer.Instance.Run();
    }

    void SetOnClick()
    {
        foreach (NumberManager number in ObjectCreateManager.numbers)
        {
            number.GetComponent<Button>().onClick.AddListener(() => { FindObjectOfType<Num100Controller>().ClickHandle(number.gameObject); });
        }
    }

    public bool IsTrueTarget(int value)
    {
        return value == NumTarget;
    }

    private void CheckEndGame()
    {
        // play time > request time
        CheckOutOfTime();

        // all target is cleared
        if (numScore == numToUnlock)
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

    public void ClickHandle(GameObject clicked)
    {
        NumberManager numberManager = clicked.GetComponent<NumberManager>();

        if (IsTrueTarget(numberManager.value))
        {
            // play audio
            SFX_Manager.Instance.PlaySE(config.clipTrueClick, false);

            DestroyObject(clicked);

            NumberManager.values.RemoveAt(targetId);

            // update the target
            AddScore();
            ChangeTarget();
        }
        else
        {
            SFX_Manager.Instance.PlaySE(config.clipFalseClick, false);
        }
    }

    private void GameOverAction()
    {
        DEFINE.isPlaying = false;
        SetHighestScore();

        LevelManager.Instance.Failed();
    }

    public void SetEndGGame(bool status)
    {
        lose = status;
    }

    public string GetTargetName()
    {
        return DEFINE.NUMBER + NumTarget;
    }

    public void AddScore()
    {
        numScore++;
        UIManager.Instance.SetTextScore(numScore, 1);
        LevelManager.Instance.SetProcess(numScore, (int)numToUnlock);
    }

    public void ChangeTarget()
    {
        if (NumberManager.values.Count == 0)
            return;

        targetId = 0;

        switch (nextTarget)
        {
            case NextTarget.Higher:
                {
                    NumberManager.values.Sort((a, b) => a.CompareTo(b));

                    break;
                }
            case NextTarget.Lower:
                {
                    NumberManager.values.Sort((a, b) => -a.CompareTo(b));

                    break;
                }
            case NextTarget.Random:
                {
                    NumberManager.values.Sort((a, b) => a.CompareTo(b));

                    targetId = Random.Range(0, NumberManager.values.Count);
                    break;
                }
        }

        Debug.Log(NumberManager.values[targetId]);

        NumTarget = NumberManager.values[targetId];
        //GameObject.Find(GetTargetName()).transform.SetAsLastSibling();
        UIManager.Instance.SetTextTarget(NumTarget);
    }

    public bool IsALoseGame()
    {
        return this.lose;
    }

    public void DestroyObject(GameObject obj)
    {
        if (obj == null)
            return;
        // Destroy the object
        if (obj.TryGetComponent(out NumberManager numberManager))
        {
            Instantiate(numberManager.numberConfig.deadEffect).transform.position = obj.transform.position;
        }
        Destroy(obj);
    }

    void SetHighestScore()
    {
        float current = Timer.currentTime;

        if (current < GetBest())
        {
            PlayerPrefs.SetFloat(DEFINE.KEY_BEST + numObjects, current);
        }
    }

    public float GetBest()
    {
        return PlayerPrefs.GetFloat(DEFINE.KEY_BEST + numObjects, int.MaxValue);
    }
}