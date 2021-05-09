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


    void Start()
    {
        StartTurn();
    }

    public void StartTurn()
    {
        foreach (var pos in cardPositions)
        {
            var newCard = Instantiate(cardPrefab, transform);
            newCard.transform.position = pos.position;
            newCard.SetCardType(deck[Random.Range(0, deck.Length)]);
        }
    }
}
