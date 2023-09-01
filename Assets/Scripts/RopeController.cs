using System.Collections;
using UnityEngine;

public class RopeController : Singleton<RopeController>
{
    public float minLength = 2f;
    public float maxLength = 10f;
    public float rotateSpeed = 45f;
    public float elongatedSpeed = 5f;
    public float retractionSpeed = 10f;
    public Vector2 rotateRange = new Vector2(-45f, 45f);

    private LineRenderer ropeRenderer;
    //[SerializeField] 
    private bool isSwinging = true;
    //[SerializeField] 
    private bool isExtending = false;
    //[SerializeField]
    private bool isRetracting = false;
    //[SerializeField]
    public bool isHooking = false;

    [SerializeField] Animator hoistAnimator;

    [SerializeField] Transform hook;
    [SerializeField] Transform hookContainer;
    Animator hookAnimator;
    GameObject hookedObject;
    [SerializeField] int hookedWeight = 0;

    private float currentRotation = 0f;
    private int rotateDirection = 1;

    private void Start()
    {
        ropeRenderer = GetComponent<LineRenderer>();
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, Vector3.zero);
        ropeRenderer.SetPosition(1, Vector3.down * minLength);

        hookAnimator = hook.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isSwinging)
        {
            RotateRope();
        }

        if (Input.GetMouseButtonDown(0) && isSwinging)
        {
            isSwinging = false;
            isExtending = true;
            StartCoroutine(ExtendRope());
        }

        if (isRetracting)
        {
            RetractRope();
        }

        hook.transform.localPosition = ropeRenderer.GetPosition(1);
    }

    private void RotateRope()
    {
        if (Mathf.Abs(currentRotation) >= Mathf.Abs(rotateRange.y))
        {
            rotateDirection *= -1;
        }

        currentRotation += rotateDirection * rotateSpeed * Time.deltaTime;
        currentRotation = Mathf.Clamp(currentRotation, rotateRange.x, rotateRange.y);

        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    private IEnumerator ExtendRope()
    {
        float currentLength = minLength;

        PlayAnim(hoistAnimator);

        while (isExtending && currentLength < maxLength)
        {
            currentLength += elongatedSpeed * Time.deltaTime;
            Vector3 currentDirect = Vector3.down;
            Vector3 newPosition = transform.position + currentDirect.normalized * currentLength;
            ropeRenderer.SetPosition(1, newPosition);
            yield return null;
        }

        isExtending = false;
        isRetracting = true;

        PlayAnimInvert(hoistAnimator);
    }

    private void RetractRope()
    {
        Vector3 direction = Vector3.up;
        float distance = Mathf.Abs(ropeRenderer.GetPosition(1).y);

        if (hookedObject == null) hookedWeight = 0;

        if (distance > minLength)
        {
            direction.Normalize();
            Vector3 newPosition = ropeRenderer.GetPosition(1) + (retractionSpeed - hookedWeight) * Time.deltaTime * direction;
            ropeRenderer.SetPosition(1, newPosition);
        }
        else
        {
            isRetracting = false;
            isSwinging = true;

            ropeRenderer.SetPosition(1, Vector3.down * minLength);

            if (hookedObject != null)
            {
                GameManager.Instance.AddCoin(hookedWeight);
                Destroy(hookedObject);
            }

            StopAnim(hoistAnimator);
            StopAnim(hookAnimator);
        }
    }

    public void RegisterObject(GameObject obj)
    {
        if (hookedObject != null)
            return;

        PlayAnim(hookAnimator);

        hookedObject = obj;
        hookedObject.transform.SetParent(hookContainer);
        hookedObject.transform.localPosition = Vector3.zero;

        hookedWeight = hookedObject.GetComponent<PickupObject>().MyWeight;

        isExtending = false;
        isRetracting = true;
    }

    void PlayAnim(Animator animator)
    {
        animator.Play("play");
    }

    void PlayAnimInvert(Animator animator)
    {
        animator.Play("playInvert");
    }

    void StopAnim(Animator animator)
    {
        animator.Play("nothing");
    }
}

