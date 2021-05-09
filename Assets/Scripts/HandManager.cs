using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField]
    RectTransform[] cardPositions;
    [SerializeField]
    CardSO[] deck;
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
            newCard.SetCardType(deck[Random.Range(0, deck.Length)]);
            cards.Add(newCard);
        }
    }

    void EndTurn()
    {
        foreach (CardUI card in cards)
            Destroy(card.gameObject);
        cards.Clear();
    }

    void RemoveCard(CardUI card)
    {
        cards.Remove(card);
    }
}
