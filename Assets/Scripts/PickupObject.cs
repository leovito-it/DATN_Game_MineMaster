using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupObject : MonoBehaviour
{
    [SerializeField] List<Sprite> listObject = new();
    public int MyWeight = 100;

    const int MinWeight = 10;
    const int MaxWeight = 30;

    // Start is called before the first frame update
    void Start()
    {
        RandomMe();
    }

    void RandomMe()
    {
        if (gameObject.TryGetComponent(out Image img))
        {
            //img.color = new Color(Random.value, Random.value, Random.value, 1f);
            img.color = Color.white;
            img.sprite = listObject[Random.Range(0, listObject.Count)];
            img.SetNativeSize();
        }

        MyWeight = Random.Range(MinWeight, MaxWeight) * 10;
        transform.localScale = Mathf.Lerp(0.5f, 1.2f, (MyWeight - MinWeight * 10f) / ((MaxWeight - MinWeight) * 10f)) * Vector3.one;

        List<Cell> cells = SiteManager.Instance.cells;

        gameObject.transform.SetParent(cells[Random.Range(0, cells.Count)].transform);
        gameObject.transform.localPosition = Vector3.zero;
    }
}
