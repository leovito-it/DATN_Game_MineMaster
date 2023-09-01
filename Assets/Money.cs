using DG.Tweening;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    const string MONEY = "Money";
    const int MAX = 99999;
    const int MIN = 0;

    public static int Current
    {
        get { return PlayerPrefs.GetInt(MONEY, MIN); }
        set { DEFINE.SaveKey(MONEY, Mathf.Clamp(value, MIN, MAX)); }
    }

    int current;

    [Header("Effect")]
    [SerializeField] TextMeshProUGUI txtMoney;
    [SerializeField] ParticleSystem particle;
    [SerializeField] float timeEF;

    private void Start()
    {
        txtMoney.Clear();
    }

    public void Add(int value, bool useEffect = false)
    {
        current = Current;

        if (useEffect)
        {
            particle.Play();
            DEFINE.SetText(txtMoney, $"+ {value}$");

            DOVirtual.Int(current, current + value, timeEF, (x) =>
            {
                Current = x;
            }).OnComplete(() =>
            {
                txtMoney.Clear();
            });
        }
        else
        {
            Current = current + value;
        }
    }
}
