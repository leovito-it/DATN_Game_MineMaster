using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberManager : MonoBehaviour
{
    public NumberConfig numberConfig;
    public int value;

    public static List<int> values = new List<int>();

    void OnStart()
    {
        ScaleMe();

        Text txt = GetComponentInChildren<Text>();

        if (value != 0)
        {
            values.Add(value);
        }

        txt.text = value.ToString();
        name = DEFINE.NUMBER + value;
    }

    void ScaleMe()
    {
        if (numberConfig != null)
            gameObject.GetComponent<RectTransform>().localScale = numberConfig.scale * Vector3.one;
    }

    static List<int> temp;
    static int randomValue = -1;

    public static void SetValues(int max)
    {
        temp = new List<int>();
        GetRandomValue(max);

        foreach (NumberManager number in ObjectCreateManager.numbers)
        {
            while (temp.Contains(randomValue))
            {
                GetRandomValue(max);
            }

            number.value = randomValue + 1;
            temp.Add(randomValue);

            number.OnStart();
        }
    }

    static void GetRandomValue(int max)
    {
        randomValue = Random.Range(0, max);
    }
}
