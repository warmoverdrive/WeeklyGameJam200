using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    int startYPos = 100, shownYPos = 350;
    [SerializeField]
    int animSpeed = 60;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ShowHand());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(HideHand());
    }

    IEnumerator ShowHand()
    {
        while (rectTransform.anchoredPosition.y < shownYPos)
        {
            rectTransform.anchoredPosition += new Vector2(0, animSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, shownYPos);
    }

    IEnumerator HideHand()
    {
        while (rectTransform.anchoredPosition.y > startYPos)
        {
            rectTransform.anchoredPosition -= new Vector2(0, animSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startYPos);
    }
}
