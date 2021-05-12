using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Piece Data", menuName = "ScriptableObjects/PieceData", order = 1)]
public class PieceSO : ScriptableObject
{
	public string pieceName;
	[TextArea]
	public string rules;
	public GameObject model;
	public bool canImprove;
	public GameObject[] availableModelsForMenu;
	public PieceSO reclaimationType;
}
