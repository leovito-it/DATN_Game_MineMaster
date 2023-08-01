using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGetCamera : MonoBehaviour
{
    private void Awake()
    {
        SetCamera();
    }

    void SetCamera()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 999;
    }

    private void OnValidate()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas.renderMode != RenderMode.ScreenSpaceCamera || canvas.worldCamera == null)
        {
            SetCamera();
        }
    }
}
