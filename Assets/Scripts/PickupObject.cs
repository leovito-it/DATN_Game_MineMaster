using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupObject : MonoBehaviour
{
    public Sprite MyIcon;
    public int MyWeight = 100;
    public int MyValue = 0;

    const float SCALE_MIN = 0.7f;
    const float SCALE_MAX = 1.6f;

    // Start is called before the first frame update
    void Start()
    {
        RandomMe();
    }

    void RandomMe()
    {
        float randomScale = Random.Range(SCALE_MIN, SCALE_MAX);
        MyWeight = Mathf.RoundToInt(MyWeight * randomScale);
        transform.localScale = randomScale * Vector3.one;

        List<Cell> cells = SiteManager.Instance.cells;

        gameObject.transform.SetParent(cells[Random.Range(0, cells.Count)].transform);
        gameObject.transform.localPosition = Vector3.zero;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (MyIcon != null)
        {
            Image img = GetComponent<Image>();
            img.sprite = MyIcon;
            img.SetNativeSize();
        }
    }
#endif
}
