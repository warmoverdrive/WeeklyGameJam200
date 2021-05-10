using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
	[SerializeField]
	RectTransform[] cardPositions;
	[SerializeField]
	List<CardSO> deck = new List<CardSO>();
	[SerializeField]
	CardSO[] cardPool;
	[SerializeField]
	CardUI cardPrefab;

	[SerializeField]
	List<CardUI> cards = new List<CardUI>();

	public static event Action OnCompleteEndTurn;

	private void Awake()
	{
		GameManager.OnPlayerTurn += StartTurn;
		GameManager.OnEndTurn += EndTurn;
		GameManager.OnPlayCard += RemoveCard;
	}

	private void OnDestroy()
	{
		GameManager.OnPlayerTurn -= StartTurn;
		GameManager.OnEndTurn -= EndTurn;
		GameManager.OnPlayCard -= RemoveCard;
	}

	void StartTurn()
	{
		foreach (var pos in cardPositions)
		{
			var newCard = Instantiate(cardPrefab, transform);
			newCard.transform.position = pos.position;
			newCard.SetCardType(deck[UnityEngine.Random.Range(0, deck.Count)]);
			cards.Add(newCard);
		}
	}

	void EndTurn()
	{
		for (int i = 0; i < cards.Count; i++)
			if (cards[i] == null)
				continue;
			else
				Destroy(cards[i].gameObject);
		cards.Clear();

		deck.Add(cardPool[UnityEngine.Random.Range(0, cardPool.Length)]);

		OnCompleteEndTurn?.Invoke();
	}

	void RemoveCard(CardUI card)
	{
		Destroy(card.gameObject);
	}
}
