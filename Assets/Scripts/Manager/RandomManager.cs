using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomManager : MonoBehaviour
{
    [Header("Select the object:")]
    public GameObject @object;
    [Header("Choose a random type:")]
    public RandomType randomType = RandomType.ImageColor;

    [Header("Setting here:")]
    public Sprite[] sprites;
    public Gradient colors;
    public bool loop = false;
    public float timeLoop = 1f;

    void Start()
    {
        ChangeColor();
        if (loop)
            _ = StartCoroutine(StartLoop());
    }

    void ChangeColor()
    {
        Color newColor = GetRandomColor(colors);

        switch (randomType)
        {
            case RandomType.Image:
                {
                    SetRandomImage(@object, sprites);
                    break;
                }
            case RandomType.ImageColor:
                {
                    @object.GetComponent<Image>().color = newColor;
                    break;
                }
            case RandomType.TextColor:
                {
                    Text text = @object.TryGetComponent(out text) ? text : @object.GetComponentInChildren<Text>();
                    text.color = newColor;

                    break;
                }
        }
    }

    IEnumerator StartLoop()
    {
        while (true)
        {
            ChangeColor();
            yield return new WaitForSeconds(timeLoop);
        }
    }

    public static Color GetRandomColor(Gradient colors)
    {
        return colors.Evaluate(Random.Range(0f, 1f));
    }

    public static void SetRandomImage(GameObject @object, Sprite[] list)
    {

        Image image = @object.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Object: " + @object.name + " don't have the <Image> component.");
            return;
        }

        image.sprite = list[Random.Range(0, list.Length)];
        //image.rectTransform.localScale = new Vector3(scale, scale, 1f);

        image.type = Image.Type.Sliced;
        image.fillCenter = true;
        image.pixelsPerUnitMultiplier = 100;
    }

    public enum RandomType
    {
        ImageColor, TextColor, Image
    };
}
