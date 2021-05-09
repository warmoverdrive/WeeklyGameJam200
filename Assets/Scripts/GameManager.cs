using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum States { PlayerTurn, EndTurn };

public class GameManager : MonoBehaviour
{

    public static event Action OnPlayerTurn;
    public static event Action OnEndTurn;
    public static event Action<int> OnBankUpdate;
    public static event Action<int> OnTurnCountUpdate;
    public static event Action<CardUI> OnPlayCard;
    public static States state;

    int turnCount = 1;
    int bank = 10;
    [SerializeField] int turnDelay = 2;
    [SerializeField] CardUI selectedCard;

    private void Awake()
    {
        CardUI.OnCardSelected += UpdateSelectedCard;
        BoardSpace.OnBoardClicked += UpdateBoardSpace;
    }
    private void OnDestroy()
    {
        CardUI.OnCardSelected -= UpdateSelectedCard;
        BoardSpace.OnBoardClicked -= UpdateBoardSpace;
    }

    private void Start()
    {
        // trigger BoardManager building board, returning a list of tiles
        InitializeUI();
        StartPlayerTurn();
    }

    void InitializeUI()
    {
        OnBankUpdate.Invoke(bank);
        OnTurnCountUpdate.Invoke(turnCount);
    }

    void StartPlayerTurn()
    {
        // Set cash (passive income?)
        // Deal Hand
        // Set State to PlayerTurn
        OnPlayerTurn.Invoke();
        state = States.PlayerTurn;
    }

    public void EndTurn()
    {
        // Check hazards
        // Calculate tiles
        // Add card to deck
        // turn count ++
        // Start Player TurnCount
        Debug.Log("Ending turn");
        state = States.EndTurn;
        OnEndTurn.Invoke();
        StartCoroutine(EndOfTurnDelay());
    }

    IEnumerator EndOfTurnDelay()
    {
        yield return new WaitForSeconds(turnDelay);
        Debug.Log("Starting new turn");
        turnCount++;
        OnTurnCountUpdate.Invoke(turnCount);
        StartPlayerTurn();
    }

    private void UpdateSelectedCard(CardUI card)
        => selectedCard = card;

    private void UpdateBoardSpace(BoardSpace space)
    {
        if (selectedCard == null) return;

        // check cost

        if (Array.Exists<PieceSO>(selectedCard.cardType.validPieces, piece => piece.pieceName == space.pieceType.pieceName))
        {
            if (selectedCard.cardType.isImprovement)
                space.GetComponentInChildren<Improvement>().SetImprovementType(selectedCard.cardType.newImprovementSO);
            else
                space.SetPieceType(selectedCard.cardType.newPieceSO);

            OnPlayCard.Invoke(selectedCard);
            selectedCard = null;
        }
    }
}
