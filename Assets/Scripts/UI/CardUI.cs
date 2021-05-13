using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

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
	public CardSO cardType { get; private set; }
	ToolTip tooltip;
	RectTransform rectTransform;
	AudioSource audioSource;

	bool tooltipShown = false;

	public static event Action<CardUI> OnCardSelected;

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		audioSource = GetComponent<AudioSource>();
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
			ClickSound();
			if (tooltipShown)
				HideTooltip();
			else
				ShowTooltip();
		}
		else if (eventData.button == PointerEventData.InputButton.Left && GameManager.state == States.PlayerTurn)
		{
			ClickSound();
			OnCardSelected?.Invoke(this);
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

	void ClickSound() => audioSource.PlayOneShot(audioSource.clip);
}
