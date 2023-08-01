using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : SingletonMonoBehaviour<PageManager>
{
    public RectTransform ScrollContent;
    public Transform StepContainer;
    public GameObject StepObj;

    [SerializeField] float pageWidth = 1334;
    public int numPage = 5;

    [SerializeField] float moveDuration = 0.5f;

    [SerializeField] float defaultSize = 50f, showSize = 75f;
    [SerializeField] Color defaultColor, showColor;

    static int currentPage = 0;

    Vector2 startMousePos, endMousePos;
    bool isDragging = false, isMoving = false;

    public void InitStep()
    {
        // Clear
        for (int i = 0; i < StepContainer.childCount; i++)
        {
            Destroy(StepContainer.GetChild(i).gameObject);
        }
        // Init
        for (int i = 0; i < numPage; i++)
        {
            Instantiate(StepObj, StepContainer);
        }

        ShowPageNum();
    }

    void ShowPageNum()
    {
        for (int i = 0; i < numPage; i++)
        {
            // Show by resize
            int index = i;
            StepContainer.GetChild(index).GetComponent<RectTransform>().sizeDelta = (index == currentPage ? showSize : defaultSize) * Vector2.one;
            StepContainer.GetChild(index).GetComponent<Image>().color = (index == currentPage ? showColor : defaultColor);
        }
    }

    private void OnMouseDrag()
    {
        endMousePos = Input.mousePosition;
        isDragging = Mathf.Abs(startMousePos.x - endMousePos.x) > 50f;
    }

    private void OnMouseDown()
    {
        startMousePos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            if (GetDragType() == DragType.Next)
                NextPage();
            else
                PrevPage();
        }
        isDragging = false;
    }

    DragType GetDragType()
    {
        if (startMousePos.x == endMousePos.x)
            return DragType.None;

        return startMousePos.x < endMousePos.x ? DragType.Prev : DragType.Next;
    }

    void NextPage()
    {
        if (currentPage == numPage - 1 || isMoving)
            return;

        currentPage++;
        _ = StartCoroutine(UpdatePosition());
    }

    void PrevPage()
    {
        if (currentPage == 0 || isMoving)
            return;

        currentPage--;
        _ = StartCoroutine(UpdatePosition());
    }

    IEnumerator UpdatePosition()
    {
        if (GetDragType() != DragType.None)
        {
            isMoving = true;
            float timer = 0;

            ShowPageNum();

            while (timer < moveDuration)
            {
                timer += Time.deltaTime;

                ScrollContent.anchoredPosition = Vector2.Lerp(ScrollContent.anchoredPosition, -currentPage * new Vector2(pageWidth, 0), timer / moveDuration);
                yield return new WaitForFixedUpdate();
            }
        }
        ScrollContent.anchoredPosition = -currentPage * new Vector2(pageWidth, 0);
        isMoving = false;
    }

    public void Reset()
    {
        isMoving = false;
        isDragging = false;
        _ = StartCoroutine(UpdatePosition());
    }

    private void OnEnable()
    {
        Reset();
    }

    enum DragType { Prev, Next, None }
}
