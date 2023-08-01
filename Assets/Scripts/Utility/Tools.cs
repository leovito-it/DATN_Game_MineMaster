using System;
using System.Collections;
using UnityEngine;

public class Tools : MonoBehaviour
{
    Num100Controller manager => FindObjectOfType<Num100Controller>();

    public Animator anim;

    public float timeCD_Find;
    public float timeCD_Bomb;

    public MyCountdown cdmg_Find, cdmg_Bomb;

    bool isOpening = false;

    public void OnOff()
    {
        Debug.Log("Clicked");

        anim.Play(isOpening ? "default" : "open");
        isOpening = !isOpening;
    }

    public void FindCurrentTarget()
    {
        cdmg_Find.timeCd = timeCD_Find;

        cdmg_Find.StartCD(delegate {
            // zoom the target
            GameObject target = GameObject.Find(manager.GetTargetName());

            RectTransform rt = target.GetComponent<RectTransform>();
            rt.localScale *= DEFINE.scaleRate;
            rt.SetAsLastSibling();

            StartCoroutine(MinigameToolKit.Shake(rt, MinigameToolKit.LockAxis.None, 5f, 5f));
        });
    }

    public void CreateTimingBomb(GameObject bomb)
    {
        cdmg_Bomb.timeCd = timeCD_Bomb;

        cdmg_Bomb.StartCD(delegate
        {
            int randomTarget = UnityEngine.Random.Range(0, NumberManager.values.Count);
            Instantiate(bomb, GameObject.Find(DEFINE.NUMBER + randomTarget).transform);
            StartCoroutine(BOOM(randomTarget, delegate
            {
                manager.ClickHandle(GameObject.Find(DEFINE.NUMBER + randomTarget));
            }));
        });
    }

    IEnumerator BOOM(int target, Action callback)
    {
        while (manager.NumTarget != target )
        {
            yield return null;
        }
        callback?.Invoke();
    }
}
