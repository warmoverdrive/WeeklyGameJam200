using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    int startYPos = 100, shownYPos = 350;
    [SerializeField]
    int animSpeed = 60;
    [SerializeField]
    Text title, text, cost;
    [SerializeField]
    Image image;
    [SerializeField]
    CardSO cardType;
    ToolTip tooltip;
    RectTransform rectTransform;

    bool tooltipShown = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(CardInit());
    }

    public void SetCardType(CardSO newCardType)
    {
        cardType = newCardType;
        title.text = cardType.cardName;
        text.text = cardType.cardRule;
        cost.text = $"${cardType.cardCost}";
        image.sprite = cardType.cardImage;
    }

    private IEnumerator CardInit()
    {
        while (tooltip == null)
        {
            tooltip = FindObjectOfType<ToolTip>();
            yield return new WaitForEndOfFrame();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (tooltipShown)
                HideTooltip();
            else
                ShowTooltip();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"{cardType.cardName} clicked!");
        }
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
        HideTooltip();
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

    private void ShowTooltip()
    {
        tooltipShown = true;
        tooltip.ActivateTooltip(cardType.cardName, cardType.cardRule);
    }
    private void HideTooltip()
    {
        tooltipShown = false;
        tooltip.DeactivateTooltip();
    }
}
