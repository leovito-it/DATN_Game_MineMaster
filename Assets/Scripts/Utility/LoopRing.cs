using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopRing : MonoBehaviour
{
    public RectTransform[] rings;
    public float MaxScale = 8f;
    public float Speed = 0.2f;

    public AnimationCurve curve;

    Vector3 startScale;

    private void Start()
    {
        startScale = rings[0].localScale;
        _ = StartCoroutine(Loop());
    }

    // Update is called once per frame
    IEnumerator Loop()
    {
        while (true)
        {
            for (int i = 0; i < rings.Length; i++)
            {
                float currentScale = rings[i].localScale.x;
                float nextScale = currentScale + curve.Evaluate(currentScale / MaxScale) * Speed;

                rings[i].localScale = nextScale * Vector3.one;

                if (nextScale >= MaxScale)
                {
                    rings[i].localScale = startScale;
                    Debug.LogWarning("RUN");
                }
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}
