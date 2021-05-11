using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBoard : MonoBehaviour
{
	[SerializeField]
	int boardDepth = 5, boardWidth = 5;
	[SerializeField]
	[Range(0f, 1f)]
	float reclaimationChance = 0.1f;
	[SerializeField]
	float updateTime = 0.75f;
	[SerializeField]
	GameObject boardPiece;
	[SerializeField]
	PieceSO[] pieceTypes;

	List<List<BoardSpace>> boardSpaces;

	void Start()
	{
		InitializeBoard();
		StartCoroutine(RandomBoardUpdate());
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
				pieceBS.SetPieceType(pieceTypes[Random.Range(0, pieceTypes.Length)]);
				boardSpaces[depth].Add(pieceBS);
			}
		}
	}

	private IEnumerator RandomBoardUpdate()
	{
		while (true)
		{
			yield return new WaitForSeconds(updateTime);
			foreach (var row in boardSpaces)
				foreach (var space in row)
				{
					if (Random.value <= reclaimationChance)
						space.SetPieceType(pieceTypes[Random.Range(0, pieceTypes.Length)]);
				}
		}
	}
}
