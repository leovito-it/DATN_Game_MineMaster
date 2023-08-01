using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ObjectCreateManager : MonoBehaviour
{
    [Header("Object creation")]

    // The object to spawn
    // WARNING: take if from the Project panel, NOT the Scene/Hierarchy!
    public NumberManager numberPrefab;
    public GameObject container;

    public float gravityRate = 0f;
    public bool resizeToFit = true;

    [Header("Other options")]

    // Configure the spawning pattern
    [Range(0f,5f)]
    public float spawnInterval;
    [Range(0,100)]
    public int maxObject = 100;


    public int[] fontRange = new int[2] { 25, 35 };
    public Gradient colors;

    public bool randomBackground = false;
    public Sprite[] listBackground;

    private int count = 0; // count number of objects

    private RectTransform rt;

    public static List<NumberManager> numbers = new List<NumberManager>();

    const float maxScale = 2.5f;

    // This will spawn an object, and then wait some time, then spawn another...
    public void SpawnObjects()
    {
        numbers.Clear();

        if (resizeToFit)
        { 
            float scale = GetScale();
            numberPrefab.numberConfig.scale = scale > maxScale ? maxScale : scale;
        }

        StartCoroutine(Create());
    }

    IEnumerator Create()
    {
        if (spawnInterval > 0)
            while (count < maxObject)
            {
                CreateOne();
                // Wait for some time before spawning another object
                yield return new WaitForSeconds(spawnInterval);
            }
        else
            for (int i = 0; i < maxObject; i++)
            {
                CreateOne();
            }
    }

    private void CreateOne()
    {
        rt = container.GetComponent<RectTransform>();

        // set random position for object
        MinigameToolKit.SpawnOptions options = new MinigameToolKit.SpawnOptions()
        {
            container = rt,
            amount = maxObject,
            randomPosition = true,
            //rotateRange = new Vector2(-30f, 30f),
            objects = new GameObject[] { numberPrefab.gameObject }
        };

        GameObject num = options.CreateObject();
        SetValues(num);   
    }

    void SetValues(GameObject number)
    {
        if (number.TryGetComponent(out Rigidbody2D rigidbody))
            rigidbody.gravityScale = gravityRate;

        // set random image if possible
        randomBackground = listBackground.Length != 0 && randomBackground;
        if (randomBackground)
            RandomManager.SetRandomImage(number, listBackground);

        // find the text object
        Text txt = number.GetComponentInChildren<Text>();

        // set value of number
        if (txt != null)
        {
            txt.color = RandomManager.GetRandomColor(colors);
            if (fontRange[0] < fontRange[1])
                txt.fontSize = Random.Range(fontRange[0], fontRange[1]);
        }

        // add to list
        numbers.Add(number.GetComponent<NumberManager>());
    }

    float GetScale()
    {
        Vector2 prefabSize = numberPrefab.GetComponent<RectTransform>().sizeDelta;
        Vector2 spawnSize = container.GetComponent<RectTransform>().rect.size;

        Debug.LogWarning(spawnSize);

        float colliderArea = Mathf.Pow(Mathf.Max( prefabSize.x, prefabSize.y) / 2, 2) * Mathf.PI;
        float spawnArea = spawnSize.x * spawnSize.y;

        float scale = (spawnArea / colliderArea) / maxObject;

        return scale > 1 ? Mathf.Sqrt(scale): scale; 
    }
}
