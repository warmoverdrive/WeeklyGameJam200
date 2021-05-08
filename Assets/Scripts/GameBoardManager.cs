using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardManager : MonoBehaviour
{
    [SerializeField]
    int boardDepth = 5, boardWidth = 5;
    [SerializeField]
    GameObject boardPiece;
    [SerializeField]
    PieceSO[] startingPieceTypes;

    void Start()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int depth = 0; depth < boardDepth; depth++)
        {
            for (int width = 0; width < boardWidth; width++)
            {
                var piece = Instantiate(boardPiece, new Vector3(width, 0, depth), Quaternion.identity, transform);
                piece.GetComponent<BoardSpace>().SetPieceType(startingPieceTypes[Random.Range(0, startingPieceTypes.Length)]);
            }
        }
    }
}
