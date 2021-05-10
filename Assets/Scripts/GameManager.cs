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
	GameBoardManager boardManager;
	bool boardProcessed = false;
	bool handProcessed = false;

	private void Awake()
	{
		CardUI.OnCardSelected += UpdateSelectedCard;
		BoardSpace.OnBoardClicked += UpdateBoardSpace;
		Improvement.OnHarvest += UpdateBank;
		GameBoardManager.OnCompleteEndTurn += OnBoardProcessed;
		HandManager.OnCompleteEndTurn += OnHandProcessed;
	}
	private void OnDestroy()
	{
		CardUI.OnCardSelected -= UpdateSelectedCard;
		BoardSpace.OnBoardClicked -= UpdateBoardSpace;
		Improvement.OnHarvest -= UpdateBank;
		GameBoardManager.OnCompleteEndTurn -= OnBoardProcessed;
		HandManager.OnCompleteEndTurn -= OnHandProcessed;
	}

	private void Start()
	{
		// trigger BoardManager building board, returning a list of tiles
		boardManager = FindObjectOfType<GameBoardManager>();
		InitializeUI();
		StartPlayerTurn();
	}

	void InitializeUI()
	{
		OnBankUpdate?.Invoke(bank);
		OnTurnCountUpdate?.Invoke(turnCount);
	}

	void StartPlayerTurn()
	{
		// Set cash (passive income?)
		OnPlayerTurn?.Invoke();
		state = States.PlayerTurn;
		boardProcessed = false;
		handProcessed = false;
	}

	public void EndTurn()
	{
		state = States.EndTurn;
		StartCoroutine(ProcessEndTurn());
	}

	IEnumerator ProcessEndTurn()
	{
		OnEndTurn?.Invoke();

		// Make sure End Turn processing is complete
		while (boardProcessed == false || handProcessed == false)
			yield return new WaitForEndOfFrame();

		// wait for a second to show the player things have been processed
		yield return new WaitForSeconds(turnDelay);

		turnCount++;
		OnTurnCountUpdate?.Invoke(turnCount);

		StartPlayerTurn();
	}

	private void UpdateSelectedCard(CardUI card)
		=> selectedCard = card;

	private void UpdateBoardSpace(BoardSpace space)
	{
		if (selectedCard == null) return;

		if (selectedCard.cardType.cardCost > bank) return; // TODO send some warning to player

		if (Array.Exists<PieceSO>(selectedCard.cardType.validPieces, piece => piece.pieceName == space.pieceType.pieceName))
		{
			if (selectedCard.cardType.isImprovement)
			{
				var improvement = space.GetComponentInChildren<Improvement>();
				if (improvement.improvementType)
					return;
				else
					improvement.SetImprovementType(selectedCard.cardType.newImprovementSO);
			}
			else
				space.SetPieceType(selectedCard.cardType.newPieceSO);

			OnPlayCard?.Invoke(selectedCard);
			UpdateBank(-selectedCard.cardType.cardCost);
			selectedCard = null;
		}
	}

	void UpdateBank(int value)
	{
		bank += value;
		OnBankUpdate?.Invoke(bank);
	}

	void OnBoardProcessed() => boardProcessed = true;
	void OnHandProcessed() => handProcessed = true;
}
