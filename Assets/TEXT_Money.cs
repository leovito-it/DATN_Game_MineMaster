using TMPro;
using UnityEngine;

public class TEXT_Money : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;

    private void OnGUI()
    {
        DEFINE.SetText(textMeshPro, $"{Money.Current}");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        OnGUI();
    }
#endif
}
