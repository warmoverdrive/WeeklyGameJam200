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

	List<CardUI> cards = new List<CardUI>();

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
			newCard.SetCardType(deck[Random.Range(0, deck.Count)]);
			newCard.index = cards.Count;
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

		deck.Add(cardPool[Random.Range(0, cardPool.Length)]);
	}

	void RemoveCard(CardUI card)
	{
		Destroy(card.gameObject);
	}
}
