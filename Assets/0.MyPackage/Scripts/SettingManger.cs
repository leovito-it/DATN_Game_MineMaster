using SFX;
using UnityEngine;
using UnityEngine.VFX;

public class SettingManger : MonoBehaviour
{

    [Header("UI")]
    public GameObject disableBG;
    public GameObject disableSE;
    public GameObject disableVibr;

    bool _bg = true;
    bool _se = true;
    bool _vibr = true;


    private void OnEnable()
    {
        _bg = !SFX_Manager.Instance.BGmute;
        _se = !SFX_Manager.Instance.SEmute;
        _vibr = Vibration.Enable;

        Debug.Log($"bg:{_bg}; se:{_se}");
    }

    public void ChangeBG()
    {
        AudioManager.Instance.MuteBG(_bg);
        _bg = !_bg;
    }

    public void ChangeSE()
    {
        AudioManager.Instance.MuteSE(_se);
        _se = !_se;
    }

    public void ChangeVibration()
    {
        Vibration.Enable = !_vibr;
        _vibr = !_vibr;

        Vibration.Vibrate(500);
    }

    private void OnGUI()
    {
        disableBG.SetActive(!_bg);
        disableSE.SetActive(!_se);
        disableVibr.SetActive(!_vibr);
    }
}
