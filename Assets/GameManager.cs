using SFX;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Swapn")]
    [SerializeField] int numObject = 5;
    [SerializeField] GameObject prefab;
    [SerializeField] Canvas canvas;

    [SerializeField] Money money;
    [SerializeField] Timer timer;

    void Start()
    {
        Play();
        SoundName.BgGameplay1.Get().PlayAsBackground();
    }

    public void Play()
    {
        DEFINE.Status = DEFINE.GameStatus.Playing;

        timer.RunCountDown(60);
        CreateObjects();
    }

    void CreateObjects()
    {
        for (int i = 0; i < numObject; i++)
        {
            Instantiate(prefab, canvas.transform);
        }
    }

    public void AddCoin(int value)
    {
        money.Add(value, true);
        SoundName.SeAddMoney.Get().Play();
    }
}
