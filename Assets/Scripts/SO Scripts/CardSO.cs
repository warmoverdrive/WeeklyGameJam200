using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardSO : ScriptableObject
{
	public string cardName;
	[TextArea]
	public string cardRule;
	public int cardCost;
	public Sprite cardImage;
	public AudioClip cardSFX;
	public bool isImprovement;
	public PieceSO newPieceSO;
	public ImprovementsSO newImprovementSO;
	public PieceSO[] validPieces;
}
