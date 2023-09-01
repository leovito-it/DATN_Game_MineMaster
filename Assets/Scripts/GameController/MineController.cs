using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineController : MonoBehaviour
{
    [SerializeField] Rule rule;

    [Header("Check and finish")]
    [SerializeField] GameObject tryAgainMessage;
    [SerializeField] Button quickCheck;

    [Header("Prefabs")]
    [SerializeField] GameObject mineObject;
    [SerializeField] GameObject flagObject;

    [SerializeField] Transform flagCount, flagDisable;

    public enum ClickType { Select, Flagged }

    [Header("Config")]
    [SerializeField] Sprite clickedBg;
    [SerializeField] Color[] numberColors = new Color[] { Color.black };

    ClickType clickType = ClickType.Select;
    float numMine = 10;
    float endTime = 120;

    // DATA
    readonly List<int> mineIndex = new();
    readonly List<int> clickedIndex = new();
    readonly List<int> flaggedIndex = new();

    bool firstCheck = true;
    [SerializeField] bool isDebug = false;

    static string TIME_OVER => LanguageText.IsVietnamese ? "Thời gian" : "Over time";
    static string NUM_END => LanguageText.IsVietnamese ? "Số lượng mìn" : "Num of mine";

    private void Start()
    {
        UIManager.Instance.SetTextLevel(LevelManager.Instance.Level);

        LevelManager.Instance.UpdateVars(ref endTime, ref numMine, rule);
        LevelManager.Instance.SetInfo(TIME_OVER, NUM_END);
        LevelManager.Instance.SetProcess(0, (int)numMine);

        UIManager.Instance.SetTextTarget((int)numMine);

        InitMines();
        InitFlagCount();
        SetOnClick();

        Timer.Instance.RunCountDown(endTime, () => { if (Timer.remainingTime <= 0) GameOver(); });

        if (isDebug)
            DEBUG();
    }

    void InitMines()
    {
        string log = "";
        for (int i = 0; i < numMine; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, SiteManager.Instance.Count);
            while (mineIndex.Contains(randomIndex))
            {
                randomIndex = UnityEngine.Random.Range(0, SiteManager.Instance.Count);
            }
            mineIndex.Add(randomIndex);
            log += randomIndex + "_";
        }
        Debug.LogWarning(log);
    }

    void InitFlagCount()
    {
        for (int i = 0; i < flagCount.childCount; i++)
        {
            Destroy(flagCount.GetChild(i).gameObject);
        }

        for (int i = 0; i < numMine; i++)
        {
            Instantiate(flagObject, flagCount);
        }
    }

    private void SetOnClick()
    {
        for (int i = 0; i < SiteManager.Instance.Count; i++)
        {
            int index = i;
            Transform cell = SiteManager.Instance.GetAtIndex(index);
            cell.GetComponent<Button>().onClick.AddListener(() => OnClickHandle(index));
        }
    }

    void OnClickHandle(int index)
    {
        // khong cho nhan lai lan 2
        if (clickedIndex.Contains(index) || DEFINE.Status != DEFINE.GameStatus.Playing)
            return;

        if (clickType == ClickType.Select)
        {
            if (flaggedIndex.Contains(index))
                return;

            if (mineIndex.Contains(index))
            {
                BOOM(index);
            }
            else
            {
                AlertMineAround(index);
            }
        }
        else
        {
            Flagged(index);
        }
    }

    public void ChangeClickType()
    {
        if (clickType == ClickType.Select)
        {
            clickType = ClickType.Flagged;
            flagDisable.gameObject.SetActive(false);
        }
        else
        {
            clickType = ClickType.Select;
            flagDisable.gameObject.SetActive(true);
        }
    }

    private void Flagged(int index)
    {
        Transform pos = SiteManager.Instance.GetAtIndex(index);

        if (!flaggedIndex.Contains(index))
        {
            if (flagCount.childCount == 0)
                return;

            Instantiate(flagObject, pos).name = flagObject.name;
            flaggedIndex.Add(index);

            //show in UI
            Destroy(flagCount.GetChild(flagCount.childCount - 1).gameObject);

            if (flaggedIndex.Count == (int)numMine)
            {
                quickCheck.gameObject.SetActive(true);
            }
        }
        else
        {
            Destroy(pos.Find(flagObject.name).gameObject);
            flaggedIndex.Remove(index);

            Instantiate(flagObject, flagCount).transform.SetAsLastSibling();
        }

        LevelManager.Instance.SetProcess(flaggedIndex.Count, (int)numMine);
    }

    private void AlertMineAround(int index)
    {
        int mineCount = GetMineAround(index);
        Cell cell = SiteManager.Instance.cells[index];

        if (!isDebug)
            AddClicked(index);

        if (cell.Checked)
            return;

        cell.Checked = true;

        if (flaggedIndex.Contains(index))
        {
            Flagged(index);
        }

        if (mineCount != 0)
        {
            Text txt = cell.GetComponentInChildren<Text>();
            txt.text = mineCount.ToString();
            txt.color = numberColors[Mathf.Min(mineCount - 1, numberColors.Length - 1)];
            txt.enabled = true;
            txt.gameObject.SetActive(true);

            if (clickedIndex.Count == SiteManager.Instance.Count - numMine)
                Check(null);

            return;
        }

        foreach (int neighbor in SiteManager.Instance.GetAround(index))
        {
            AlertMineAround(neighbor);
        }
    }

    private void BOOM(int index)
    {
        Transform pos = SiteManager.Instance.GetAtIndex(index);
        Instantiate(mineObject, pos).transform.localPosition = Vector3.zero;

        Invoke(nameof(GameOver), 1f);
    }

    void AddClicked(int index)
    {
        if (clickedIndex.Contains(index))
            return;

        clickedIndex.Add(index);

        Transform cell = SiteManager.Instance.GetAtIndex(index);
        cell.GetComponent<Image>().sprite = clickedBg;
    }

    int GetMineAround(int index)
    {
        int mineCount = 0;
        foreach (int neighbor in SiteManager.Instance.GetAround(index))
        {
            if (mineIndex.Contains(neighbor))
            {
                mineCount++;
            }
        }
        return mineCount;
    }

    int GetScore()
    {
        int count = 0;
        foreach (int flag in flaggedIndex)
        {
            if (mineIndex.Contains(flag))
            {
                count++;
            }
        }
        return count;
    }

    void Win()
    {
        DEFINE.Status = DEFINE.GameStatus.Win;
        SetHighestScore();

        LevelManager.Instance.UnlockNextLevel();
    }

    void GameOver()
    {
        DEFINE.Status = DEFINE.GameStatus.Lose;
        SetHighestScore();

        LevelManager.Instance.Failed();
    }

    public void Check(Button btn)
    {
        if (IsWellDone())
        {
            Win();
        }
        else
        {
            if (firstCheck)
            {
                firstCheck = false;

                if (btn != null)
                {
                    btn.GetComponentInChildren<Text>().text = "Check";
                    btn.gameObject.SetActive(false);
                }
            }
            else
            {
                GameOver();
            }
        }
    }

    bool IsWellDone()
    {
        Debug.LogWarning("Adhshdah");
        if (clickedIndex.Count == 100 - numMine)
        {
            return true;
        }
        else
        {
            if (flaggedIndex.Count != numMine)
                return false;

            foreach (int id in flaggedIndex)
            {
                if (!mineIndex.Contains(id))
                    return false;
            }
            return true;
        }
    }

    void SetHighestScore()
    {
        float current = Timer.currentTime;

        if (current < GetBest())
        {
            PlayerPrefs.SetFloat("MineBest", current);
        }
    }

    float GetBest()
    {
        return PlayerPrefs.GetFloat("MineBest", int.MaxValue);
    }

    void DEBUG()
    {
        for (int i = 0; i < 100; i++)
        {
            if (mineIndex.Contains(i))
            {
                Transform pos = SiteManager.Instance.GetAtIndex(i);
                Instantiate(mineObject, pos).transform.localPosition = Vector3.zero;
            }

            AlertMineAround(i);
        }
    }
}
