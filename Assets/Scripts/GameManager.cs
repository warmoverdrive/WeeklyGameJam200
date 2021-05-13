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
	public static event Action<string> OnTurnCountUpdate;
	public static event Action<CardUI> OnPlayCard;
	public static event Action OnBoardUpdate;
	public static States state;

	int turnCount = 1;
	int bank = 10;
	[SerializeField] int turnLimit = 50;
	[SerializeField] int turnDelay = 2;
	[SerializeField] CardUI selectedCard;
	[SerializeField] AudioClip endTurnSound;
	GameBoardManager boardManager;
	AudioSource audioSource;
	UIManager uiManager;
	bool boardProcessed = false;
	bool handProcessed = false;
	bool improvementsPresent = false;

	private void Awake()
	{
		CardUI.OnCardSelected += UpdateSelectedCard;
		BoardSpace.OnBoardClicked += UpdateBoardSpace;
		Improvement.OnHarvest += UpdateBank;
		GameBoardManager.OnCompleteEndTurn += OnBoardProcessed;
		GameBoardManager.OnImprovementsPresent += OnImprovementsPresent;
		HandManager.OnCompleteEndTurn += OnHandProcessed;
	}
	private void OnDestroy()
	{
		CardUI.OnCardSelected -= UpdateSelectedCard;
		BoardSpace.OnBoardClicked -= UpdateBoardSpace;
		Improvement.OnHarvest -= UpdateBank;
		GameBoardManager.OnCompleteEndTurn -= OnBoardProcessed;
		GameBoardManager.OnImprovementsPresent -= OnImprovementsPresent;
		HandManager.OnCompleteEndTurn -= OnHandProcessed;
	}

	private void Start()
	{
		// trigger BoardManager building board, returning a list of tiles
		boardManager = FindObjectOfType<GameBoardManager>();
		uiManager = GetComponent<UIManager>();
		audioSource = GetComponent<AudioSource>();
		InitializeUI();
		StartPlayerTurn();
	}

	void InitializeUI()
	{
		OnBankUpdate?.Invoke(bank);
		OnTurnCountUpdate?.Invoke($"{turnCount}/{turnLimit}");
	}

	void StartPlayerTurn()
	{
		// Set cash (passive income?)
		OnPlayerTurn?.Invoke();
		OnBoardUpdate?.Invoke();
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
		audioSource.PlayOneShot(endTurnSound);
		yield return new WaitForSeconds(endTurnSound.length);
		OnEndTurn?.Invoke();

		// Make sure End Turn processing is complete
		while (boardProcessed == false || handProcessed == false)
			yield return new WaitForEndOfFrame();

		// wait for a second to show the player things have been processed
		yield return new WaitForSeconds(turnDelay);

		if (IsGameOver())
			EndGame(hasWon: false);
		else
		{
			turnCount++;
			if (turnCount > turnLimit)
			{
				EndGame(hasWon: true);
			}
			else
			{
				OnTurnCountUpdate?.Invoke($"{turnCount}/{turnLimit}");
				StartPlayerTurn();
			}
		}

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
			audioSource.PlayOneShot(selectedCard.cardType.cardSFX);
			UpdateBank(-selectedCard.cardType.cardCost);
			selectedCard = null;
			OnBoardUpdate.Invoke();
		}
	}

	bool IsGameOver() => (bank == 0 && !improvementsPresent);

	void UpdateBank(int value)
	{
		bank += value;
		OnBankUpdate?.Invoke(bank);
	}

	void OnBoardProcessed() => boardProcessed = true;
	void OnHandProcessed() => handProcessed = true;
	void OnImprovementsPresent(bool hasImprovements) => improvementsPresent = hasImprovements;

	private void EndGame(bool hasWon) => uiManager.ToggleGameOver(bank, hasWon);
}
