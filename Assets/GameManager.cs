using SFX;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Swapn")]
    [SerializeField] int numObject = 5;
    [SerializeField] List<PickupObject> prefabs;
    [SerializeField] Canvas canvas;

    [Header("Other")]
    [SerializeField] Money money;
    [SerializeField] Timer timer;

    [SerializeField] Rule rule;

    float target = 10;
    float endTime = 120;

    static string TIME_OVER => LanguageText.IsVietnamese ? "Thời gian" : "Over time";
    static string NUM_END => LanguageText.IsVietnamese ? "Số lượng mìn" : "Num of mine";

    void Start()
    {
        LevelManager.Instance.UpdateVars(ref endTime, ref target, rule);
        LevelManager.Instance.SetInfo();
        LevelManager.Instance.SetProcess(0, (int)target);

        Play();
    }

    public void Play()
    {
        DEFINE.Status = DEFINE.GameStatus.Playing;

        timer.RunCountDown(60, () => { if (Timer.remainingTime <= 0) GameOver(); });
        CreateObjects();
    }

    void CreateObjects()
    {
        for (int i = 0; i < numObject; i++)
        {
            int random = Random.Range(0, prefabs.Count);
            Instantiate(prefabs[random], canvas.transform);
        }
    }

    public void AddCoin(int value)
    {
        money.Add(value, true);
        SoundName.SeAddMoney.Get().Play();
    }

    void Win()
    {
        DEFINE.Status = DEFINE.GameStatus.Win;
        LevelManager.Instance.UnlockNextLevel();
    }

    void GameOver()
    {
        DEFINE.Status = DEFINE.GameStatus.Lose;
        LevelManager.Instance.Failed();
    }
}
