using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : Singleton<RopeController>
{
    public Transform ropeOrigin;
    public float minLength = 2f;
    public float maxLength = 10f;
    public float rotateSpeed = 45f;
    public float elongatedSpeed = 5f;
    public float retractionSpeed = 10f;
    public Vector2 rotateRange = new Vector2(-45f, 45f);

    private LineRenderer ropeRenderer;
    [SerializeField] private bool isSwinging = true;
    [SerializeField] private bool isExtending = false;
    [SerializeField] private bool isRetracting = false;

    [SerializeField] Transform hook;
    GameObject hookedObject;

    private float currentRotation = 0f;
    private int rotateDirection = 1;

    private void Start()
    {
        ropeRenderer = GetComponent<LineRenderer>();
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, ropeOrigin.position);
        ropeRenderer.SetPosition(1, ropeOrigin.position + Vector3.down * minLength);
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
            if (hookedObject != null)
            {
                hookedObject.transform.localPosition = ropeOrigin.localPosition + ropeRenderer.GetPosition(1);
            }
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

        Vector3 direction = Quaternion.Euler(0f, 0f, currentRotation) * Vector3.down;
        Vector3 newPosition = ropeOrigin.position + direction * minLength;

        ropeRenderer.SetPosition(1, newPosition);
    }

    private IEnumerator ExtendRope()
    {
        float currentLength = minLength;

        while (isExtending && currentLength < maxLength)
        {
            currentLength += elongatedSpeed * Time.deltaTime;
            Vector3 currentDirect = ropeRenderer.GetPosition(1) - ropeOrigin.position;
            Vector3 newPosition = ropeOrigin.position + currentDirect.normalized * currentLength;
            ropeRenderer.SetPosition(1, newPosition);
            yield return null;
        }

        isExtending = false;
        isRetracting = true;
    }

    private void RetractRope()
    {
        Vector3 direction = ropeOrigin.position - ropeRenderer.GetPosition(1);
        float distance = direction.magnitude;

        if (distance > minLength)
        {
            direction.Normalize();
            Vector3 newPosition = ropeRenderer.GetPosition(1) + retractionSpeed * Time.deltaTime * direction;
            ropeRenderer.SetPosition(1, newPosition);
        }
        else
        {
            isRetracting = false;
            ropeRenderer.SetPosition(1, ropeOrigin.position);
            isSwinging = true;

            if (hookedObject != null)
            {
                Destroy(hookedObject);
            }
        }
    }

    public void RegisterObject(GameObject obj)
    {
        hookedObject = obj;
        isExtending = false;
        isRetracting = true;
    }
}

