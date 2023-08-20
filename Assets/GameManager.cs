using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int numObject = 5;
    public GameObject prefab;

    public Canvas canvas;

    void Start()
    {
        CreateObjects();
    }

    void CreateObjects()
    {
        for (int i = 0; i < numObject; i++)
        {
            Instantiate(prefab, canvas.transform);
        }
    }
}
