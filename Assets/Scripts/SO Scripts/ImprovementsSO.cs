using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Improvement Data", menuName = "ScriptableObjects/ImprovementData", order = 1)]
public class ImprovementsSO : ScriptableObject
{
	public string improvementName;
	[TextArea]
	public string improvementRules;
	public bool canGrow;
	public bool needsWater;
	public int growTime;
	public int harvestIncome;
	public GameObject modelPrefab;
	public Color color;
	public PieceSO tileChangeOnHarvest;
}
