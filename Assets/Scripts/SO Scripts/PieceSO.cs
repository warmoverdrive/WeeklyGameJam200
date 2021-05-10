using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Piece Data", menuName = "ScriptableObjects/PieceData", order = 1)]
public class PieceSO : ScriptableObject
{
	public string pieceName;
	[TextArea]
	public string rules;
	public Color color;
	public bool canImprove;
	public PieceSO reclaimationType;
}
