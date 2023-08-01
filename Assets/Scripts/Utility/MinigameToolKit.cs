using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MinigameToolKit : MonoBehaviour
{
    /// <summary> Instantiate a GameObject in the duration at the pos </summary>
    public static void ShowNotice(GameObject obj, Vector2 pos, float duration)
    {
        obj.GetComponent<RectTransform>().position = pos;
        ShowNotice(obj, duration, false);
    }

    /// <summary> Show the <b>notice object by instante and destroy all exist</summary>
    public static GameObject ShowNotice(GameObject obj, float duration, bool destroyPre)
    {
        GameObject preNotice = GameObject.Find(obj.name + "(Clone)");

        if (preNotice != null && destroyPre)
            Destroy(preNotice);

        GameObject notice = Instantiate(obj, obj.transform.parent);
        notice.SetActive(true);

        Destroy(notice, duration);
        return notice;
    }

    /// <summary> Change <b>obj</b> 's sprite after <b>delay time</b></summary>
    public static IEnumerator DelayChangeSprite(GameObject obj, Sprite sprite, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ChangeSprite(obj, sprite);
    }

    /// <summary> Change <b>obj</b> 's sprite </summary>
    public static void ChangeSprite(GameObject obj, Sprite sprite)
    {
        if (obj.TryGetComponent(out Image img))
        {
            obj.GetComponent<RectTransform>().sizeDelta = sprite.rect.size;
            img.sprite = sprite;
        }
    }

    /// <summary> Play <b>clip</b> using AudioManager script</summary>
    public static void PlayEF(AudioClip clip)
    {
        if (AudioManager.Instance != null && clip != null)
            AudioManager.Instance.PlaySE(clip, false);
    }
    /// <summary> Stop All EF</summary>
    public static void StopAllEF()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopSE();
    }

    /// <summary> Play <b>clip</b> using AudioManager script, can stop when play other EF</summary>
    public static void PlayEF_AutoStop(AudioClip clip)
    {
        AudioManager.Instance.StopSE();
        PlayEF(clip);
    }

    /// <summary> Play <b>clip</b> using AudioManager script</summary>
    public static IEnumerator PlayEF(AudioClip clip, float duration)
    {
        if (AudioManager.Instance != null && clip != null)
        {
            float timer = 0f;
            while (timer < duration)
            {
                timer += clip.length;
                AudioManager.Instance.PlaySE(clip,false);

                yield return new WaitForSeconds(clip.length);
            }
        }
    }

    /// <summary> Play EF after delay time </summary>
    public static IEnumerator DelayPlayEF(AudioClip clip, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PlayEF(clip);
    }

    /// <summary> Reload scene after delay time </summary>
    public static IEnumerator ReloadScene(string sceneName, float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        if (UnityEngine.SceneManagement.SceneManager.sceneCount > 1)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetSceneAt(1).name);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    /// <summary> Make GameObject within the bound </summary>
    public static void SetRandomPos(RectTransform pos, RectTransform bound)
    {
        pos.anchoredPosition = bound.anchoredPosition + GetRandomPosition(bound);
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

    /// <summary> Zoom in/out GameObject </summary>
    public static IEnumerator ZoomObject(RectTransform rt, float maxScale, float zoomTime)
    {
        float timer = 0;
        Vector2 startScale = rt.localScale;
        if (rt.TryGetComponent(out Animator animator))
        {
            animator.enabled = false;
        }

        while (timer <= zoomTime)
        {
            timer += Time.deltaTime;
            rt.localScale = startScale + (timer / zoomTime * (maxScale - startScale.x) * Vector2.one);
            yield return null;
        }
    }

    /// <summary> Fade the object in the time </summary>
    public static IEnumerator FadeObject(GameObject @object, TransitionType type, float fadeTime)
    {
        float timer = 0;
        Image img = @object.GetComponent<Image>();
        Color currentColor = img.color;
        float currentAlpha = img.color.a;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;

            Color modified = type == TransitionType.Out ?
                new Color(0, 0, 0, (0 - currentAlpha) * (timer / fadeTime)) :
                new Color(0, 0, 0, (1 - currentAlpha) * (timer / fadeTime));

            img.color = currentColor + modified;
            yield return null;
        }
    }

    /// <summary> Move GameObject to end position </summary>
    public static IEnumerator MoveObject(RectTransform rt, Vector3 endPos, float moveTime)
    {
        // moving
        Vector3 startPos = rt.anchoredPosition;
        Vector3 normalize = Vector3.Normalize(endPos - (Vector3)rt.anchoredPosition);
        float distance = Vector3.Distance(rt.anchoredPosition, endPos);

        float timer = 0;
        while (timer <= moveTime)
        {
            timer += Time.deltaTime;
            rt.anchoredPosition = startPos + (distance * timer / moveTime * normalize); // x = x0 + vector(v) * space(start, end) * percent(t);

            yield return null;
        }
        rt.anchoredPosition = endPos;
    }

    /// <summary> Use object's animator to play current state </summary>
    public static IEnumerator PlayAnimation(GameObject animationObject, float delayTime)
    {
        animationObject.SetActive(true);
        yield return new WaitForSeconds(delayTime);

        if (animationObject.TryGetComponent(out Animator animator))
        {
            PlayAnimation(animator);
        }
    }

    /// <summary> Play default state of the animator </summary>
    public static void PlayAnimation(Animator animator)
    {
        animator.enabled = true;
        animator.Rebind();
    }

    /// <summary> Play the exist state of the animator in the time </summary>
    public static IEnumerator PlayAnimation(Animator animator, string stateName, float playTime)
    {
        PlayAnimation(animator);
        animator.Play(stateName);

        yield return new WaitForSeconds(playTime);
        animator.enabled = false;
    }

    /// <summary> Active an object in the time</summary>
    public static IEnumerator ActiveObject(GameObject @object, float activeTime)
    {
        @object.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        @object.SetActive(false);
    }

    /// <summary> Disable all objects in the array </summary>
    public static void DisableObject(GameObject[] disableList)
    {
        foreach (GameObject @object in disableList)
            @object.SetActive(false);
    }

    /// <summary> Create after image of the object </summary>
    public static IEnumerator CreateAfterImage(GameObject obj, float alphaColor, float showTime, float timeStep, float endTime)
    {
        float timer = 0;
        GameObject newObject = new GameObject();
        GameObject parent = Instantiate(newObject, obj.transform.parent);
        parent.name = "After Image Object Parent";
        parent.transform.SetSiblingIndex(obj.transform.GetSiblingIndex());

        while (timer <= endTime)
        {
            timer += timeStep;
            GameObject afterObject = Instantiate(obj, parent.transform);
            afterObject.transform.SetSiblingIndex(0);
            Image afterImage = afterObject.GetComponent<Image>();

            afterImage.color = new Color(1, 1, 1, alphaColor);
            Destroy(afterObject, showTime);

            yield return new WaitForSeconds(timeStep);
        }
        Destroy(newObject);
        Destroy(parent);
    }

    public static void PlayEF(AudioClip clip, float from, float to)
    {
        if (AudioManager.Instance != null && clip != null)
            AudioManager.Instance.PlaySEInterval(clip, from, to);
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

    public static void AddForce(GameObject obj, Vector2 force)
    {
        if (obj.GetComponent<Rigidbody2D>() == null)
            obj.AddComponent<Rigidbody2D>();

        Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
        body.gravityScale = 0;

        body.AddForce(force);
    }

    public static IEnumerator Shake(RectTransform rt, LockAxis lockAxis, float strength, float time)
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
            GameObject newObj = Instantiate(GetObject(), container);
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
                AddForce(newObj, randomDirect * Random.insideUnitCircle);
            }
            return newObj;
        }

        public enum NameRule { JustName ,WithClone, WithOrderNumber }
    }
}