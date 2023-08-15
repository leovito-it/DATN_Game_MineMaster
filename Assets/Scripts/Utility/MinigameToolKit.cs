using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class MinigameToolKit
{
    /// <summary> Instantiate a GameObject in the duration at the position </summary>
    public static void Clone(this GameObject obj, Vector2 position, float duration = 0)
    {
        GameObject newObj = Clone(obj, duration, false);
        newObj.GetComponent<RectTransform>().position = position;
    }

    /// <summary> Show the <b>notice object by instante and destroy all exist</summary>
    public static GameObject Clone(this GameObject obj, float duration = 0, bool destroyPrevClone = false)
    {
        GameObject preNotice = GameObject.Find(obj.name + "(Clone)");

        if (preNotice != null && destroyPrevClone)
            GameObject.Destroy(preNotice);

        GameObject notice = GameObject.Instantiate(obj, obj.transform.parent);
        notice.SetActive(true);

        if (duration != 0)
            GameObject.Destroy(notice, duration);
        return notice;
    }

    /// <summary> Change <b>obj</b> 's sprite </summary>
    public static void ChangeSprite(this GameObject obj, Sprite sprite)
    {
        if (obj.TryGetComponent(out Image img))
        {
            img.sprite = sprite;
            img.SetNativeSize();
        }
    }

    /// <summary> Change sprite </summary>
    public static void ChangeSprite(this Image img, Sprite sprite)
    {
        img.sprite = sprite;
        img.SetNativeSize();
    }

    public static void ChangeSprite(this SpriteRenderer renderer, Sprite sprite)
    {
        renderer.sprite = sprite;
    }

    /// <summary> Make GameObject within the bound </summary>
    public static void MakeInsideRect(this RectTransform pos, RectTransform rect)
    {
        pos.anchoredPosition = rect.anchoredPosition + GetRandomPosition(rect);
    }

    /// <summary> Get a position in the rect that is the sizeDelta of an GameObject</summary>
    public static Vector2 GetRandomPosition(RectTransform rect)
    {
        float randomX = (rect.anchorMin == 0.5f * Vector2.one && rect.anchorMax == 0.5f * Vector2.one) ?
                Random.Range(-0.5f * rect.sizeDelta.x, 0.5f * rect.sizeDelta.x) : Random.Range(-0.5f * rect.rect.size.x, 0.5f * rect.rect.size.x);
        float randomY = (rect.anchorMin == 0.5f * Vector2.one && rect.anchorMax == 0.5f * Vector2.one) ?
                Random.Range(-0.5f * rect.sizeDelta.y, 0.5f * rect.sizeDelta.y) : Random.Range(-0.5f * rect.rect.size.y, 0.5f * rect.rect.size.y);

        return new Vector2(randomX, randomY);
    }

    /// <summary> Play default state of the animator </summary>
    public static void Play(this Animator animator, string stateName = "", float speed = 1)
    {
        animator.enabled = true;
        animator.Rebind();
        animator.Play(stateName);
        animator.speed = speed;
    }

    /// <summary> Disable all </summary>
    public static void Disable(this Component[] disableList)
    {
        foreach (var @object in disableList)
            @object.gameObject.SetActive(false);
    }

    /// <summary> Create after image of the object </summary>
    public static void CreateAfterImage(this Image img, float alphaColor, float showTime, float timeStep, float endTime)
    {
        GameObject container = new("AI Container");
        container.transform.SetParent(img.transform.parent);
        container.transform.SetSiblingIndex(img.transform.GetSiblingIndex());

        DOVirtual.Float(0f, endTime, endTime, (x) =>
        {
            if (x % timeStep != 0)
                return;

            Image newImg = GameObject.Instantiate(img, container.transform);
            newImg.transform.SetAsFirstSibling();

            newImg.color = new Color(1, 1, 1, alphaColor);
            GameObject.Destroy(newImg.gameObject, showTime);
        });

        GameObject.Destroy(container);
    }

    /// <summary>  <para>Spawn with the SpawnOptions</para> </summary>
    public static IEnumerator Spawn(SpawnOptions options)
    {
        int count = 0;
        while (true)
        {
            if (options.container.gameObject.activeInHierarchy)
            {
                if (++count > options.amount)
                    break;

                options.CreateObject();
            }

            yield return new WaitForSeconds(options.timeStep);
        }
    }

    public static void AddForce(this GameObject obj, Vector2 force)
    {
        if (!obj.TryGetComponent(out Rigidbody2D body))
            body = obj.AddComponent<Rigidbody2D>();

        body.gravityScale = 0;
        body.AddForce(force);
    }

    public static IEnumerator Shake(this RectTransform rt, LockAxis lockAxis, float strength, float time)
    {
        float timer = 0f;
        Vector2 start = rt.anchoredPosition;

        while (timer <= time)
        {
            if (rt == null)
                break;

            timer += Time.deltaTime;

            if (lockAxis == LockAxis.None)
                rt.anchoredPosition = start + Random.insideUnitCircle * strength;

            else
                rt.anchoredPosition = lockAxis == LockAxis.X ?
                    start + new Vector2(0, Random.insideUnitCircle.y * strength) : start + new Vector2(Random.insideUnitCircle.x * strength, 0);

            yield return null;
        }
    }

    public enum LockAxis { X, Y, None }

    public enum TransitionType { Out, In }

    public struct SpawnOptions
    {
        /// <summary>
        /// List objects to spawn
        /// </summary>
        public GameObject[] objects;
        public int amount;
        public RectTransform container;
        public float timeStep;
        public bool randomPosition;
        /// <summary> Active when value != 0 </summary>
        public float randomDirect;
        public Vector2 rotateRange;
        public Vector2 scaleRange;
        public NameRule nameRule;

        static float randomValue;

        public GameObject GetObject()
        {
            return objects[Random.Range(0, objects.Length)];
        }

        public bool GetRandomState(Vector2 range)
        {
            bool result = !(range == null || range == Vector2.zero);

            if (result)
                randomValue = Random.Range(Mathf.Min(range.x, range.y), Mathf.Max(range.x, range.y));

            return result;
        }

        public float GetRandomValue()
        {
            return randomValue;
        }

        public GameObject CreateObject()
        {
            GameObject newObj = GameObject.Instantiate(GetObject(), container);
            newObj.SetActive(true);

            RectTransform rt = newObj.GetComponent<RectTransform>();
            RectTransform rtContainer = container.GetComponent<RectTransform>();

            if (randomPosition)
                rt.anchoredPosition = GetRandomPosition(rtContainer);

            if (GetRandomState(rotateRange))
                rt.eulerAngles = GetRandomValue() * Vector3.forward;

            if (GetRandomState(scaleRange))
                rt.localScale = GetRandomValue() * Vector3.one;

            if (randomDirect != 0)
            {
                newObj.AddForce(randomDirect * Random.insideUnitCircle);
            }
            return newObj;
        }

        public enum NameRule { JustName, WithClone, WithOrderNumber }
    }
}