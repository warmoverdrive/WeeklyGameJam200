using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardManager : MonoBehaviour
{
	[SerializeField]
	int boardDepth = 5, boardWidth = 5;
	[SerializeField]
	[Range(0f, 1f)]
	float reclaimationChance = 0.1f;
	[SerializeField]
	GameObject boardPiece;
	[SerializeField]
	PieceSO[] startingPieceTypes;

	List<List<BoardSpace>> boardSpaces;

	public static event Action OnCompleteEndTurn;
	public static event Action<bool> OnImprovementsPresent;

	private void Awake()
	{
		GameManager.OnEndTurn += ExecuteEndTurn;
		GameManager.OnBoardUpdate += UpdateWaterTiles;
	}
	private void OnDestroy()
	{
		GameManager.OnEndTurn -= ExecuteEndTurn;
		GameManager.OnBoardUpdate -= UpdateWaterTiles;
	}

	void Start()
	{
		InitializeBoard();
		StoreNeighbours();
	}

	private void InitializeBoard()
	{
		boardSpaces = new List<List<BoardSpace>>();

		for (int depth = 0; depth < boardDepth; depth++)
		{
			boardSpaces.Add(new List<BoardSpace>());

			for (int width = 0; width < boardWidth; width++)
			{
				var piece = Instantiate(boardPiece, new Vector3(width, 0, depth), Quaternion.identity, transform);
				var pieceBS = piece.GetComponent<BoardSpace>();
				pieceBS.SetPieceType(startingPieceTypes[UnityEngine.Random.Range(0, startingPieceTypes.Length)]);
				boardSpaces[depth].Add(pieceBS);
			}
		}
	}

	private void StoreNeighbours()
	{
		for (int row = 0; row < boardDepth; row++)
			for (int col = 0; col < boardWidth; col++)
				boardSpaces[row][col].neighbors = CheckNeighbors(row, col);
	}

	private void ExecuteEndTurn() => StartCoroutine(EndTurn());

	private IEnumerator EndTurn()
	{
		yield return StartCoroutine(CheckWaterTiles());
		yield return StartCoroutine(CheckReclaimation());
		CheckIfImprovementsExist();
		OnCompleteEndTurn?.Invoke();
	}

	private void UpdateWaterTiles() => StartCoroutine(CheckWaterTiles());

	private IEnumerator CheckWaterTiles()
	{
		foreach (var row in boardSpaces)
			foreach (var space in row)
			{
				if (space.pieceType.pieceName == "Water")
				{
					foreach (var neighbor in space.neighbors)
					{
						var neighborImprovement = neighbor.GetComponentInChildren<Improvement>();
						if (neighborImprovement.improvementType == null) continue;
						if (neighborImprovement.IsWatered() == false)
							yield return StartCoroutine(neighborImprovement.WaterImprovement());
					}
				}
			}
	}

	private IEnumerator CheckReclaimation()
	{
		foreach (var row in boardSpaces)
			foreach (var space in row)
			{
				if (space.pieceType.reclaimationType == null) continue;
				if (space.GetComponentInChildren<Improvement>().improvementType == null)
					if (UnityEngine.Random.value <= reclaimationChance)
						yield return StartCoroutine(
							space.Reclaimation(
								startingPieceTypes[UnityEngine.Random.Range(
									0, startingPieceTypes.Length)]));

			}
	}

	private void CheckIfImprovementsExist()
	{
		foreach (var row in boardSpaces)
			foreach (var space in row)
			{
				if (space.GetComponentInChildren<Improvement>().improvementType != null)
				{
					OnImprovementsPresent?.Invoke(true);
					return;
				}
			}
		OnImprovementsPresent?.Invoke(false);
	}

	private List<BoardSpace> CheckNeighbors(int row, int col)
	{
		List<BoardSpace> neighbors = new List<BoardSpace>();

		if (row - 1 >= 0) neighbors.Add(boardSpaces[row - 1][col]);
		if (row + 1 < boardDepth) neighbors.Add(boardSpaces[row + 1][col]);
		if (col - 1 >= 0) neighbors.Add(boardSpaces[row][col - 1]);
		if (col + 1 < boardWidth) neighbors.Add(boardSpaces[row][col + 1]);

		return neighbors;
	}
}
